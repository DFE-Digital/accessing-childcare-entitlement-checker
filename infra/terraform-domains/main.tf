resource "azurerm_resource_group" "domains-rg" {
  name     = var.service_domain_resource_group_name
  location = "uksouth"
  tags     = local.common_tags
}

# Shared DfE module for the zone and its default CAA, SPF and DMARC records.
# The production Terraform root owns the Front Door records in this zone.
module "dns" {
  source = "git::https://github.com/DFE-Digital/terraform-modules.git//dns/zones?ref=v0.86.0"

  hosted_zone = {
    (var.service_domain_zone_name) = {
      resource_group_name = azurerm_resource_group.domains-rg.name
      caa_record_list     = var.caa_issuers
      txt_records = {
        "@" = {
          value = "v=spf1 -all"
        }
        "_dmarc" = {
          value = "v=DMARC1; p=reject; sp=reject; rua=mailto:dmarc-rua@dmarc.service.gov.uk; ruf=mailto:dmarc-ruf@dmarc.service.gov.uk"
        }
      }
    }
  }
  tags = local.common_tags
}

# The module has no outputs, so read the zone back for its name servers.
data "azurerm_dns_zone" "service-zone" {
  name                = var.service_domain_zone_name
  resource_group_name = azurerm_resource_group.domains-rg.name

  depends_on = [module.dns]
}
