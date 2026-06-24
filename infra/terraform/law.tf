resource "azurerm_log_analytics_workspace" "log-analytics-workspace" {
  name                = "${local.service_prefix}-log-analytics-workspace"
  location            = azurerm_resource_group.web-rg.location
  resource_group_name = azurerm_resource_group.web-rg.name
  retention_in_days   = 30
  daily_quota_gb      = 1
  tags                = local.common_tags
}

resource "azurerm_application_insights" "application-insights" {
  name                = "${local.service_prefix}-app-insights"
  location            = azurerm_resource_group.web-rg.location
  resource_group_name = azurerm_resource_group.web-rg.name
  application_type    = "web"
  workspace_id        = azurerm_log_analytics_workspace.log-analytics-workspace.id
  tags                = local.common_tags
}

resource "azurerm_application_insights_standard_web_test" "web-app-test" {
  count = var.enable_web_test ? 1 : 0

  name                    = "${local.service_prefix}-web-app-test"
  description             = "Web application availability test"
  resource_group_name     = azurerm_resource_group.web-rg.name
  location                = azurerm_resource_group.web-rg.location
  application_insights_id = azurerm_application_insights.application-insights.id
  frequency               = 600
  timeout                 = 60
  enabled                 = true
  retry_enabled           = true
  geo_locations           = ["emea-se-sto-edge", "emea-ru-msa-edge"]

  lifecycle {
    ignore_changes = [tags]
  }

  request {
    url = "https://${local.host_name}"
  }

  validation_rules {
    expected_status_code = 200
  }
}