locals {
  service_offering = "Improve user journeys in early education and childcare"
  product          = "Improve user journeys in early education and childcare"
  source           = "terraform"

  common_tags = {
    "Environment"      = "Prod"
    "Service Offering" = local.service_offering
    "Product"          = local.product
    "Source"           = local.source
  }
}
