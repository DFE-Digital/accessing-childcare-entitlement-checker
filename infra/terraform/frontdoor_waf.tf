resource "azurerm_cdn_frontdoor_firewall_policy" "web_firewall_policy" {
  name                = "webFirewallPolicy"
  resource_group_name = azurerm_resource_group.web-rg.name
  tags                = local.common_tags
  mode                = "Prevention"
  sku_name            = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.sku_name

  dynamic "managed_rule" {
    for_each = var.azure_frontdoor_scale == "Premium" ? [0] : []
    content {
      type    = "Microsoft_DefaultRuleSet"
      version = "2.1"
      action  = "Block"
    }
  }

  dynamic "managed_rule" {
    for_each = var.azure_frontdoor_scale == "Premium" ? [0] : []
    content {
      type    = "Microsoft_BotManagerRuleSet"
      version = "1.1"
      action  = "Block"
    }
  }
}