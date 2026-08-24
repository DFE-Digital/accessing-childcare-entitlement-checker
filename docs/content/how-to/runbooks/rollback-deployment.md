---
title: Roll back a deployment
layout: sub-navigation
order: 1
sectionKey: How-to guides
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
  key: Roll back a deployment
---
Follow this runbook to quickly roll back a deployment when you introduce a broken package or bad configuration to production.

## Swap deployment slots (Standard rollback with zero downtime)

If the target environment has deployment slots enabled (such as Production), the prior stable release is preserved in the `staging` slot. Execute an instant slot swap to restore the previous configuration without causing cold-start delays or downtime.

### Step 1: Trigger the rollback slot swap via Azure CLI

Run the following commands in an authenticated terminal (PowerShell or Bash) to perform an atomic swap:

```powershell
# 1. Log in to your Azure account
az login

# 2. Select the correct subscription
az account set --subscription "<SUBSCRIPTION_ID>"

# 3. Swap the staging slot back into production
az webapp deployment slot swap `
  --resource-group "<RESOURCE_GROUP_NAME>" `
  --name "<WEB_APP_NAME>" `
  --slot "staging" `
  --target-slot "production"
```

## Revert and redeploy via Git (Fallback rollback method)

Redeploy the last known successful release package if the target environment (such as Development or Test running on B1 plans) does not have deployment slots enabled, or if the slots are out of sync. Because the deployment pipelines are strictly event-driven, the rollback must be triggered by pushing a revert commit to the target branch.

### Step 1: Identify the last stable commit
1. Open your terminal and check the git commit log:
   ```bash
   git log --oneline -n 20
   ```
2. Identify the commit SHA of the last known successful release before the broken changes were merged.

### Step 2: Create a revert commit
1. Create a new branch off your deployment branch (e.g. `releases/vX.Y`:
   ```bash
   git fetch origin
   git checkout releases/v1.1
   git pull
   git checkout -b hotfix/rollback-broken-change
   ```
2. Revert the broken commit(s) using git revert. Pass the commit SHAs of the broken changes (newest first):
   ```bash
   git revert <BROKEN_COMMIT_SHA>
   ```
3. Save the commit message and push the hotfix branch to GitHub:
   ```bash
   git push -u origin hotfix/rollback-broken-change
   ```

### Step 3: Merge and trigger the deployment pipeline
1. Raise a Pull Request (PR) merging your hotfix branch back into the active branch (`releases/vX.Y` or `main`).
2. Once approvals are met, merge the PR.
3. Merging or pushing to these branches automatically triggers the corresponding `workflow-release.yml` (for Production/Staging) or `workflow-main.yml` (for Dev/Test) pipeline.
4. Monitor the pipeline in the **Actions** tab to ensure the deployment and automated testing steps complete successfully.

## Deploy zip package via Azure CLI (Emergency fallback method)

Deploy the previous deployment package directly if GitHub Actions is degraded and slot swaps are unavailable.

### Step 1: Download the prior deployment package from GitHub
We attach our compiled web application artefact (`webapp.zip`) directly to each release on GitHub. Do not attempt to search Azure storage accounts for these packages.

1. Navigate to the GitHub repository and click on the **Releases** section on the right-hand sidebar.
2. Locate the last known stable release (e.g., `Release v1.2.0`).
3. Under the **Assets** header for that release, click on **`webapp.zip`** to download it to your local machine.

### Step 2: Authenticate and deploy
Run the following commands in an authenticated terminal (PowerShell or Bash) to force deployment:

```powershell
# 1. Log in to your Azure account
az login

# 2. Select the correct subscription
az account set --subscription "<SUBSCRIPTION_ID>"

# 3. Deploy the stable zip package to the App Service
az webapp deploy `
  --resource-group "<RESOURCE_GROUP_NAME>" `
  --name "<WEB_APP_NAME>" `
  --src-path "/path/to/downloaded/webapp.zip" `
  --type zip
```

## Run post-rollback validation checks

1. Verify that the `/health` endpoint responds with a `200 OK`.
2. Open the application in an incognito browser window and complete a full entitlement evaluation flow to verify that state persistence (encrypted cookies) and rules calculation function correctly.
3. Inspect Application Insights in the Azure Portal to confirm that error rates are dropping.
