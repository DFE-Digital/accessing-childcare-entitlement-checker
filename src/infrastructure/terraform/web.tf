locals {
  web_app_settings = {
    "ASPNETCORE_ENVIRONMENT"                = var.aspnetcore_environment
    # "ContentfulOptions__DeliveryApiKey"     = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.contentful-delivery-api-key.versionless_id})"
    # "ContentfulOptions__PreviewApiKey"      = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.contentful-preview-api-key.versionless_id})"
    # "ContentfulOptions__ManagementApiKey"   = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.contentful-management-api-key.versionless_id})"
    # "ContentfulOptions__SpaceId"            = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.contentful-space-id.versionless_id})"
    # "ContentfulOptions__Environment"        = var.contentful_environment
    # "ContentfulOptions__UsePreviewApi"      = var.contentful_use_preview_api
    "ApplicationInsights__ConnectionString" = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.application-insights-connection-string.versionless_id})"
    # "Caching__Type"                         = var.caching_type
    # "Caching__ConnectionString"             = lower(var.caching_type) == "redis" ? "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.redis-enterprise-connection-string[0].versionless_id})" : ""
    # "Scripts__Clarity"                      = var.scripts_clarity
    #"AzureTranslation__AccessKey"           = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.azure-translation-access-key.versionless_id})"
   # "PdfGeneration__ApiKey"                 = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.pdf-generation-api-key.versionless_id})"
    # "PdfGeneration__Sandbox"                = var.pdf_generation_use_sandbox
    "Rebrand"                               = var.rebrand
    "GetToAnAnswer__BaseUrl"                = var.gtaa_base_url
  }
}

resource "azurerm_resource_group" "web-rg" {
  name     = "${local.prefix}rg-uks-cl-web"
  location = local.location
  tags     = local.common_tags
}

resource "azurerm_service_plan" "web-app-service-plan" {
  location            = local.location
  name                = "${local.service_prefix}-web-app-service-plan"
  resource_group_name = azurerm_resource_group.web-rg.name
  os_type             = "Linux"
  sku_name            = "P0v3"

  tags = local.common_tags
}

resource "azurerm_linux_web_app_slot" "web-app-service-staging" {
  app_service_id = azurerm_linux_web_app.web-app-service.id
  name           = "staging"
  https_only     = true

  site_config {
    always_on = true

    ip_restriction_default_action = "Deny"

    ip_restriction {
      name        = "Access from Front Door"
      service_tag = "AzureFrontDoor.Backend"
    }

    health_check_path                 = "/health"
    health_check_eviction_time_in_min = 5

    minimum_tls_version     = "1.3"
    scm_minimum_tls_version = "1.3"
  }

  identity {
    type = "SystemAssigned"
  }

  app_settings = local.web_app_settings

  tags = local.common_tags
}

resource "azurerm_linux_web_app" "web-app-service" {
  service_plan_id     = azurerm_service_plan.web-app-service-plan.id
  location            = local.location
  name                = "${local.service_prefix}-web-app-service"
  resource_group_name = azurerm_resource_group.web-rg.name
  https_only          = true

  site_config {
    always_on = true

    ip_restriction_default_action = "Deny"

    ip_restriction {
      name        = "Access from Front Door"
      service_tag = "AzureFrontDoor.Backend"
    }

    health_check_path                 = "/health"
    health_check_eviction_time_in_min = 5
  }

  identity {
    type = "SystemAssigned"
  }

  app_settings = local.web_app_settings

  tags = local.common_tags
}

resource "azurerm_monitor_diagnostic_setting" "webapp_logs" {
  name                       = "${var.environment_prefix}-web-app-diagnostics"
  target_resource_id         = azurerm_linux_web_app.web-app-service.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  # Capture common runtime and diagnostic logs
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

  # All Metrics
  enabled_metric {
    category = "AllMetrics"
  }
}