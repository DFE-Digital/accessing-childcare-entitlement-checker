---
title: Respond to DDoS attack
layout: sub-navigation
order: 5
sectionKey: How-to guides
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
  key: Respond to DDoS attack
---
Follow this runbook to respond to and mitigate a suspected Layer 7 or volumetric Distributed Denial of Service (DDoS) attack.

## Step 1: Confirm the attack using Azure Front Door telemetry

1. Log into the Azure Portal and navigate to your Azure Front Door Premium profile.
2. Select **Security** under the settings pane and check the WAF Log Analytics or diagnostics dashboards.
3. Check for:
   * Spikes in `Blocked` requests.
   * Spikes in 403 Forbidden responses (requests dropped by Front Door WAF).
   * High traffic volume from specific foreign IP ranges or unusual User-Agents.

## Step 2: Mitigate Layer 7 floods using the WAF

Our WAF policy (`web_firewall_policy` in `frontdoor.tf`) runs in Prevention Mode. If the automated rulesets are not catching a specific emerging threat vector, configure and apply custom filtering rules.

### Action A: Block traffic by country code
Apply a geolocation filter if the attack originates from regions outside your target demographic (UK-based parents):
1. Under `web_firewall_policy` in `frontdoor.tf`, define a custom rule to filter by Geolocation (e.g., block non-UK traffic if acceptable).
2. Deploy the updated configurations using Terraform.

### Action B: Apply rate limiting rules
Configure custom rate-limiting rules in Azure Front Door to limit client IP requests (for example, limiting clients to 100 requests per minute per IP):
1. In `frontdoor.tf`, add a custom rate-limiting rule to the `azurerm_cdn_frontdoor_firewall_policy`:
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
2. Deploy the changed template via the GitHub Actions deployment pipeline (**Deploy Environment**).

## Step 3: Verify and enforce the backend App Service lockdown

Block direct bypass traffic to ensure the App Service only accepts requests originating from Azure Front Door:
1. Confirm that the App Service's Access Restrictions (`ip_restriction` inside `web.tf`) are set to Deny all except `AzureFrontDoor.Backend`.
2. Run a Terraform drift check if any unauthorised custom rules or public rules were manually injected in the Azure portal:
   ```bash
   terraform plan
   ```
3. Overwrite any manual drift by running:
   ```bash
   terraform apply -auto-approve
   ```
