---
title: Denial of service
layout: sub-navigation
order: 5
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
Attackers generate malicious layer-7 HTTP flooding or volumetric layer-3 or layer-4 traffic to overwhelm the application.

## Impact

Degraded performance or total server timeout for legitimate users accessing the entitlement checker.

## Prevention

- Front Door WAF Policy: The service is fronted by Azure Front Door Premium with a security policy set to Prevention Mode.
- Rule Protection: Active protection using the `Microsoft_DefaultRuleSet` (v2.1) and `Microsoft_BotManagerRuleSet` (v1.1) to automatically block known malicious web threats, SQL injections, scripting attacks, and botnets.
- Strict Ingress Lockdown: The underlying App Service is protected by strict IP security restrictions, configured via Terraform to Deny all traffic except from the `AzureFrontDoor.Backend` service tag. Bypass of Front Door's WAF via direct IP access is prevented.

## Detection

- Front Door Analytics: Spikes in 403 (Blocked by WAF) and 503 HTTP codes.
- App Insights Latency Metrics: Sudden rise in response duration and CPU utilisation.

## Response

- Monitoring of blocked requests in the Log Analytics Workspace.
- If an attack bypasses current rule sets, configuration of custom Azure Front Door WAF rate-limiting or IP block rules and application via Terraform.

## Recovery

Azure Front Door's global Edge network and built-in WAF automatically filter volumetric attacks at the edge before they reach the App Service compute. Following the adjustment of rules to block emerging signatures, normal system performance is restored.

## Related runbooks

- [Respond to DDoS attack](/how-to/runbooks/ddos-attack/)
