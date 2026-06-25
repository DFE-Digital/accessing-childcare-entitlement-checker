resource "azurerm_service_plan" "web-app-service-plan" {
  location               = var.location
  name                   = "${local.service_prefix}-web-app-service-plan"
  resource_group_name    = azurerm_resource_group.web-rg.name
  os_type                = "Linux"
  sku_name               = var.webapp_sku
  worker_count           = var.webapp_instance_count
  zone_balancing_enabled = var.webapp_zone_balancing
  tags                   = local.common_tags
}

resource "azurerm_linux_web_app" "web-app-service" {
  #checkov:skip=CKV_AZURE_13: Public website intentionally allows anonymous access
  #checkov:skip=CKV_AZURE_17: Public web application does not require mutual TLS
  #checkov:skip=CKV_AZURE_88: App Service does not require Azure Files content storage
  #checkov:skip=CKV_AZURE_63: HTTP request telemetry is collected via Application Insights and Log Analytics
  #checkov:skip=CKV_AZURE_66: Application Insights provides request tracing and diagnostics
  #checkov:skip=CKV_AZURE_65: Application Insights provides exception tracking and diagnostics
  #checkov:skip=CKV_AZURE_222: Public network access is required, restricted by Front Door IP restrictions
  service_plan_id               = azurerm_service_plan.web-app-service-plan.id
  location                      = var.location
  name                          = "${local.service_prefix}-web-app-service"
  resource_group_name           = azurerm_resource_group.web-rg.name
  https_only                    = true
  virtual_network_subnet_id     = azapi_resource.app_subnet.id
  app_settings                  = local.web_app_settings
  public_network_access_enabled = true
  client_affinity_enabled       = true
  tags                          = local.common_tags

  site_config {
    always_on                         = var.webapp_always_on
    ftps_state                        = "Disabled"
    ip_restriction_default_action     = "Deny"
    health_check_path                 = "/health"
    health_check_eviction_time_in_min = 5
    http2_enabled                     = true

    application_stack {
      dotnet_version = "10.0"
    }

    ip_restriction {
      name        = "Access from Front Door"
      service_tag = "AzureFrontDoor.Backend"
      headers {
        x_azure_fdid = [azurerm_cdn_frontdoor_profile.frontdoor-web-profile.resource_guid]
      }
    }
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.app_identity.id]
  }

  lifecycle {
    ignore_changes = [
      app_settings["WEBSITE_RUN_FROM_PACKAGE"]
    ]
  }
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
  count = var.webapp_enable_staging_slot ? 1 : 0

  lifecycle {
    precondition {
      condition     = contains(local.slot_supported_skus, upper(var.webapp_sku))
      error_message = "Deployment slots require Standard or higher App Service plans."
    }
    ignore_changes = [
      app_settings["WEBSITE_RUN_FROM_PACKAGE"]
    ]
  }

  name                          = "staging"
  app_service_id                = azurerm_linux_web_app.web-app-service.id
  https_only                    = true
  virtual_network_subnet_id     = azapi_resource.app_subnet.id
  app_settings                  = local.web_app_settings
  public_network_access_enabled = true
  client_affinity_enabled       = true
  tags                          = local.common_tags

  site_config {
    always_on                         = var.webapp_always_on
    ftps_state                        = "Disabled"
    ip_restriction_default_action     = "Deny"
    health_check_path                 = "/health"
    health_check_eviction_time_in_min = 5
    http2_enabled                     = true

    application_stack {
      dotnet_version = "10.0"
    }

    ip_restriction {
      name        = "Access from Front Door"
      service_tag = "AzureFrontDoor.Backend"
      headers {
        x_azure_fdid = [azurerm_cdn_frontdoor_profile.frontdoor-web-profile.resource_guid]
      }
    }
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.app_identity.id]
  }
}


