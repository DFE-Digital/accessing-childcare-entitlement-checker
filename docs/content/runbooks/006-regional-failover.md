---
title: Regional failover
layout: page
showPagination: true
order: 6
sectionKey: Runbooks
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
---
This runbook outlines the process for migrating the service from our primary Azure region (`UK South`) to a secondary region (e.g., `UK West`) in the event of a catastrophic regional Azure outage.

## Important architectural notes
Because our application is completely stateless and maintains user session states entirely within encrypted client session cookies rather than a stateful database or server-side caching layer, a regional failover does not require any database migration, replication check, or data reconciliation. 

## Step 1: Provision infrastructure in secondary region
The infrastructure is fully defined as Terraform files under `infra/terraform/`.

1. Go to the GitHub repository and select the Actions tab.
2. Select the Deploy Environment workflow.
3. Click Run workflow and select the appropriate branch (e.g., `release/vX.Y`).
4. In the custom parameters, change the primary deployment region variable (e.g., `location` or setting `location_override`) to `ukwest`. (If variables are governed by `.tfvars` files, make a quick hotfix commit to the active deployment branch to set the region in `infra/terraform/environments/production.tfvars` from `uksouth` to `ukwest`).
5. Run the workflow to provision the App Service, App Service Plan, Storage, and diagnostics within the secondary region (`UK West`).

## Step 2: Route public ingress via Azure Front Door
Since Azure Front Door is a global edge routing service, its routing tables can be dynamically reconfigured to point to the new backend app service.

1. Once the secondary App Service is successfully provisioned, obtain its default hostname (e.g., `s-web-app-service-ukwest.azurewebsites.net`).
2. Update the `host_name` under the `azurerm_cdn_frontdoor_origin` resource in `fromtdoor.tf` to point to the new Web App hostname.
3. Apply the Terraform change via the pipeline to instantly switch global routing over to the secondary region.

## Step 3: Validate the secondary instance
1. Access the custom public domain and perform several test evaluations to verify that the Rules Engine and Web Application are performing correctly.
2. Monitor App Insights logs mapped to the secondary region to confirm that telemetry is being recorded.
