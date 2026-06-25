---
title: Deployed infrastructure
layout: sub-navigation
sectionKey: Architecture
order: 5
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Architecture
  key: Deployed infrastructure
---
This page is generated automatically from the Terraform configuration.

<!-- BEGIN_TF_DOCS -->
## Requirements

| Name | Version |
|------|---------|
| <a name="requirement_azapi"></a> [azapi](#requirement\_azapi) | 2.10.0 |
| <a name="requirement_azurerm"></a> [azurerm](#requirement\_azurerm) | ~> 4.52 |

## Providers

| Name | Version |
|------|---------|
| <a name="provider_azapi"></a> [azapi](#provider\_azapi) | 2.10.0 |
| <a name="provider_azurerm"></a> [azurerm](#provider\_azurerm) | ~> 4.52 |

## Modules

No modules.

## Resources

| Name | Type |
|------|------|
| [azapi_resource.app_subnet](https://registry.terraform.io/providers/Azure/azapi/2.10.0/docs/resources/resource) | resource |
| [azapi_resource.pe_subnet](https://registry.terraform.io/providers/Azure/azapi/2.10.0/docs/resources/resource) | resource |
| [azurerm_application_insights.application-insights](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/application_insights) | resource |
| [azurerm_cdn_frontdoor_custom_domain.fd-custom-domain](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_custom_domain) | resource |
| [azurerm_cdn_frontdoor_custom_domain_association.web-app-custom-domain](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_custom_domain_association) | resource |
| [azurerm_cdn_frontdoor_endpoint.frontdoor-web-endpoint](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_endpoint) | resource |
| [azurerm_cdn_frontdoor_firewall_policy.web_firewall_policy](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_firewall_policy) | resource |
| [azurerm_cdn_frontdoor_origin.frontdoor-web-origin](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_origin) | resource |
| [azurerm_cdn_frontdoor_origin_group.frontdoor-origin-group](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_origin_group) | resource |
| [azurerm_cdn_frontdoor_profile.frontdoor-web-profile](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_profile) | resource |
| [azurerm_cdn_frontdoor_route.frontdoor-web-route](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_route) | resource |
| [azurerm_cdn_frontdoor_rule.security_txt_rule](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_rule) | resource |
| [azurerm_cdn_frontdoor_rule.thanks_txt_rule](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_rule) | resource |
| [azurerm_cdn_frontdoor_rule_set.security_redirects](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_rule_set) | resource |
| [azurerm_cdn_frontdoor_security_policy.frontdoor-web-security-policy](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_security_policy) | resource |
| [azurerm_linux_web_app.web-app-service](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/linux_web_app) | resource |
| [azurerm_linux_web_app_slot.staging](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/linux_web_app_slot) | resource |
| [azurerm_log_analytics_workspace.log-analytics-workspace](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/log_analytics_workspace) | resource |
| [azurerm_monitor_diagnostic_setting.blob_logs](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_diagnostic_setting) | resource |
| [azurerm_monitor_diagnostic_setting.frontdoor_logging](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_diagnostic_setting) | resource |
| [azurerm_monitor_diagnostic_setting.webapp_logs](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_diagnostic_setting) | resource |
| [azurerm_network_security_group.app_nsg](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/network_security_group) | resource |
| [azurerm_network_security_group.pe_nsg](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/network_security_group) | resource |
| [azurerm_private_dns_zone.blob_dns_zone](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/private_dns_zone) | resource |
| [azurerm_private_dns_zone.web_dns_zone](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/private_dns_zone) | resource |
| [azurerm_private_dns_zone_virtual_network_link.blob_dns_link](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/private_dns_zone_virtual_network_link) | resource |
| [azurerm_private_dns_zone_virtual_network_link.web_dns_link](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/private_dns_zone_virtual_network_link) | resource |
| [azurerm_private_endpoint.deployment_pe](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/private_endpoint) | resource |
| [azurerm_private_endpoint.staging_pe](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/private_endpoint) | resource |
| [azurerm_private_endpoint.web_pe](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/private_endpoint) | resource |
| [azurerm_resource_group.web-rg](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/resource_group) | resource |
| [azurerm_service_plan.web-app-service-plan](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/service_plan) | resource |
| [azurerm_storage_account.deployment_storage](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/storage_account) | resource |
| [azurerm_storage_container.deployments](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/storage_container) | resource |
| [azurerm_user_assigned_identity.app_identity](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/user_assigned_identity) | resource |
| [azurerm_virtual_network.vnet](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/virtual_network) | resource |
| [azurerm_client_config.client](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/data-sources/client_config) | data source |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_aspnetcore_environment"></a> [aspnetcore\_environment](#input\_aspnetcore\_environment) | ASP.NET Core environment | `string` | n/a | yes |
| <a name="input_azure_frontdoor_sku"></a> [azure\_frontdoor\_sku](#input\_azure\_frontdoor\_sku) | Azure Front Door SKU | `string` | `"Standard"` | no |
| <a name="input_custom_domain"></a> [custom\_domain](#input\_custom\_domain) | Custom front-door domain | `string` | `""` | no |
| <a name="input_development_basic_auth_password"></a> [development\_basic\_auth\_password](#input\_development\_basic\_auth\_password) | Shared password for development-only basic auth | `string` | `""` | no |
| <a name="input_elz_environment"></a> [elz\_environment](#input\_elz\_environment) | The ELZ environment to match subscription (e.g. Dev) | `string` | n/a | yes |
| <a name="input_environment_prefix"></a> [environment\_prefix](#input\_environment\_prefix) | Environment prefix (e.g. d01) | `string` | n/a | yes |
| <a name="input_fd_use_private_link"></a> [fd\_use\_private\_link](#input\_fd\_use\_private\_link) | Should use private link to connect to app service | `bool` | `false` | no |
| <a name="input_waf_enable_managed_rules"></a> [waf\_enable\_managed\_rules](#input\_waf\_enable\_managed\_rules) | Enable managed rule sets in WAF | `bool` | `false` | no |
| <a name="input_webapp_always_on"></a> [webapp\_always\_on](#input\_webapp\_always\_on) | Set always for web app | `bool` | `false` | no |
| <a name="input_webapp_enable_public_access"></a> [webapp\_enable\_public\_access](#input\_webapp\_enable\_public\_access) | Enable public access for web app | `bool` | `true` | no |
| <a name="input_webapp_enable_staging_slot"></a> [webapp\_enable\_staging\_slot](#input\_webapp\_enable\_staging\_slot) | Enable staging slot for web app | `bool` | `false` | no |
| <a name="input_webapp_instance_count"></a> [webapp\_instance\_count](#input\_webapp\_instance\_count) | The number of instances for the web app | `number` | `1` | no |
| <a name="input_webapp_sku"></a> [webapp\_sku](#input\_webapp\_sku) | Web App SKU (e.g. B1) | `string` | `"B1"` | no |
| <a name="input_webapp_zone_balancing"></a> [webapp\_zone\_balancing](#input\_webapp\_zone\_balancing) | Enable zone balancing on web app | `bool` | `false` | no |

## Outputs

| Name | Description |
|------|-------------|
| <a name="output_frontdoor_hostname"></a> [frontdoor\_hostname](#output\_frontdoor\_hostname) | n/a |
| <a name="output_resource_group_name"></a> [resource\_group\_name](#output\_resource\_group\_name) | n/a |
| <a name="output_staging_slot_in_use"></a> [staging\_slot\_in\_use](#output\_staging\_slot\_in\_use) | Indicates whether the staging slot is enabled for the web app. |
| <a name="output_web_app_name"></a> [web\_app\_name](#output\_web\_app\_name) | n/a |
<!-- END_TF_DOCS -->