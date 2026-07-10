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

resource "azurerm_consumption_budget_resource_group" "load_test_rg_budget" {
  count             = var.enable_load_testing ? 1 : 0
  name              = "${azurerm_resource_group.load_test_rg[0].name}-budget"
  resource_group_id = azurerm_resource_group.load_test_rg[0].id

  amount     = var.budget_amount_load_test
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
