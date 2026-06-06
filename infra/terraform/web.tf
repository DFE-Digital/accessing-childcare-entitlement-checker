resource "azurerm_service_plan" "web-app-service-plan" {
  location            = local.location
  name                = "${local.service_prefix}-web-app-service-plan"
  resource_group_name = azurerm_resource_group.web-rg.name
  os_type             = "Linux"
  sku_name            = var.webapp_sku
  worker_count        = var.webapp_instance_count
  tags                = local.common_tags
}

resource "azurerm_linux_web_app" "web-app-service" {
  service_plan_id           = azurerm_service_plan.web-app-service-plan.id
  location                  = local.location
  name                      = "${local.service_prefix}-web-app-service"
  resource_group_name       = azurerm_resource_group.web-rg.name
  https_only                = true
  virtual_network_subnet_id = azurerm_subnet.app_subnet.id

  site_config {
    always_on = true

    application_stack {
      dotnet_version = "10.0"
    }

    ip_restriction_default_action = "Deny"

    ip_restriction {
      name        = "Access from Front Door"
      service_tag = "AzureFrontDoor.Backend"
    }

    health_check_path                 = "/health"
    health_check_eviction_time_in_min = 5
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.app_identity.id]
  }

  app_settings = local.web_app_settings

  tags = local.common_tags
}

resource "azurerm_monitor_diagnostic_setting" "webapp_logs" {
  name                       = "${var.environment_prefix}-web-app-diagnostics"
  target_resource_id         = azurerm_linux_web_app.web-app-service.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  enabled_log {
    category = "AppServiceConsoleLogs"
  }

  enabled_log {
    category = "AppServiceHTTPLogs"
  }

  enabled_log {
    category = "AppServicePlatformLogs"
  }

  enabled_log {
    category = "AppServiceAppLogs"
  }

  enabled_metric {
    category = "AllMetrics"
  }
}

resource "azurerm_linux_web_app_slot" "staging" {
  count = var.enable_staging_slot ? 1 : 0

  lifecycle {
    precondition {
      condition     = contains(local.slot_supported_skus, upper(var.webapp_sku))
      error_message = "Deployment slots require Standard or higher App Service plans."
    }
  }

  name                      = "staging"
  app_service_id            = azurerm_linux_web_app.web-app-service.id
  https_only                = true
  virtual_network_subnet_id = azurerm_subnet.app_subnet.id

  site_config {
    always_on = true

    application_stack {
      dotnet_version = "10.0"
    }

    ip_restriction_default_action = "Deny"

    ip_restriction {
      name        = "Access from Front Door"
      service_tag = "AzureFrontDoor.Backend"
    }

    health_check_path                 = "/health"
    health_check_eviction_time_in_min = 5
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.app_identity.id]
  }

  app_settings = local.web_app_settings

  tags = local.common_tags
}
