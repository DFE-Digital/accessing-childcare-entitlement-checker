locals {
  project_id          = "s279"
  project_short_code  = "cec"
  service_offering    = "Improve user journeys in early education and childcare"
  product             = "Improve user journeys in early education and childcare"
  source              = "terraform"
  location            = "uksouth"
  location_short_code = "uks"

  prefix         = "${local.project_id}${var.environment_prefix}"
  service_prefix = "${local.prefix}-${local.location_short_code}-${local.project_short_code}"

  common_tags = {
    "Environment"      = var.elz_environment
    "Service Offering" = local.service_offering
    "Product"          = local.product
    "Source"           = local.source
  }

  web_app_settings = merge({
    "ASPNETCORE_ENVIRONMENT" = var.aspnetcore_environment
    }, var.aspnetcore_environment != "Production" ? {
    "DevelopmentBasicAuthPassword" = var.development_basic_auth_password
  } : {})

  slot_supported_skus = ["P0V3", "P1V3"]
}