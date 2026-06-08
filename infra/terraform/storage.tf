resource "azurerm_storage_account" "deployment_storage" {
  name                          = "${local.prefix}webappdeploy"
  resource_group_name           = azurerm_resource_group.web-rg.name
  location                      = local.location
  account_tier                  = "Standard"
  account_replication_type      = "RAGRS"
  public_network_access_enabled = false
  shared_access_key_enabled     = false
  tags                          = local.common_tags
}

resource "azurerm_storage_container" "deployments" {
  name                  = "deployments"
  storage_account_id    = azurerm_storage_account.deployment_storage.id
  container_access_type = "private"
}

resource "azurerm_private_dns_zone" "blob_dns_zone" {
  name                = "privatelink.blob.core.windows.net"
  resource_group_name = azurerm_resource_group.web-rg.name
  tags                = local.common_tags
}

resource "azurerm_private_dns_zone_virtual_network_link" "blob_dns_link" {
  name                  = "${local.service_prefix}-blob-dns-link"
  resource_group_name   = azurerm_resource_group.web-rg.name
  private_dns_zone_name = azurerm_private_dns_zone.blob_dns_zone.name
  virtual_network_id    = azurerm_virtual_network.vnet.id
  tags                  = local.common_tags
}

resource "azurerm_private_endpoint" "deployment_pe" {
  name                = "${local.service_prefix}-deployment-pe"
  location            = local.location
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

resource "azurerm_role_assignment" "web_app_storage_reader" {
  scope                = azurerm_storage_account.deployment_storage.id
  role_definition_name = "Storage Blob Data Reader"
  principal_id         = azurerm_user_assigned_identity.app_identity.principal_id
}
