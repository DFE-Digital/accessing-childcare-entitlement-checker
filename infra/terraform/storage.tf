resource "azurerm_storage_account" "deployment_storage" {
  #checkov:skip=CKV2_AZURE_1:Microsoft-managed encryption is sufficient for this storage account
  #checkov:skip=CKV_AZURE_33:Azure Queue Storage is not used by this storage account
  #checkov:skip=CKV_AZURE_206: ZRS is intentionally used to provide zonal resilience aligned with the App Service architecture.
  name                            = "${local.prefix}webappdeploy"
  resource_group_name             = azurerm_resource_group.web-rg.name
  location                        = var.location
  account_tier                    = "Standard"
  account_replication_type        = "ZRS"
  public_network_access_enabled   = false
  shared_access_key_enabled       = false
  min_tls_version                 = "TLS1_2"
  allow_nested_items_to_be_public = false
  access_tier                     = "Smart"
  tags                            = local.common_tags

  blob_properties {
    last_access_time_enabled = true
    delete_retention_policy {
      days = 30
    }
    container_delete_retention_policy {
      days = 30
    }
  }

  network_rules {
    default_action             = "Deny"
    virtual_network_subnet_ids = [azapi_resource.app_subnet.id]
    bypass                     = ["AzureServices"]
  }
}

resource "azurerm_storage_container" "deployments" {
  #checkov:skip=CKV2_AZURE_21:Blob service diagnostics are enabled via azurerm_monitor_diagnostic_setting.blob_logs
  name                  = "deployments"
  storage_account_id    = azurerm_storage_account.deployment_storage.id
  container_access_type = "private"

  depends_on = [
    azurerm_monitor_diagnostic_setting.blob_logs
  ]
}

resource "azurerm_monitor_diagnostic_setting" "blob_logs" {
  name                       = "${local.prefix}-deploy-blob-diagnostics"
  target_resource_id         = "${azurerm_storage_account.deployment_storage.id}/blobServices/default"
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  enabled_log {
    category = "StorageRead"
  }

  enabled_log {
    category = "StorageWrite"
  }

  enabled_log {
    category = "StorageDelete"
  }

  enabled_metric {
    category = "Transaction"
  }
}

resource "azurerm_monitor_diagnostic_setting" "storage_diagnostics" {
  name                       = "${local.prefix}-deploy-storage-diagnostics"
  target_resource_id         = azurerm_storage_account.deployment_storage.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  enabled_metric {
    category = "Transaction"
  }

  enabled_metric {
    category = "Capacity"
  }
}

resource "azurerm_private_dns_zone" "blob_dns_zone" {
  name                = "privatelink.blob.core.windows.net"
  resource_group_name = azurerm_resource_group.web-rg.name
  tags                = merge(local.common_tags, { "Service Offering" = null })
}

resource "azurerm_private_dns_zone_virtual_network_link" "blob_dns_link" {
  name                  = "${local.prefix}-blob-dns-link"
  resource_group_name   = azurerm_resource_group.web-rg.name
  private_dns_zone_name = azurerm_private_dns_zone.blob_dns_zone.name
  virtual_network_id    = azurerm_virtual_network.vnet.id
  tags                  = merge(local.common_tags, { "Service Offering" = null })
}

resource "azurerm_private_endpoint" "deployment_pe" {
  name                = "${local.service_prefix}-deployment-pe"
  location            = var.location
  resource_group_name = azurerm_resource_group.web-rg.name
  subnet_id           = azapi_resource.pe_subnet.id
  tags                = local.common_tags

  private_service_connection {
    name                           = "${local.service_prefix}-deployment-psc"
    private_connection_resource_id = azurerm_storage_account.deployment_storage.id
    is_manual_connection           = false
    subresource_names              = ["blob"]
  }

  private_dns_zone_group {
    name                 = "blob-dns-zone-group"
    private_dns_zone_ids = [azurerm_private_dns_zone.blob_dns_zone.id]
  }

  depends_on = [
    azurerm_private_dns_zone_virtual_network_link.blob_dns_link
  ]
}

resource "azurerm_storage_management_policy" "deployment_policy" {
  storage_account_id = azurerm_storage_account.deployment_storage.id

  rule {
    name    = "cleanup-old-non-retained-packages"
    enabled = true

    filters {
      blob_types = ["blockBlob"]

      match_blob_index_tag {
        name      = "retain"
        operation = "=="
        value     = "false"
      }
    }

    actions {
      base_blob {
        delete_after_days_since_last_access_time_greater_than = 180
      }
    }
  }
}

resource "azurerm_role_assignment" "sp_blob_data_owner" {
  scope                = azurerm_storage_account.deployment_storage.id
  role_definition_name = "Storage Blob Data Owner"
  principal_id         = data.azurerm_client_config.client.object_id
  principal_type       = "ServicePrincipal"
}

resource "azurerm_role_assignment" "web_app_blob_data_owner" {
  scope                = azurerm_storage_account.deployment_storage.id
  role_definition_name = "Storage Blob Data Reader"
  principal_id         = azurerm_user_assigned_identity.app_identity.principal_id
  principal_type       = "ServicePrincipal"
}