---
title: Application crashes
layout: page
showPagination: true
order: 2
sectionKey: Operational
eleventyNavigation:
  parent: Operational
---

Application may crash because of .NET unhandled exceptions, memory leaks, or resource exhaustion on the App Service Plan.

## Impact

Partial or complete service unavailability for parents/carers accessing the web tool.

## Prevention

- Auto-Scale & Sizing: Deploying on a multi-instance Linux App Service Plan with a minimum of 2 active instances.
- Lightweight Endpoint Health Check: Standard ASP.NET Core health check middleware mapped to `/health` which avoids loading heavy pages or state logic.
- Monitoring Resource Caps: Tracking CPU, Memory, and Disk utilisation limits within Azure Application Insights.

## Detection

- Azure App Service Health Probe: Azure's internal load balancer probes the `/health` path.
- Application Insights Alerts: Configured thresholds for server exceptions and slow requests.
- Availability Monitoring: Ping tests targeting the Azure Front Door endpoint.

## Response

- Let the Azure App Service platform automatically evict and restart unhealthy instances.
- For persisting crashes, isolate and inspect Application Insights diagnostic logs.
- Temporarily scale out the App Service Plan to handle resource starvation.

## Recovery

Azure App Service is configured with active health checks. If an instance fails to respond to probes, Azure's internal load balancer evicts the instance from rotation after a maximum of 5 minutes, automatically restarting and rebuilding the container package on a healthy virtual worker.

## Related runbooks

- [Investigate service degradation](/runbooks/007-investigate-service-degradation/)
