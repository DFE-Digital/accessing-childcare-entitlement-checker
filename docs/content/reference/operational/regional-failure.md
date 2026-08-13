---
title: Regional failure
layout: sub-navigation
order: 4
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
A catastrophic, full-region outage affects the entire Azure `UK South` infrastructure.

## Impact

Total service outage. The public-facing Azure App Service and its regional log analytics are completely offline.

## Prevention

- Read-Access Geo-Redundant Storage (RA-GRS): Deployment zip files are backed up in geo-redundant storage, ensuring deployment assets survive regional disasters.
- Infra-as-Code (Terraform): The entire application environment is declared in Terraform, allowing the infrastructure to be provisioned in a secondary region within minutes.

## Detection

- Global availability alerts from Azure Front Door.
- Azure Status Page notifications.

## Response

1. Assessment of the expected duration of the Microsoft `UK South` regional outage.
2. If the outage duration is projected to exceed the SLA, initiation of redeployment to a backup region (e.g., `UK West`).

## Recovery

Redeployment of the application to a secondary region (such as `UK West`) is executed using the Terraform workspace by providing the updated target region variables. Once the secondary App Service is online, the Azure Front Door endpoint's origin routes are updated to direct public ingress to the new origin.

## Related runbooks

- [Regional failover](/how-to/runbooks/regional-failover/)
