resource "azurerm_log_analytics_workspace" "log-analytics-workspace" {
  name                = "${local.service_prefix}-log-analytics-workspace"
  location            = azurerm_resource_group.web-rg.location
  resource_group_name = azurerm_resource_group.web-rg.name
  retention_in_days   = var.log_analytics_retention_in_days
  daily_quota_gb      = var.log_analytics_daily_quota_gb
  tags                = local.common_tags
}

resource "azurerm_application_insights" "application-insights" {
  name                                  = "${local.service_prefix}-app-insights"
  location                              = azurerm_resource_group.web-rg.location
  resource_group_name                   = azurerm_resource_group.web-rg.name
  application_type                      = "web"
  workspace_id                          = azurerm_log_analytics_workspace.log-analytics-workspace.id
  daily_data_cap_in_gb                  = var.application_insights_daily_data_cap_in_gb
  daily_data_cap_notifications_disabled = false
  sampling_percentage                   = var.application_insights_sampling_percentage
  tags                                  = local.common_tags
}

resource "azurerm_application_insights_standard_web_test" "web-app-test" {
  count = var.enable_web_test ? 1 : 0

  name                    = "${local.service_prefix}-web-app-test"
  description             = "Web application availability test"
  resource_group_name     = azurerm_resource_group.web-rg.name
  location                = azurerm_resource_group.web-rg.location
  application_insights_id = azurerm_application_insights.application-insights.id
  frequency               = 600
  timeout                 = 60
  enabled                 = true
  retry_enabled           = true
  geo_locations           = ["emea-se-sto-edge", "emea-ru-msa-edge"]

  lifecycle {
    ignore_changes = [tags]
  }

  request {
    url = "https://${local.host_name}"
  }

  validation_rules {
    expected_status_code = 200
  }
}

resource "azurerm_storage_account" "log-archive" {
  # checkov:skip=CKV_AZURE_206: Log Analytics Data Export requires shared key access to be enabled for authentication.
  # checkov:skip=CKV_AZURE_190: Log Analytics Data Export does not support firewall/VNet restrictions (must allow All networks).
  # checkov:skip=CKV_AZURE_35: Log Analytics Data Export does not support firewall/VNet restrictions (must allow All networks).
  # checkov:skip=CKV_AZURE_59: Storage account must allow public network access for Log Analytics Data Export to function.
  # checkov:skip=CKV_AZURE_33: Queue service is not used; this account is solely for Log Analytics blob exports.
  # checkov:skip=CKV2_AZURE_40: Log Analytics Data Export requires shared key access to be enabled for authentication.
  # checkov:skip=CKV2_AZURE_33: Log Analytics Data Export does not support private endpoints (must allow All networks).
  # checkov:skip=CKV2_AZURE_1: Log archive uses default Microsoft-managed keys which are sufficient for this data class.
  name                             = "${local.storage_prefix}lawarchive"
  resource_group_name              = azurerm_resource_group.web-rg.name
  location                         = azurerm_resource_group.web-rg.location
  account_tier                     = "Standard"
  account_kind                     = "StorageV2"
  account_replication_type         = "ZRS"
  access_tier                      = "Smart"
  allow_nested_items_to_be_public  = false
  min_tls_version                  = "TLS1_2"
  https_traffic_only_enabled       = true
  shared_access_key_enabled        = true
  sftp_enabled                     = false
  local_user_enabled               = false
  cross_tenant_replication_enabled = false

  blob_properties {
    delete_retention_policy {
      days = 7
    }
    container_delete_retention_policy {
      days = 7
    }
  }

  sas_policy {
    expiration_period = "30.00:00:00"
    expiration_action = "Log"
  }

  network_rules {
    default_action = "Allow"
  }

  tags = local.common_tags
}

resource "azurerm_storage_management_policy" "log-archive-policy" {
  storage_account_id = azurerm_storage_account.log-archive.id

  rule {
    name    = "deleteafter3years"
    enabled = true
    filters {
      blob_types = ["blockBlob"]
    }
    actions {
      base_blob {
        delete_after_days_since_modification_greater_than = 1095
      }
    }
  }
}

resource "azurerm_log_analytics_data_export_rule" "log-export" {
  name                    = "log-export-to-storage"
  resource_group_name     = azurerm_resource_group.web-rg.name
  workspace_resource_id   = azurerm_log_analytics_workspace.log-analytics-workspace.id
  destination_resource_id = azurerm_storage_account.log-archive.id
  enabled                 = true

  table_names = [
    "AppRequests",
    "AppExceptions",
    "AppDependencies",
    "AppMetrics",
    "AzureMetrics",
    "AzureDiagnostics"
  ]
}