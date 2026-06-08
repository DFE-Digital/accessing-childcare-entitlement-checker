---
title: Regional failure
layout: page
showPagination: true
order: 4
sectionKey: Operational
eleventyNavigation:
  parent: Operational
---

A catastrophic, full-region outage affects the entire Azure `UK South` infrastructure.

## Impact

Total service outage. The public-facing Azure App Service and its regional log analytics are completely offline.

## Prevention

- Read-Access Geo-Redundant Storage (RA-GRS): Deployment zip files are backed up in geo-redundant storage, meaning the deployment assets survive the regional disaster.
- Infra-as-Code (Terraform): The entire application environment is declared in Terraform, allowing the infrastructure to be provisioned in a secondary region within minutes.

## Detection

- Global availability alerts from Azure Front Door.
- Azure Status Page notifications.

## Response

1. Assess the expected duration of the Microsoft `UK South` regional outage.
2. If outage duration is expected to exceed the SLA, initiate a redeployment to a backup region (e.g., `UK West`).

## Recovery

Redeploy the application to a secondary region (such as `UK West`) using the Terraform workspace by providing the updated target region variables. Once the secondary App Service is online, update the Azure Front Door endpoint's origin routes to direct public ingress to the new origin.

## Related runbooks

- [Regional failover](/runbooks/006-regional-failover/)
