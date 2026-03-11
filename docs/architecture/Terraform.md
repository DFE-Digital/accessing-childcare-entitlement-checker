<!-- BEGIN_TF_DOCS -->
## Requirements

| Name | Version |
|------|---------|
| <a name="requirement_azurerm"></a> [azurerm](#requirement\_azurerm) | ~> 4.52 |

## Providers

| Name | Version |
|------|---------|
| <a name="provider_azurerm"></a> [azurerm](#provider\_azurerm) | 4.58.0 |

## Modules

No modules.

## Resources

| Name | Type |
|------|------|
| [azurerm_application_insights.application-insights](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/application_insights) | resource |
| [azurerm_cdn_frontdoor_custom_domain.fd-custom-domain](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_custom_domain) | resource |
| [azurerm_cdn_frontdoor_custom_domain_association.web-app-custom-domain](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_custom_domain_association) | resource |
| [azurerm_cdn_frontdoor_endpoint.frontdoor-web-endpoint](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_endpoint) | resource |
| [azurerm_cdn_frontdoor_firewall_policy.web_firewall_policy](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_firewall_policy) | resource |
| [azurerm_cdn_frontdoor_origin.frontdoor-web-origin](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_origin) | resource |
| [azurerm_cdn_frontdoor_origin_group.frontdoor-origin-group](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_origin_group) | resource |
| [azurerm_cdn_frontdoor_profile.frontdoor-web-profile](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_profile) | resource |
| [azurerm_cdn_frontdoor_route.frontdoor-web-route](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_route) | resource |
| [azurerm_cdn_frontdoor_rule.security_headers_rule](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_rule) | resource |
| [azurerm_cdn_frontdoor_rule.security_txt_rule](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_rule) | resource |
| [azurerm_cdn_frontdoor_rule.thanks_txt_rule](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_rule) | resource |
| [azurerm_cdn_frontdoor_rule_set.security_headers](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_rule_set) | resource |
| [azurerm_cdn_frontdoor_rule_set.security_redirects](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_rule_set) | resource |
| [azurerm_cdn_frontdoor_security_policy.frontdoor-web-security-policy](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cdn_frontdoor_security_policy) | resource |
| [azurerm_cognitive_account.ai-translator](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cognitive_account) | resource |
| [azurerm_key_vault.key-vault](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault) | resource |
| [azurerm_key_vault_access_policy.github-kv-access](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_access_policy) | resource |
| [azurerm_key_vault_access_policy.web-app-kv-access](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_access_policy) | resource |
| [azurerm_key_vault_access_policy.web-app-staging-kv-access](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_access_policy) | resource |
| [azurerm_key_vault_secret.application-insights-connection-string](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_key_vault_secret.azure-translation-access-key](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_key_vault_secret.contentful-delivery-api-key](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_key_vault_secret.contentful-management-api-key](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_key_vault_secret.contentful-preview-api-key](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_key_vault_secret.contentful-space-id](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_key_vault_secret.pdf-generation-api-key](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_key_vault_secret.redis-enterprise-connection-string](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_linux_web_app.web-app-service](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/linux_web_app) | resource |
| [azurerm_linux_web_app_slot.web-app-service-staging](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/linux_web_app_slot) | resource |
| [azurerm_log_analytics_workspace.log-analytics-workspace](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/log_analytics_workspace) | resource |
| [azurerm_managed_redis.redis-enterprise](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/managed_redis) | resource |
| [azurerm_monitor_action_group.service-support-action](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_action_group) | resource |
| [azurerm_monitor_diagnostic_setting.frontdoor_logging](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_diagnostic_setting) | resource |
| [azurerm_monitor_diagnostic_setting.webapp_logs](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_diagnostic_setting) | resource |
| [azurerm_monitor_metric_alert.availability-alert](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_metric_alert) | resource |
| [azurerm_monitor_metric_alert.cpu_alert](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_metric_alert) | resource |
| [azurerm_monitor_metric_alert.memory_alert](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_metric_alert) | resource |
| [azurerm_monitor_metric_alert.web_app_error_alert](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_metric_alert) | resource |
| [azurerm_monitor_smart_detector_alert_rule.dependency-performance-degradation-detector](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_smart_detector_alert_rule) | resource |
| [azurerm_monitor_smart_detector_alert_rule.exception-volume-changed-detector](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_smart_detector_alert_rule) | resource |
| [azurerm_monitor_smart_detector_alert_rule.failure-anomalies-detector](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_smart_detector_alert_rule) | resource |
| [azurerm_monitor_smart_detector_alert_rule.memory-leak-detector](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_smart_detector_alert_rule) | resource |
| [azurerm_monitor_smart_detector_alert_rule.request-performance-degradation-detector](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_smart_detector_alert_rule) | resource |
| [azurerm_monitor_smart_detector_alert_rule.trace-severity-detector](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_smart_detector_alert_rule) | resource |
| [azurerm_resource_group.core-rg](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/resource_group) | resource |
| [azurerm_resource_group.redis-rg](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/resource_group) | resource |
| [azurerm_resource_group.translator-rg](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/resource_group) | resource |
| [azurerm_resource_group.web-rg](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/resource_group) | resource |
| [azurerm_service_plan.web-app-service-plan](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/service_plan) | resource |
| [azurerm_client_config.client](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/data-sources/client_config) | data source |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_alerting"></a> [alerting](#input\_alerting) | Alerting configuration per environment | <pre>map(object({<br>    name                 = string<br>    alerts_enabled       = bool<br>    email_alerts_enabled = bool<br>    smart_alerts_enabled = bool<br>    thresholds = object({<br>      availability = number<br>      cpu          = number<br>      memory       = number<br>      error        = number<br>    })<br>  }))</pre> | <pre>{<br>  "d01": {<br>    "alerts_enabled": false,<br>    "email_alerts_enabled": false,<br>    "name": "Test",<br>    "smart_alerts_enabled": false,<br>    "thresholds": {<br>      "availability": 90,<br>      "cpu": 95,<br>      "error": 5,<br>      "memory": 95<br>    }<br>  },<br>  "p01": {<br>    "alerts_enabled": true,<br>    "email_alerts_enabled": true,<br>    "name": "Production",<br>    "smart_alerts_enabled": true,<br>    "thresholds": {<br>      "availability": 99.9,<br>      "cpu": 85,<br>      "error": 1,<br>      "memory": 85<br>    }<br>  },<br>  "t01": {<br>    "alerts_enabled": true,<br>    "email_alerts_enabled": false,<br>    "name": "Staging",<br>    "smart_alerts_enabled": true,<br>    "thresholds": {<br>      "availability": 99.9,<br>      "cpu": 85,<br>      "error": 1,<br>      "memory": 85<br>    }<br>  }<br>}</pre> | no |
| <a name="input_aspnetcore_environment"></a> [aspnetcore\_environment](#input\_aspnetcore\_environment) | ASP.NET Core environment | `string` | n/a | yes |
| <a name="input_azure_frontdoor_scale"></a> [azure\_frontdoor\_scale](#input\_azure\_frontdoor\_scale) | Azure Front Door Scale | `string` | `"Standard_AzureFrontDoor"` | no |
| <a name="input_caching_type"></a> [caching\_type](#input\_caching\_type) | Caching type | `string` | n/a | yes |
| <a name="input_contentful_delivery_api_key"></a> [contentful\_delivery\_api\_key](#input\_contentful\_delivery\_api\_key) | Contentful Delivery API Key | `string` | n/a | yes |
| <a name="input_contentful_environment"></a> [contentful\_environment](#input\_contentful\_environment) | Contentful Environment | `string` | n/a | yes |
| <a name="input_contentful_management_api_key"></a> [contentful\_management\_api\_key](#input\_contentful\_management\_api\_key) | Contentful Management API Key | `string` | n/a | yes |
| <a name="input_contentful_preview_api_key"></a> [contentful\_preview\_api\_key](#input\_contentful\_preview\_api\_key) | Contentful Preview API Key | `string` | n/a | yes |
| <a name="input_contentful_space_id"></a> [contentful\_space\_id](#input\_contentful\_space\_id) | Contentful Space ID | `string` | n/a | yes |
| <a name="input_contentful_use_preview_api"></a> [contentful\_use\_preview\_api](#input\_contentful\_use\_preview\_api) | Use Contentful Preview API? | `bool` | n/a | yes |
| <a name="input_custom_domain"></a> [custom\_domain](#input\_custom\_domain) | Custom front-door domain | `string` | `""` | no |
| <a name="input_elz_environment"></a> [elz\_environment](#input\_elz\_environment) | The ELZ environment to match subscription (e.g. Dev) | `string` | n/a | yes |
| <a name="input_environment_prefix"></a> [environment\_prefix](#input\_environment\_prefix) | Environment prefix (e.g. d01) | `string` | n/a | yes |
| <a name="input_gtaa_base_url"></a> [gtaa\_base\_url](#input\_gtaa\_base\_url) | The base url for the 'Get-To-An-Answer' questionnaire service | `string` | n/a | yes |
| <a name="input_pdf_generation_api_key"></a> [pdf\_generation\_api\_key](#input\_pdf\_generation\_api\_key) | PDF Generation API Key | `string` | n/a | yes |
| <a name="input_pdf_generation_use_sandbox"></a> [pdf\_generation\_use\_sandbox](#input\_pdf\_generation\_use\_sandbox) | Generate PDFs in Sandbox Mode? | `bool` | n/a | yes |
| <a name="input_rebrand"></a> [rebrand](#input\_rebrand) | Force DfE Rebrand before 25th June 2025 | `bool` | `false` | no |
| <a name="input_scripts_clarity"></a> [scripts\_clarity](#input\_scripts\_clarity) | Clarity code | `string` | n/a | yes |
| <a name="input_support_alert_email"></a> [support\_alert\_email](#input\_support\_alert\_email) | Where to send alert emails to | `string` | n/a | yes |

## Outputs

No outputs.
<!-- END_TF_DOCS -->