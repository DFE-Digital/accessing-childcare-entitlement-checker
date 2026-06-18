---
title: Rotate secrets
layout: page
showPagination: true
order: 4
sectionKey: Runbooks
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
---
This runbook outlines how to rotate administrative secrets and integration passwords used by the Accessing Childcare Entitlement Checker service.

## Note on subscription credentials
This application is configured with zero-password subscription deployments. Deployment pipelines use Azure OpenID Connect (OIDC) Federated Credentials for authentication. There are no client secrets or service principal passwords stored in GitHub Actions for subscription access, meaning subscription secrets do not need to be rotated.

The only credentials that require rotation procedures are environment-specific Basic Auth keys and diagnostic keys.

## Rotatable secrets list

| Secret Name | Scope | Location of Secret | Affected Resources |
| :--- | :--- | :--- | :--- |
| `DEVELOPMENT_BASIC_AUTH_PASSWORD` | Pre-prod access control | GitHub Actions Secrets | Dev, Test, Staging App Service Instances |
| Log Analytics Workspace Key | Diagnostic storage | Azure Key Vault / App Settings | App Service telemetry channels |

## Procedure: Rotate basic auth password (`Development_basic_auth_password`)

### Step 1: Generate a new strong password
Generate a secure 32-character random string.

### Step 2: Update GitHub actions secrets
1. Go to the GitHub repository.
2. Navigate to Settings -> Secrets and variables -> Actions.
3. Under Repository secrets, locate `DEVELOPMENT_BASIC_AUTH_PASSWORD`.
4. Click Edit, paste the new password, and save.

### Step 3: Redeploy with Terraform
The new password is fed to App Service environment variables via the Terraform variable `development_basic_auth_password` (which builds `local.web_app_settings` in `web.tf`).
1. Trigger the Deploy Environment workflow for the non-production environments (Dev, Test, Staging).
2. The pipeline's Terraform phase (`terraform apply`) will detect the secret update and automatically inject the new value into the App Service configuration settings.
3. The App Service container will restart automatically to load the new settings.

## Procedure: Rotate app insights connection strings / keys
If diagnostic collection keys are compromised, follow these steps:
1. Log into the Azure Portal.
2. Go to the Log Analytics Workspace or Application Insights resource.
3. Select API Access or Locks/Keys and select Regenerate on the target secondary key.
4. Update the corresponding configuration variables in the Terraform variables (`tfvars` files) or GitHub environment variables.
5. Deploy via GitHub Actions to apply the updated connection keys to the Web App instances.
6. Once validated, revoke the previous primary key.
