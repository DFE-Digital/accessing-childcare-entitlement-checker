---
title: Terraform variables
layout: sub-navigation
sectionKey: Reference
order: 10
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Architecture
  key: Terraform variables
---
## Terraform variables

This reference provides a complete list of the infrastructure-as-code configuration parameters used to provision and configure resources for the Accessing Childcare Entitlement Checker.

These parameters are defined in the Terraform configuration files and allow for customisation of the environment, SKUs, and scaling behaviours.

| Variable name | Type | Default value | Description |
| :--- | :--- | :--- | :--- |
| `elz_environment` | `string` | *No default* | The ELZ environment to match subscription (e.g. Dev) |
| `environment_prefix` | `string` | *No default* | Environment prefix (e.g. d01) |
| `aspnetcore_environment` | `string` | *No default* | ASP.NET Core environment |
| `development_basic_auth_password` | `string` | `""` | Shared password for development-only basic auth |
| `azure_frontdoor_sku` | `string` | `"Standard"` | Azure Front Door SKU |
| `custom_domain` | `string` | `""` | Custom front-door domain |
| `waf_enable_managed_rules` | `bool` | `false` | Enable managed rule sets in WAF |
| `webapp_sku` | `string` | `"B1"` | Web App SKU (e.g. B1) |
| `webapp_zone_balancing` | `bool` | `false` | Enable zone balancing on web app |
| `webapp_instance_count` | `number` | `1` | The number of instances for the web app |
| `webapp_enable_staging_slot` | `bool` | `false` | Enable staging slot for web app |
| `enable_web_test` | `bool` | `false` | Enable application insights web test |
| `location` | `string` | `"uksouth"` | The Azure region to deploy resources into |
| `location_short_code` | `string` | `"uks"` | The short code for the Azure region (e.g. uks) |
| `waf_mode` | `string` | `"Prevention"` | The mode the WAF should be deployed in (Prevention or Detection) |
| `alert_email_address` | `string` | `""` | The email address to send alert notifications to |
| `enable_alerts` | `bool` | `false` | Toggle to enable/disable Azure Monitor alerts |
| `redis_sku_name` | `string` | `"Balanced_B1"` | The SKU of the Managed Redis instance |
| `log_analytics_daily_quota_gb` | `number` | *No default* | The daily quota in GB for the Log Analytics workspace |
| `log_analytics_retention_in_days` | `number` | *No default* | The retention period in days for the Log Analytics workspace |
| `enable_load_testing` | `bool` | `false` | Enable Azure Load Testing |
| `budget_amount_web` | `number` | *No default* | The budget amount for the web resource group |
| `budget_amount_load_test` | `number` | *No default* | The budget amount for the load test resource group |
| `budget_alert_threshold_forecast` | `number` | `90` | The threshold percentage for forecasted budget alerts |
| `budget_alert_threshold_actual` | `number` | `100` | The threshold percentage for actual budget alerts |
| `application_insights_daily_data_cap_in_gb` | `number` | *No default* | The daily data cap in GB for Application Insights |
| `application_insights_sampling_percentage` | `number` | *No default* | The sampling percentage for Application Insights |
| `google_tag_manager_container_id` | `string` | `""` | Google Tag Manager container ID |
| `feature_flags` | `map(string)` | `{}` | A map of feature flags to enable/disable |
