resource "azurerm_user_assigned_identity" "app_identity" {
  location            = local.location
  name                = "${local.service_prefix}-app-identity"
  resource_group_name = azurerm_resource_group.web-rg.name
  tags                = local.common_tags
}