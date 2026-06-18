---
title: Respond to DDoS attack
layout: page
showPagination: true
order: 5
sectionKey: Runbooks
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
---
This runbook outlines the steps to take when the service is experiencing a suspected Layer 7 or volumetric Distributed Denial of Service (DDoS) attack.

## Step 1: Confirm the attack via telemetry
1. Log into the Azure Portal and navigate to Azure Front Door Premium profile.
2. Select Security under the settings pane and check the WAF Log Analytics or diagnostics dashboards.
3. Check for:
   - Spikes in `Blocked` requests.
   - Spikes in 403 Forbidden responses (requests dropped by Front Door WAF).
   - High traffic volume from specific foreign IP ranges or unusual User-Agents.

## Step 2: Leverage WAF to mitigate layer 7 floods
Our WAF policy (`web_firewall_policy` in `fromtdoor.tf`) runs in Prevention Mode. If the automated rulesets are not catching a specific emerging threat vector, we must apply custom filtering rules.

### Action a: Block traffic by country code
If the attack is originating from specific regions where your target demographic (UK-based parents) is not located:
1. Under `web_firewall_policy` in `fromtdoor.tf`, define a custom rule filtering by Geolocation (e.g. Block non-UK traffic if acceptable).
2. Apply the configuration using Terraform.

### Action b: Apply rate limiting
Azure Front Door allows custom rate-limiting rules. To limit clients to (for example) 300 requests per 5 minutes per IP:
1. In `fromtdoor.tf`, add a custom rate-limiting rule to the `azurerm_cdn_frontdoor_firewall_policy`:
   ```hcl
   # Example structural change to propose to the Terraform configurations
   custom_rule {
     name             = "RateLimitAll"
     enabled          = true
     action           = "Block"
     type             = "RateLimitRule"
     priority         = 100
     rate_limit_duration_in_minutes = 1
     rate_limit_threshold           = 100

     match_condition {
       match_variable = "RemoteAddr"
       operator       = "Any"
     }
   }
   ```
2. Deploy the changed template via the GitHub Actions deployment pipeline (`Deploy Environment`).

## Step 3: Verify backend App Service lockdown
Ensure that the App Service is not receiving direct attack traffic bypassing Front Door:
1. Confirm the App Service's Access Restrictions (`ip_restriction` inside `web.tf`) are set to Deny all except `AzureFrontDoor.Backend`.
2. If any unauthorized custom rules or public rules were manually injected in the Azure portal, run a drift check using Terraform:
   ```bash
   terraform plan
   ```
   and overwrite manual drift:
   ```bash
   terraform apply -auto-approve
   ```
