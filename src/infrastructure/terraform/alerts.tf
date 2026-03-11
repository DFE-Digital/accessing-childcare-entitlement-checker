
resource "azurerm_monitor_metric_alert" "availability-alert" {
  name                = "availability-alert"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_application_insights.application-insights.id]
  description         = "Alert if availability is below ${var.alerting[var.environment_prefix].thresholds.availability}%"
  severity            = 0
  frequency           = "PT1M"
  window_size         = "PT1H"
  enabled             = var.alerting[var.environment_prefix].alerts_enabled
  tags                = local.common_tags

  criteria {
    metric_namespace = "microsoft.insights/components"
    metric_name      = "availabilityResults/availabilityPercentage"
    aggregation      = "Average"
    operator         = "LessThan"
    threshold        = var.alerting[var.environment_prefix].thresholds.availability
  }

  action {
    action_group_id = azurerm_monitor_action_group.service-support-action.id
  }
}

resource "azurerm_monitor_metric_alert" "cpu_alert" {
  name                = "web-app-cpu-alert"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_service_plan.web-app-service-plan.id]
  description         = "Alert if CPU utilisation exceeds ${var.alerting[var.environment_prefix].thresholds.cpu}% for more than 5 minutes"
  severity            = 2
  frequency           = "PT1M"
  window_size         = "PT5M"
  enabled             = var.alerting[var.environment_prefix].alerts_enabled
  tags                = local.common_tags

  criteria {
    metric_namespace = "Microsoft.Web/serverfarms"
    metric_name      = "CpuPercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = var.alerting[var.environment_prefix].thresholds.cpu
  }

  action {
    action_group_id = azurerm_monitor_action_group.service-support-action.id
  }
}

resource "azurerm_monitor_metric_alert" "memory_alert" {
  name                = "web-app-memory-alert"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_service_plan.web-app-service-plan.id]
  description         = "Alert if memory utilisation exceeds ${var.alerting[var.environment_prefix].thresholds.memory}% for more than 5 minutes"
  severity            = 2
  frequency           = "PT1M"
  window_size         = "PT5M"
  enabled             = var.alerting[var.environment_prefix].alerts_enabled
  tags                = local.common_tags

  criteria {
    metric_namespace = "Microsoft.Web/serverfarms"
    metric_name      = "MemoryPercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = var.alerting[var.environment_prefix].thresholds.memory
  }

  action {
    action_group_id = azurerm_monitor_action_group.service-support-action.id
  }
}

resource "azurerm_monitor_metric_alert" "web_app_error_alert" {
  name                = "web-app-error-alert"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_linux_web_app.web-app-service.id]
  description         = "Alert if HTTP 5xx error count exceeds ${var.alerting[var.environment_prefix].thresholds.error}"
  severity            = 0
  frequency           = "PT1M"
  window_size         = "PT30M"
  enabled             = var.alerting[var.environment_prefix].alerts_enabled
  tags                = local.common_tags

  criteria {
    metric_namespace = "Microsoft.Web/sites"
    metric_name      = "Http5xx"
    aggregation      = "Total"
    operator         = "GreaterThan"
    threshold        = var.alerting[var.environment_prefix].thresholds.error
  }

  action {
    action_group_id = azurerm_monitor_action_group.service-support-action.id
  }
}