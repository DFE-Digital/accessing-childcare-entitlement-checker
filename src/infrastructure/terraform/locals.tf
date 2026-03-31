locals {
  common_tags = {
    "Environment"      = var.elz_environment
    "Product"          = "Childcare Entitlement Checker"
    "Service Offering" = "Eligibility Service"
  }
  prefix         = "s279${var.environment_prefix}"
  service_prefix = "s279${var.environment_prefix}-uks-cec"
  location       = "uksouth" # UK South is the only region that supports all the services we need, so this is fixed for now
}