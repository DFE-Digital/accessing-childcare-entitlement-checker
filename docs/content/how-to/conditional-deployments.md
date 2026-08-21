---
title: Conditional deployments guide
layout: sub-navigation
sectionKey: How-to guides
order: 2
includeInBreadcrumbs: true
eleventyNavigation:
  parent: How-to guides
  key: Conditional deployments
---
Follow this guide to configure and scale your environments (Development, Staging, and Production) using feature flags and configuration variables in the project's Terraform configuration.

## Configure your conditional deployments

Optimize your environment configuration to reduce costs in lower environments while enforcing production-grade security, scalability, and zero-downtime deployment capabilities in staging and production.

Manage the four primary conditional aspects in our Terraform configurations:

1. [Configure Azure Front Door Custom Domains](#configure-azure-front-door-custom-domains)
2. [Configure Azure Front Door Premium Features](#configure-azure-front-door-premium-features)
3. [Configure Staging Deployment Slots](#configure-staging-deployment-slots)
4. [Configure Non-Production Basic Authentication](#configure-non-production-basic-authentication)


## Configure Azure Front Door custom domains

* Variables: `custom_domain` (string, default: `""`)
* Trigger: Active when `custom_domain` is set to any non-empty string.
* Relevant Files: `infra/terraform/frontdoor.tf`

### Route custom domain traffic
Specify a custom domain to execute these routing and security tasks:
* **Provision Custom Domain**: Terraform provisions an `azurerm_cdn_frontdoor_custom_domain` resource named `${local.service_prefix}-fd-custom-domain` for custom DNS routing.
* **Manage Certificates**: Configure managed TLS certificates (`certificate_type = "ManagedCertificate"`) automatically through Azure Front Door.
* **Associate Domain**: Configure an `azurerm_cdn_frontdoor_custom_domain_association` to bind the custom domain to the primary Front Door routing rules.
* **Route Security Policy**: Dynamically include a `domain` association block inside the WAF policy (`azurerm_cdn_frontdoor_security_policy.frontdoor-web-security-policy`) to protect traffic hitting the custom domain with web application firewall rules.

Leave `custom_domain` blank to skip provisioning custom domain resources and access the application only via the default Front Door endpoint (`*.azurefd.net`).

## Configure Azure Front Door premium features

* Variables: `azure_frontdoor_sku` (string, default: `"Standard"`), `fd_use_private_link` (boolean, default: `false`), `waf_enable_managed_rules` (boolean, default: `false`)
* Trigger: Private Link is active when `fd_use_private_link` is set to `true`. Managed WAF rules are active when `waf_enable_managed_rules` is set to `true`. Both features require `azure_frontdoor_sku` to be set to `"Premium"`.
* Relevant Files: `infra/terraform/frontdoor.tf`, `infra/terraform/frontdoor_waf.tf`, `infra/terraform/variables.tf`

### Apply premium settings
Specify the Premium SKU using `azure_frontdoor_sku = "Premium"` and enable or disable individual enterprise-grade features with separate feature flags using dynamic blocks:

* **Integrate Private Link**: Apply a dynamic `private_link` block inside `azurerm_cdn_frontdoor_origin.frontdoor-web-origin` when `fd_use_private_link` is `true`. This configures Front Door to route traffic to the backend App Service using Azure's private backbone network via Private Link, rather than routing over the public internet.
* **Set Managed WAF Rules**: Apply two comprehensive managed rulesets in `Prevention` mode within the Web Application Firewall (`azurerm_cdn_frontdoor_firewall_policy.web_firewall_policy`) when `waf_enable_managed_rules` is `true`:
  * Default Rule Set (`Microsoft_DefaultRuleSet` version `2.1`): Protects against common web vulnerabilities (OWASP Top 10).
  * Bot Manager Rule Set (`Microsoft_BotManagerRuleSet` version `1.1`): Detects and mitigates malicious bot traffic.

Omit these rulesets and networking capabilities in Standard deployments (where `azure_frontdoor_sku` is `"Standard"`) to avoid Azure deployment failures.

## Configure staging deployment slots

* Variables: `webapp_enable_staging_slot` (boolean, default: `false`), `webapp_sku` (string)
* Trigger: Active when `webapp_enable_staging_slot` is set to `true`.
* Relevant Files: `infra/terraform/web.tf`, `infra/terraform/variables.tf`

### Provision staging slots
Provision a secondary staging slot to support zero-downtime releases and pre-production checks:

* **Provision Staging Slot**: Create an `azurerm_linux_web_app_slot` named `staging` attached to the main App Service when `webapp_enable_staging_slot` is `true`.
* **Set Dedicated Private Endpoints**: Provision a dedicated `azurerm_private_endpoint` named `${local.service_prefix}-staging-pe` pointing to the staging slot target (`sites-staging`) to ensure secure backchannel verification from within the virtual network.
* **Verify SKU Constraints**: Prevent runtime deployment failures by ensuring the staging slot resource includes a Terraform `precondition` block:
  ```hcl
  lifecycle {
    precondition {
      condition     = contains(local.slot_supported_skus, upper(var.webapp_sku))
      error_message = "Deployment slots require Standard or higher App Service plans."
    }
  }
  ```
  This guarantees that the App Service plan SKU is compatible with slot allocation (e.g., Standard or Premium levels like `P0V3`, `P1V3`). Deployments using cheaper SKUs (such as `B1` or shared tiers) will fail gracefully during the Terraform plan phase.

## Configure non-production basic authentication

* Variables: `aspnetcore_environment` (string), `development_basic_auth_password` (sensitive string, default: `""`)
* Trigger: Active when `aspnetcore_environment != "Production"`.
* Relevant Files: `infra/terraform/locals.tf`, `infra/terraform/web.tf`

### Enforce basic authentication
Inject a basic HTTP authentication mechanism to prevent unauthorised public discovery and access during development and testing:

* **Inject App Settings**: Dynamically merge a configuration key in `locals.tf` if the target ASP.NET Core environment is not "Production":
  ```hcl
  web_app_settings = merge({
    "ASPNETCORE_ENVIRONMENT"                = var.aspnetcore_environment
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = azurerm_application_insights.application-insights.connection_string
    ...
    }, var.aspnetcore_environment != "Production" ? {
    "DevelopmentBasicAuthPassword" = var.development_basic_auth_password
  } : {})
  ```
* **Enforce Application Protection**: Intercept incoming requests in non-production environments when `DevelopmentBasicAuthPassword` is injected, demanding credentials matching the specified password.
