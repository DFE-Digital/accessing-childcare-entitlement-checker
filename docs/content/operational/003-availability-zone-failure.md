---
title: Availability zone failure
layout: page
showPagination: true
order: 3
sectionKey: Operational
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
A single Azure Availability Zone in the `UK South` region experiences an outage or severe network partition.

## Impact

The loss of one or more App Service instances or storage replica paths, temporarily degrading computing capacity.

## Prevention

- Zone Redundant Architecture: Ensuring the Azure App Service Plan spreads instances across multiple physical availability zones in UK South.
- Total Stateless Design: Multi-step journey progress is entirely maintained in client-side encrypted session cookies (using ASP.NET Core Data Protection). There is no server-side sticky session requirement or centralised database.
- Geo-Redundant Storage (RA-GRS): Storing deployment zip packages in an RA-GRS Storage Account, ensuring the artifacts remain readable even if the storage path in the primary zone goes offline.

## Detection

- Front Door Health Probes: Azure Front Door continuously probes backend App Service instances.
- Platform Alerts: Service health notifications in the Azure Portal.

## Response & Recovery

No manual intervention is required. Azure Front Door's backend load-balancing will automatically detect unhealthy probe responses from the affected zone and stop routing traffic to those instances within seconds.

Because the service is stateless, affected users redirected to the remaining instances will experience zero data loss or session expiration, resuming their forms seamlessly.

## Related runbooks

- [Investigate service degradation](/runbooks/007-investigate-service-degradation/)
