---
title: Denial of service
layout: page
showPagination: true
order: 5
sectionKey: Operational
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
Attackers generate malicious layer-7 HTTP flooding or volumetric layer-3 or layer-4 traffic to overwhelm the application.

## Impact

Slow performance or total server timeout for legitimate parents trying to use the entitlement checker.

## Prevention

- Front Door WAF Policy: The service is fronted by Azure Front Door Premium with a security policy set to Prevention Mode.
- Rule Protection: Active protection using the `Microsoft_DefaultRuleSet` (v2.1) and `Microsoft_BotManagerRuleSet` (v1.1) to automatically block known malicious web threats, SQL injections, scripting attacks, and botnets.
- Strict Ingress Lockdown: The underlying App Service is protected by strict IP security restrictions, configured via Terraform to Deny all traffic except from the `AzureFrontDoor.Backend` service tag. Attackers cannot bypass Front Door's WAF by hitting the App Service's direct IP.

## Detection

- Front Door Analytics: Spikes in 403 (Blocked by WAF) and 503 HTTP codes.
- App Insights Latency Metrics: Sudden rise in response duration and CPU utilisation.

## Response

- Monitor blocked requests in the Log Analytics Workspace.
- If an attack bypasses current rule sets, configure custom Azure Front Door WAF rate-limiting or IP block rules and apply via Terraform.

## Recovery

Azure Front Door's global Edge network and built-in WAF automatically filter volumetric attacks at the edge before they reach our App Service compute. Once rules are adjusted to block emerging signatures, the system naturally heals.

## Related runbooks

- [Respond to DDoS attack](/runbooks/005-ddos-attack/)
