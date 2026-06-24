resource "azurerm_cdn_frontdoor_firewall_policy" "web_firewall_policy" {
  name                = "${local.prefix}webfirewallpolicy"
  resource_group_name = azurerm_resource_group.web-rg.name
  tags                = local.common_tags
  mode                = var.waf_mode
  sku_name            = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.sku_name

  dynamic "managed_rule" {
    for_each = var.waf_enable_managed_rules ? ["apply"] : []
    content {
      type    = "Microsoft_DefaultRuleSet"
      version = "2.1"
      action  = "Block"
    }
  }

  dynamic "managed_rule" {
    for_each = var.waf_enable_managed_rules ? ["apply"] : []
    content {
      type    = "Microsoft_BotManagerRuleSet"
      version = "1.1"
      action  = "Block"
    }
  }
}
