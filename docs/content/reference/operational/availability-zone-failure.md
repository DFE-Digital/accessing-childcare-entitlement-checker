---
title: Availability zone failure
layout: sub-navigation
order: 3
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
A single Azure Availability Zone in the `UK South` region experiences an outage or severe network partition.

## Impact

The loss of one or more App Service instances or storage replica paths, temporarily degrading computing capacity.

## Prevention

- Zone Redundant Architecture: The Azure App Service Plan distributes instances across multiple physical availability zones in UK South.
- Total Stateless Design: Multi-step journey progress is maintained in client-side encrypted session cookies (utilising ASP.NET Core Data Protection). No server-side sticky session requirement or centralised database is present.
- Geo-Redundant Storage (RA-GRS): Storing deployment zip packages in an RA-GRS Storage Account ensures the artifacts remain readable even if the storage path in the primary zone is unavailable.

## Detection

- Front Door Health Probes: Azure Front Door continuously probes backend App Service instances.
- Platform Alerts: Service health notifications in the Azure Portal.

## Response & recovery

No manual intervention is required. Azure Front Door's backend load-balancing automatically detects unhealthy probe responses from the affected zone and ceases routing traffic to those instances.

Because the service is stateless, redirection of users to the remaining active instances occurs with zero data loss or session expiration, enabling seamless journey resumption.

## Related runbooks

- [Investigate service degradation](/how-to/runbooks/investigate-service-degradation/)
