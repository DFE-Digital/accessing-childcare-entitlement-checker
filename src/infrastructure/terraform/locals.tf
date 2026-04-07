locals {
  common_tags = {
    "Environment"      = var.elz_environment
    "Product"          = "Improve user journeys in early education and childcare"
    "Service Offering" = "Improve user journeys in early education and childcare"
  }
  prefix         = "s279${var.environment_prefix}"
  service_prefix = "s279${var.environment_prefix}-uks-cec"
  location       = "uksouth" # UK South is the only region that supports all the services we need, so this is fixed for now
}