resource "azurerm_resource_group" "web-rg" {
  name     = "${local.rg_prefix}-web"
  location = var.location
  tags     = local.common_tags
}