locals {
  common_tags = {
    "Environment"      = var.elz_environment
    "Product"          = "Childcare Entitlement Checker"
    "Service Offering" = "Eligibility Service"
  }
  prefix         = "s279${var.environment_prefix}"
  service_prefix = "s279${var.environment_prefix}-uks-cl"
  location       = "uksouth"
}