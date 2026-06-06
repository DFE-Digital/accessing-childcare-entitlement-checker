resource "azurerm_resource_group" "web-rg" {
  name     = "${local.prefix}rg-uks-cec-web"
  location = local.location
  tags     = local.common_tags
}