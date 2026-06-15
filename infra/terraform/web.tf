resource "azurerm_service_plan" "web-app-service-plan" {
  location               = local.location
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
  service_plan_id               = azurerm_service_plan.web-app-service-plan.id
  location                      = local.location
  name                          = "${local.service_prefix}-web-app-service"
  resource_group_name           = azurerm_resource_group.web-rg.name
  https_only                    = true
  virtual_network_subnet_id     = azapi_resource.app_subnet.id
  app_settings                  = local.web_app_settings
  public_network_access_enabled = var.webapp_enable_public_access
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
  public_network_access_enabled = var.webapp_enable_public_access
  client_affinity_enabled       = true
  tags                          = local.common_tags

  site_config {
    always_on                     = var.webapp_always_on
    ip_restriction_default_action = "Deny"

    application_stack {
      dotnet_version = "10.0"
    }

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
}

resource "azurerm_private_dns_zone" "web_dns_zone" {
  name                = "privatelink.azurewebsites.net"
  resource_group_name = azurerm_resource_group.web-rg.name
  tags                = local.common_tags
}

resource "azurerm_private_dns_zone_virtual_network_link" "web_dns_link" {
  name                  = "${local.service_prefix}-web-dns-link"
  resource_group_name   = azurerm_resource_group.web-rg.name
  private_dns_zone_name = azurerm_private_dns_zone.web_dns_zone.name
  virtual_network_id    = azurerm_virtual_network.vnet.id
  tags                  = local.common_tags
}

resource "azurerm_private_endpoint" "web_pe" {
  name                = "${local.service_prefix}-web-pe"
  location            = local.location
  resource_group_name = azurerm_resource_group.web-rg.name
  subnet_id           = azapi_resource.pe_subnet.id
  tags                = local.common_tags

  private_service_connection {
    name                           = "${local.service_prefix}-web-psc"
    private_connection_resource_id = azurerm_linux_web_app.web-app-service.id
    is_manual_connection           = false
    subresource_names              = ["sites"]
  }

  private_dns_zone_group {
    name                 = "web-dns-zone-group"
    private_dns_zone_ids = [azurerm_private_dns_zone.web_dns_zone.id]
  }

  depends_on = [
    azurerm_private_dns_zone_virtual_network_link.web_dns_link
  ]
}

resource "azurerm_private_endpoint" "staging_pe" {
  count               = var.webapp_enable_staging_slot ? 1 : 0
  name                = "${local.service_prefix}-staging-pe"
  location            = local.location
  resource_group_name = azurerm_resource_group.web-rg.name
  subnet_id           = azapi_resource.pe_subnet.id
  tags                = local.common_tags

  private_service_connection {
    name                           = "${local.service_prefix}-staging-psc"
    private_connection_resource_id = azurerm_linux_web_app.web-app-service.id
    is_manual_connection           = false
    subresource_names              = ["sites-staging"]
  }

  private_dns_zone_group {
    name                 = "web-dns-zone-group"
    private_dns_zone_ids = [azurerm_private_dns_zone.web_dns_zone.id]
  }

  depends_on = [
    azurerm_private_dns_zone_virtual_network_link.web_dns_link
  ]
}
