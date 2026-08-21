---
title: Rotate secrets
layout: sub-navigation
order: 4
sectionKey: How-to guides
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
  key: Rotate secrets
---
Follow this runbook to rotate administrative secrets and integration passwords used by the Accessing Childcare Entitlement Checker service.

## Retain subscription credentials

The deployment pipelines use Azure OpenID Connect (OIDC) Federated Credentials to authenticate without passwords. Do not rotate subscription secrets because no client secrets are stored in GitHub Actions.

Rotate only the environment-specific keys listed below.

## Identify rotatable secrets

| Secret Name                       | Scope                   | Location of Secret             | Affected Resources                       |
|:----------------------------------|:------------------------|:-------------------------------|:-----------------------------------------|
| `DEVELOPMENT_BASIC_AUTH_PASSWORD` | Pre-prod access control | GitHub Actions Secrets         | Dev, Test, Staging App Service Instances |
| Log Analytics Workspace Key       | Diagnostic storage      | Azure Key Vault / App Settings | App Service telemetry channels           |

## Rotate the Basic Auth password

### Step 1: Generate a new strong password
Generate a secure 32-character random string.

### Step 2: Update GitHub Actions secrets
1. Navigate to the GitHub repository.
2. Go to **Settings -> Secrets and variables -> Actions**.
3. Locate `DEVELOPMENT_BASIC_AUTH_PASSWORD` under Repository secrets.
4. Click **Edit**, paste the new password, and click **Update secret**.

### Step 3: Redeploy with Terraform to inject the new settings
Trigger the pipeline to automatically feed the new password to the App Service environment variables via the Terraform `development_basic_auth_password` variable (which builds `local.web_app_settings` in `web.tf`).
1. Trigger the **Deploy Environment** workflow for the non-production environments (Dev, Test, Staging).
2. Let the pipeline's Terraform phase (`terraform apply`) detect the secret update and automatically inject the new value into the App Service configuration settings.
3. Allow the App Service container to restart automatically and load the new settings.

## Rotate Application Insights connection strings / keys

If diagnostic collection keys are compromised, execute the following steps:
1. Log into the Azure Portal.
2. Navigate to the target **Log Analytics Workspace** or **Application Insights** resource.
3. Select **API Access** or **Locks/Keys** and click **Regenerate** on the target secondary key.
4. Update the corresponding configuration variables in the Terraform variables (`tfvars` files) or GitHub environment variables.
5. Deploy via GitHub Actions to apply the updated connection keys to the Web App instances.
6. Verify diagnostic collection, then revoke the previous primary key.
