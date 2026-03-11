resource "azurerm_resource_group" "translator-rg" {
  name     = "${local.prefix}rg-uks-cl-translator"
  location = local.location
  tags     = local.common_tags
}

resource "azurerm_cognitive_account" "ai-translator" {
  name                = "${local.service_prefix}-ai-translation"
  location            = azurerm_resource_group.translator-rg.location
  resource_group_name = azurerm_resource_group.translator-rg.name
  kind                = "TextTranslation" # Specifies the Translator service
  sku_name            = "S1"
  tags                = local.common_tags
}
