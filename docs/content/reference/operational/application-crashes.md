---
title: Application crashes
layout: sub-navigation
order: 2
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
The application may crash due to .NET unhandled exceptions, memory leaks, or resource exhaustion on the App Service Plan.

## Impact

Partial or complete service unavailability for users accessing the web tool.

## Prevention

- Auto-scale and sizing: The team deploys the application on a multi-instance Linux App Service Plan with at least two active instances.
- Lightweight endpoint health check: The standard ASP.NET Core health check middleware maps to `/health`. This approach avoids loading intensive pages or state logic.
- Monitoring resource caps: The team tracks CPU, memory, and disk utilisation limits within Azure Application Insights.

## Detection

- Azure App Service health probe: The internal load balancer probes the `/health` path.
- Application Insights alerts: Configured thresholds trigger alerts for server exceptions and slow requests.
- Availability monitoring: Ping tests target the Azure Front Door endpoint.

## Response

- Automatic eviction: The Azure App Service platform evicts and restarts unhealthy instances.
- Log inspection: The team isolates and inspects Application Insights diagnostic logs for persisting crashes.
- Scale-out: The team temporarily scales out the App Service Plan to address resource starvation.

## Recovery

The team configures Azure App Service with active health checks. If an instance fails to respond to probes, the internal load balancer evicts the instance from rotation after five minutes. This action initiates an automatic restart and container rebuild.

## Related runbooks

- [Investigate service degradation](/how-to/runbooks/investigate-service-degradation/)
