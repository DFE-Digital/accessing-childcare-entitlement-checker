---
title: Conditional Deployments Guide
layout: sub-navigation
sectionKey: Developers
eleventyNavigation:
  parent: Developers
  key: Conditional Deployments
order: 10
---

This guide details the conditional aspects of the project's Terraform configuration, outlining how various environments (Development, Staging, and Production) are customized and scaled through feature flags and configuration variables.

## Overview

The Terraform configuration is highly modular and supports conditional resource provisioning to optimize for cost in lower environments while providing production-grade security, scalability, and zero-downtime deployment capabilities in staging and production.

There are four primary conditional aspects in our Terraform configurations:

1. [Azure Front Door Custom Domains](#1-azure-front-door-custom-domains)
2. [Azure Front Door Premium Features](#2-azure-front-door-premium-features)
3. [Staging Deployment Slots](#3-staging-deployment-slots)
4. [Non-Production Basic Authentication](#4-non-production-basic-authentication)

---

## 1. Azure Front Door Custom Domains

* **Variables:** `custom_domain` (string, default: `""`)
* **Trigger:** Active when `custom_domain` is set to any non-empty string.
* **Relevant Files:** `infra/terraform/frontdoor.tf`

### Behavior
When a custom domain is specified:
* **Custom Domain Provisioning:** Terraform provisions an `azurerm_cdn_frontdoor_custom_domain` resource named `${local.service_prefix}-fd-custom-domain` for custom DNS routing.
* **Certificate Management:** Configures managed TLS certificates (`certificate_type = "ManagedCertificate"`) automatically through Azure Front Door.
* **Domain Association:** Configures an `azurerm_cdn_frontdoor_custom_domain_association` to bind the custom domain to the primary Front Door routing rules.
* **Security Policy Routing:** Dynamically includes a `domain` association block inside the WAF policy (`azurerm_cdn_frontdoor_security_policy.frontdoor-web-security-policy`) so that web application firewall rules protect traffic hitting the custom domain.

If `custom_domain` is left blank, the Front Door custom domain resources are completely skipped, and the application is accessed only via the default Front Door endpoint (`*.azurefd.net`).

---

## 2. Azure Front Door Premium Features

* **Variables:** `azure_frontdoor_scale` (string, default: `"Standard"`)
* **Trigger:** Active when `azure_frontdoor_scale` is set to `"Premium"`.
* **Relevant Files:** `infra/terraform/frontdoor.tf`, `infra/terraform/frontdoor_waf.tf`

### Behavior
When set to `"Premium"`, advanced enterprise-grade features are dynamically enabled via `for_each` blocks:

* **Private Link Integration:** Inside `azurerm_cdn_frontdoor_origin.frontdoor-web-origin`, a dynamic `private_link` block is applied. This configures Front Door to route traffic to the backend App Service using Azure's private backbone network via Private Link, rather than routing over the public internet.
* **Managed WAF Rule Sets:** Within the Web Application Firewall (`azurerm_cdn_frontdoor_firewall_policy.web_firewall_policy`), two comprehensive managed rulesets are applied in `Prevention` mode:
  * **Default Rule Set (`Microsoft_DefaultRuleSet` version `2.1`):** Protects against common web vulnerabilities (OWASP Top 10).
  * **Bot Manager Rule Set (`Microsoft_BotManagerRuleSet` version `1.1`):** Detects and mitigates malicious bot traffic.

These rulesets and networking capabilities are only compatible with the Premium SKU and are omitted in Standard deployments to control costs.

---

## 3. Staging Deployment Slots

* **Variables:** `enable_staging_slot` (boolean, default: `false`)
* **Trigger:** Active when `enable_staging_slot` is set to `true`.
* **Relevant Files:** `infra/terraform/web.tf`

### Behavior
To support zero-downtime releases and pre-production verification, we support deploying code into a secondary staging slot:

* **Staging Slot Provisioning:** Provisions an `azurerm_linux_web_app_slot` named `staging` attached to the main App Service.
* **Dedicated Private Endpoints:** Provisions a dedicated `azurerm_private_endpoint` named `${local.service_prefix}-staging-pe` pointing to the staging slot target (`sites-staging`), ensuring secure backchannel verification from within the virtual network.
* **SKU Constraint Verification:** To prevent runtime deployment failures, the staging slot resource includes a Terraform `precondition` block:
  ```hcl
  lifecycle {
    precondition {
      condition     = contains(local.slot_supported_skus, upper(var.webapp_sku))
      error_message = "Deployment slots require Standard or higher App Service plans."
    }
  }
  ```
  This guarantees that the App Service plan SKU is compatible with slot allocation (e.g., Standard or Premium levels like `P0V3`, `P1V3`). Deployments using cheaper SKUs (such as `B1` or shared tiers) will fail gracefully during the Terraform plan phase.

---

## 4. Non-Production Basic Authentication

* **Variables:** `aspnetcore_environment` (string), `development_basic_auth_password` (sensitive string, default: `""`)
* **Trigger:** Active when `aspnetcore_environment != "Production"`.
* **Relevant Files:** `infra/terraform/locals.tf`, `infra/terraform/web.tf`

### Behavior
To prevent unauthorized public discovery and access during development and testing phases, a basic HTTP authentication mechanism is optionally supported:

* **App Settings Injection:** In `locals.tf`, the `web_app_settings` map dynamically merges a configuration key if the target ASP.NET Core environment is not "Production":
  ```hcl
  web_app_settings = merge({
    "ASPNETCORE_ENVIRONMENT"                = var.aspnetcore_environment
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = azurerm_application_insights.application-insights.connection_string
    ...
    }, var.aspnetcore_environment != "Production" ? {
    "DevelopmentBasicAuthPassword" = var.development_basic_auth_password
  } : {})
  ```
* **Application Protection:** When `DevelopmentBasicAuthPassword` is injected, the running application codebase intercepts incoming requests in non-production environments and demands credentials matching the specified password.
