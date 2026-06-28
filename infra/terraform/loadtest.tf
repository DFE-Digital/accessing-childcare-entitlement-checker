resource "azurerm_resource_group" "load_test_rg" {
  count    = var.enable_load_testing ? 1 : 0
  name     = "${local.rg_prefix}-alt"
  location = var.location
  tags     = local.common_tags
}

resource "azurerm_load_test" "load_test" {
  count               = var.enable_load_testing ? 1 : 0
  name                = "${local.service_prefix}-alt"
  location            = azurerm_resource_group.load_test_rg[0].location
  resource_group_name = azurerm_resource_group.load_test_rg[0].name
  tags                = local.common_tags
}
