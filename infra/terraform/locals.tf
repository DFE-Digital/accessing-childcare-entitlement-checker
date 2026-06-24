locals {
  project_id         = "s279"
  project_short_code = "cec"
  service_offering   = "Improve user journeys in early education and childcare"
  product            = "Improve user journeys in early education and childcare"
  source             = "terraform"

  prefix         = "${local.project_id}${var.environment_prefix}"
  service_prefix = "${local.prefix}-${var.location_short_code}-${local.project_short_code}"

  host_name = var.custom_domain == "" ? azurerm_cdn_frontdoor_endpoint.frontdoor-web-endpoint.host_name : var.custom_domain

  common_tags = {
    "Environment"      = var.elz_environment
    "Service Offering" = local.service_offering
    "Product"          = local.product
    "Source"           = local.source
  }

  web_app_settings = merge({
    "ASPNETCORE_ENVIRONMENT"                       = var.aspnetcore_environment
    "APPLICATIONINSIGHTS_CONNECTION_STRING"        = azurerm_application_insights.application-insights.connection_string
    "WEBSITE_SWAP_WARMUP_PING_PATH"                = "/health"
    "WEBSITE_SWAP_WARMUP_PING_STATUSES"            = "200"
    "WEBSITE_RUN_FROM_PACKAGE"                     = ""
    "WEBSITE_RUN_FROM_PACKAGE_BLOB_MI_RESOURCE_ID" = azurerm_user_assigned_identity.app_identity.id
    }, var.aspnetcore_environment != "Production" ? {
    "DevelopmentBasicAuthPassword" = var.development_basic_auth_password
  } : {})

  slot_supported_skus = ["P0V3", "P1V3"]
}