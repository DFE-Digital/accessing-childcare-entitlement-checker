resource "azurerm_monitor_action_group" "email_action_group" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-email-action-group"
  resource_group_name = azurerm_resource_group.web-rg.name
  short_name          = "email-alerts"

  email_receiver {
    name                    = "AlertRecipient"
    email_address           = var.alert_email_address
    use_common_alert_schema = true
  }

  lifecycle {
    precondition {
      condition     = var.enable_alerts ? var.alert_email_address != "" : true
      error_message = "The 'alert_email_address' variable must be provided when 'enable_alerts' is set to true."
    }
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "web_test_alert" {
  count               = var.enable_alerts && var.enable_web_test ? 1 : 0
  name                = "${local.service_prefix}-web-test-alert"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_application_insights.application-insights.id]
  severity            = 1
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "microsoft.insights/components"
    metric_name      = "availabilityResults/availabilityPercentage"
    aggregation      = "Average"
    operator         = "LessThan"
    threshold        = 100
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "high_response_time" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-high-response-time"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_application_insights.application-insights.id]
  severity            = 3
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "microsoft.insights/components"
    metric_name      = "requests/duration"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 2000
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "high_exception_rate" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-high-exception-rate"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_application_insights.application-insights.id]
  severity            = 2
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "microsoft.insights/components"
    metric_name      = "exceptions/count"
    aggregation      = "Count"
    operator         = "GreaterThan"
    threshold        = 10
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "app_service_5xx_errors" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-app-service-5xx-errors"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_linux_web_app.web-app-service.id]
  severity            = 1
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "Microsoft.Web/sites"
    metric_name      = "Http5xx"
    aggregation      = "Total"
    operator         = "GreaterThan"
    threshold        = 10
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "app_service_plan_cpu" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-asp-high-cpu"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_service_plan.web-app-service-plan.id]
  severity            = 2
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "Microsoft.Web/serverfarms"
    metric_name      = "CpuPercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 80
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "app_service_plan_memory" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-asp-high-memory"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_service_plan.web-app-service-plan.id]
  severity            = 2
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "Microsoft.Web/serverfarms"
    metric_name      = "MemoryPercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 80
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "waf_blocked_requests" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-waf-blocked-requests"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_cdn_frontdoor_profile.frontdoor-web-profile.id]
  severity            = 3
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "Microsoft.Cdn/profiles"
    metric_name      = "WebApplicationFirewallRequestCount"
    aggregation      = "Total"
    operator         = "GreaterThan"
    threshold        = 50

    dimension {
      name     = "Action"
      operator = "Include"
      values   = ["Block"]
    }
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "redis_high_cpu" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-redis-high-cpu"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_managed_redis.redis.id]
  severity            = 2
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "Microsoft.Cache/redisEnterprise"
    metric_name      = "CPU"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 80
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "redis_high_memory" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-redis-high-memory"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_managed_redis.redis.id]
  severity            = 2
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "Microsoft.Cache/redisEnterprise"
    metric_name      = "usedmemorypercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 80
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_metric_alert" "redis_high_connections" {
  count               = var.enable_alerts ? 1 : 0
  name                = "${local.service_prefix}-redis-high-connections"
  resource_group_name = azurerm_resource_group.web-rg.name
  scopes              = [azurerm_managed_redis.redis.id]
  severity            = 3
  frequency           = "PT1M"
  window_size         = "PT5M"

  criteria {
    metric_namespace = "Microsoft.Cache/redisEnterprise"
    metric_name      = "connectedclients"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 1000
  }

  action {
    action_group_id = azurerm_monitor_action_group.email_action_group[0].id
  }

  tags = local.common_tags
}

resource "azurerm_monitor_scheduled_query_rules_alert_v2" "law_daily_cap_alert" {
  count                = var.enable_alerts ? 1 : 0
  name                 = "${local.service_prefix}-law-daily-cap-alert"
  resource_group_name  = azurerm_resource_group.web-rg.name
  location             = azurerm_resource_group.web-rg.location
  scopes               = [azurerm_log_analytics_workspace.log-analytics-workspace.id]
  severity             = 2
  evaluation_frequency = "PT15M"
  window_duration      = "P1D"

  criteria {
    query                   = <<-QUERY
      Usage
      | where IsBillable == true
      | summarize IngestedGB = sum(Quantity) / 1024
      | where IngestedGB > ${var.log_analytics_daily_quota_gb * 0.8}
    QUERY
    time_aggregation_method = "Count"
    threshold               = 0
    operator                = "GreaterThan"
  }

  action {
    action_groups = [azurerm_monitor_action_group.email_action_group[0].id]
  }

  tags = local.common_tags
}
