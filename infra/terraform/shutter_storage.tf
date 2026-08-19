# checkov:skip=CKV_AZURE_59: Public network access must be allowed so Front Door Standard can fetch assets over the public internet endpoint using Managed Identity authentication.
# checkov:skip=CKV_AZURE_206: LRS replication is sufficient and cost-effective for static shutter content.
# checkov:skip=CKV_AZURE_33: Queue service is not used by the shutter static site.
# checkov:skip=CKV2_AZURE_1: Microsoft Managed Keys are sufficient for public non-sensitive static shutter assets.
# checkov:skip=CKV2_AZURE_33: Front Door Standard SKU does not support Private Link to backend origins.
# checkov:skip=CKV2_AZURE_21: Diagnostic settings and Front Door logs are used for monitoring instead of legacy storage logging.
resource "azurerm_storage_account" "shutter" {
  name                            = "${local.storage_prefix}shutter"
  resource_group_name             = azurerm_resource_group.web-rg.name
  location                        = azurerm_resource_group.web-rg.location
  account_tier                    = "Standard"
  account_replication_type        = "LRS"
  account_kind                    = "StorageV2"
  shared_access_key_enabled       = false
  allow_nested_items_to_be_public = false
  min_tls_version                 = "TLS1_2"

  blob_properties {
    delete_retention_policy {
      days = 7
    }
  }

  tags = local.common_tags
}

resource "azurerm_storage_container" "shutter_container" {
  name                  = "shutter"
  storage_account_id    = azurerm_storage_account.shutter.id
  container_access_type = "private"
}

resource "azurerm_role_assignment" "shutter_deploy_role" {
  scope                = azurerm_storage_account.shutter.id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = data.azurerm_client_config.client.object_id
  principal_type       = "ServicePrincipal"
}

resource "azurerm_role_assignment" "frontdoor_shutter_read_role" {
  scope                = azurerm_storage_account.shutter.id
  role_definition_name = "Storage Blob Data Reader"
  principal_id         = azurerm_user_assigned_identity.frontdoor_identity.principal_id
  principal_type       = "ServicePrincipal"
}
