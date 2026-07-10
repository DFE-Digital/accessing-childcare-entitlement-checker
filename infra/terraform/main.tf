resource "azurerm_resource_group" "web-rg" {
  name     = "${local.rg_prefix}-web"
  location = var.location
  tags     = local.common_tags
}

resource "azurerm_consumption_budget_resource_group" "web_rg_budget" {
  name              = "${azurerm_resource_group.web-rg.name}-budget"
  resource_group_id = azurerm_resource_group.web-rg.id

  amount     = var.budget_amount_web
  time_grain = "Monthly"

  time_period {
    start_date = "2024-01-01T00:00:00Z"
  }

  notification {
    enabled        = true
    threshold      = var.budget_alert_threshold_forecast
    operator       = "GreaterThan"
    threshold_type = "Forecasted"

    contact_groups = [
      azurerm_monitor_action_group.email_action_group.id
    ]
  }

  notification {
    enabled        = true
    threshold      = var.budget_alert_threshold_actual
    operator       = "GreaterThan"
    threshold_type = "Actual"

    contact_groups = [
      azurerm_monitor_action_group.email_action_group.id
    ]
  }
}