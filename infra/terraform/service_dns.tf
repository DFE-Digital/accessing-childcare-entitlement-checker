# Apex records for the service domain. The zone comes from infra/terraform-domains.
# Follows the apex pattern in DFE-Digital/terraform-modules domains/environment_domains.
locals {
  manage_service_dns = var.service_custom_domain != "" && var.service_domain_resource_group_name != ""
}

data "azurerm_dns_zone" "service-zone" {
  count               = local.manage_service_dns ? 1 : 0
  name                = var.service_custom_domain
  resource_group_name = var.service_domain_resource_group_name
}

resource "azurerm_dns_a_record" "service-apex" {
  count               = local.manage_service_dns ? 1 : 0
  name                = "@"
  zone_name           = data.azurerm_dns_zone.service-zone[0].name
  resource_group_name = var.service_domain_resource_group_name
  ttl                 = 300
  target_resource_id  = azurerm_cdn_frontdoor_endpoint.frontdoor-web-endpoint.id
  tags                = local.common_tags

  depends_on = [azurerm_cdn_frontdoor_route.frontdoor-web-route]
}

resource "azurerm_dns_txt_record" "service-apex-validation" {
  count               = local.manage_service_dns ? 1 : 0
  name                = "_dnsauth"
  zone_name           = data.azurerm_dns_zone.service-zone[0].name
  resource_group_name = var.service_domain_resource_group_name
  ttl                 = 300
  tags                = local.common_tags

  record {
    value = azurerm_cdn_frontdoor_custom_domain.fd-service-custom-domain[0].validation_token
  }
}
