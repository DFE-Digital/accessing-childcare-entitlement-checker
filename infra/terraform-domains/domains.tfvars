# `service_domain_zone_name` and `service_domain_resource_group_name`
# must be given the same values as in infra/terraform/environments/production.tfvars.
service_domain_zone_name           = "check-if-you-are-eligible-for-childcare-funding.service.gov.uk"
service_domain_resource_group_name = "s279p01rg-uks-cec-domains"
caa_issuers                        = ["globalsign.com", "digicert.com"]
