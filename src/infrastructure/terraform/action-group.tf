resource "azurerm_monitor_action_group" "service-support-action" {
  name                = "Service support"
  resource_group_name = azurerm_resource_group.web-rg.name
  short_name          = "Support"
  tags                = local.common_tags

  dynamic "email_receiver" {
    for_each = var.alerting[var.environment_prefix].email_alerts_enabled ? ["true"] : []
    content {
      name                    = "send-to-support"
      email_address           = var.support_alert_email
      use_common_alert_schema = true
    }
  }
}