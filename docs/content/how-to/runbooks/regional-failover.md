---
title: Regional failover
layout: sub-navigation
order: 6
sectionKey: How-to guides
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
  key: Regional failover
---
Follow this runbook to migrate the service from the primary Azure region (`UK South`) to a secondary region (such as `UK West`) in the event of a catastrophic regional Azure outage.

## Prepare for regional failover

The application uses an ephemeral Azure Cache for Redis and is functionally stateless. Active user sessions are lost during a regional failover, prompting users to restart. No database synchronisation is required. To initiate failover, push a configuration hotfix to the active deployment branch.

## Step 1: Update Bicep and Terraform configuration variables

Checkout a hotfix branch locally and update the target region variables.

1. **Navigate to your local repository** and checkout a new hotfix branch off the active deployment branch (e.g. `releases/vX.Y` or `main`):
   ```bash
   git checkout releases/v1.1
   git checkout -b hotfix/failover-to-ukwest
   ```
2. **Update the Bicep bootstrap region:**
   Open `infra/bicep/environments/<environment>.params.json` (e.g. `production.params.json`) and change the value of `location` to `ukwest`:
   ```json
   {
     "location": { "value": "ukwest" }
   }
   ```
3. **Update the Terraform region parameters:**
   Open `infra/terraform/environments/<environment>.tfvars` (e.g. `production.tfvars`) and change both `location` and `location_short_code`:
   ```hcl
   location            = "ukwest"
   location_short_code = "ukw"
   ```
4. **Commit and push the configuration changes:**
   ```bash
   git add infra/
   git commit -m "chore(infra): failover environment to ukwest due to primary region outage"
   git push -u origin hotfix/failover-to-ukwest
   ```

## Step 2: Merge the Pull Request and execute failover

Because Azure Front Door dynamically references the hostname via `azurerm_linux_web_app.web-app-service.default_hostname`, Terraform automatically recreates the App Service in the secondary region and updates the Front Door global routing origin in a single, atomic apply step. Do not edit `frontdoor.tf` manually.

1. Raise a Pull Request (PR) merging your hotfix branch into the active branch (`releases/vX.Y` or `main`).
2. Once approved, merge the PR.
3. Merging triggers the `Release Pipeline` (`workflow-release.yml`) or `Main Integration` (`workflow-main.yml`) workflow.
4. Monitor the pipeline progress in the **Actions** tab. The pipeline will automatically bootstrap the state storage in UK West, provision the secondary App Service, run the automated E2E tests, and dynamically update your Azure Front Door routing tables.

## Step 3: Run post-failover validation checks

1. Access the custom public domain and perform several test evaluations to verify that the Rules Engine and Web Application are performing correctly.
2. Monitor App Insights logs mapped to the secondary region to confirm that telemetry is being recorded.
