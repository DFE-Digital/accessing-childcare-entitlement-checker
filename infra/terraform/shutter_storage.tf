resource "azurerm_storage_account" "shutter" {
  name                            = "${local.storage_prefix}shutter"
  resource_group_name             = azurerm_resource_group.web-rg.name
  location                        = azurerm_resource_group.web-rg.location
  account_tier                    = "Standard"
  account_replication_type        = "LRS"
  account_kind                    = "StorageV2"
  shared_access_key_enabled       = true
  allow_nested_items_to_be_public = true
  min_tls_version                 = "TLS1_2"

  tags = local.common_tags
}

resource "azurerm_storage_container" "shutter_container" {
  name                  = "shutter"
  storage_account_id    = azurerm_storage_account.shutter.id
  container_access_type = "blob"
}

resource "azurerm_role_assignment" "shutter_deploy_role" {
  scope                = azurerm_storage_account.shutter.id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = data.azurerm_client_config.client.object_id
  principal_type       = "ServicePrincipal"
}
