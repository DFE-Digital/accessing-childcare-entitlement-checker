---
title: Roll back a deployment
layout: page
showPagination: true
order: 1
sectionKey: Runbooks
eleventyNavigation:
  parent: Runbooks
---

This runbook describes the procedure to roll back a deployment when a broken package or bad configuration is introduced to production.

## Standard Rollback: Instant Slot Swap (Zero Downtime)

For environments with deployment slots enabled (such as Production), the prior stable release is preserved in the `staging` slot immediately after a swap. The fastest, safest, and only zero-downtime rollback method is to instantly swap the slots back.

### Step 1: Trigger the Rollback Slot Swap via Azure CLI

Run the following commands in an authenticated terminal (PowerShell or Bash):

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

*This swap operation is atomic, occurs in seconds, and has zero downtime or container cold-start penalty.*

## Fallback Rollback: Via GitHub Actions

If deployment slots are not enabled for the target environment (e.g., Development or Test running on B1 plans), or if the slots are out of sync, you must redeploy the previous successful release package.

### Step 1: Identify the Prior Stable Release
1. Go to the GitHub repository and select the **Actions** tab.
2. Filter the workflows by **Deploy Environment** or **Build & Deploy** to identify the last successful run on the target environment (e.g., `Production` or `Staging`).
3. Note the git commit hash (SHA) or release tag corresponding to that successful build.

### Step 2: Trigger the Rollback Run
1. Go to the **Actions** tab and select the **Build & Deploy** or **Deploy Environment** workflow.
2. Click **Run workflow**.
3. Choose the branch or tag identified in Step 1 (e.g., `release/vX.Y` or a specific commit SHA).
4. Select the target environment and trigger the workflow.
5. Monitor the pipeline to ensure that both the Terraform Provisioning and Zip Deploy stages succeed.

## Emergency Fallback: Direct Zip Deploy Via Azure CLI

If GitHub Actions is down or degraded and slots are not an option, you can deploy the previous deployment package directly.

### Step 1: Download the Prior Deployment Package
Our deployment packages are stored with Read-Access Geo-Redundant Storage (RA-GRS) support. 
1. Retrieve the stable `.zip` deployment artifact matching the previous version from the Azure storage account or your local cache.

### Step 2: Authenticate and Deploy
Run the following commands in an authenticated terminal (PowerShell or Bash):

```powershell
# 1. Log in to your Azure account
az login

# 2. Select the correct subscription
az account set --subscription "<SUBSCRIPTION_ID>"

# 3. Deploy the stable zip package to the App Service
az webapp deploy `
  --resource-group "<RESOURCE_GROUP_NAME>" `
  --name "<WEB_APP_NAME>" `
  --src-path "/path/to/previous/webapp.zip" `
  --type zip
```

## Post-Rollback Validation

1. Verify the `/health` endpoint responds with a `200 OK`.
2. Open the application in an incognito browser window and complete a full entitlement evaluation flow to ensure that state persistence (encrypted cookies) and rules calculation function correctly.
3. Check Application Insights in the Azure Portal to confirm that the error rate is dropping.
