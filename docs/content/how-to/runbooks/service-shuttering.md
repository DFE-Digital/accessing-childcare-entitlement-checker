---
title: Service shuttering
layout: sub-navigation
order: 9
sectionKey: How-to guides
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
  key: Service shuttering
---
Follow this runbook to shutter the web application and display a static, GOV.UK-branded "Service Unavailable" page. This is typically used during planned maintenance windows or during severe emergency service degradation.

Shuttering is controlled entirely via Azure Front Door routing. The static shutter page is hosted securely in a low-cost, locked-down Azure Storage Account container and is automatically kept up-to-date by the main CI/CD deployment pipeline.

We have two methods for toggling the shutter state:

1. **GitHub Actions Workflow (Recommended)** - Automated trigger using GitHub UI.
2. **Azure Portal (ClickOps)** - Manual configuration through the Azure Portal interface.

## Using GitHub Actions Workflow (Recommended)

This method triggers the **Shutter Service** GitHub Actions workflow, which automatically runs the necessary Azure CLI commands securely.

### Step 1: Open the Shutter Service Workflow

1. Navigate to the repository on GitHub.
2. Click the **Actions** tab at the top of the page.
3. In the left-hand sidebar under **All workflows**, select **Shutter Service**.

### Step 2: Run the Workflow

1. On the right-hand side, click the **Run workflow** dropdown button.
2. Configure the following inputs:
   - **Target Environment**: Select the target environment (e.g., `development`, `test`, `staging`, `production`).
   - **Action**: Choose either `shutter` (to enable the shutter page) or `restore` (to restore the live service).
3. Click the green **Run workflow** button to execute.

This workflow will perform the following actions:
- Authenticate securely to Azure via OpenID Connect (OIDC).
- Automatically resolve the correct Azure resource group and Front Door route names for your selected environment.
- **For Shuttering**:
  - Safely update the Front Door Route (`<prefix>-web-fd-route`) to forward traffic to the `shutter-origin-group`.
  - Set the route's **Origin path** to `/shutter` so assets are fetched from the correct container directory.
  - Associate both the `SecurityRules` and `ShutterRules` rule sets, ensuring that critical security redirects remain functional while the site is shuttered.
- **For Restoring**:
  - Revert the Front Door Route to point back to the main App Service `web-fd-origin-group`.
  - Clear the route's **Origin path** (sets it to empty).
  - Restore the route's association to use only the `SecurityRules` rule set.

## Manual Toggle via Azure Portal (ClickOps)

Use this method if you do not have permission to run GitHub Actions workflows or prefer using the graphical user interface.

### Step 1: Locate the Front Door Profile

1. Log into the [Azure Portal](https://portal.azure.com).
2. Search for **Front Door and CDN profiles** in the global search bar.
3. Select the Front Door profile for your target environment (named `<prefix>-web-fd-profile`, e.g., `s279p01-web-fd-profile`).

### Step 2: Open the Front Door Manager

1. In the left-hand navigation pane under **Settings**, click on **Front Door manager**.
2. Locate the main route in the routing table, named `<prefix>-web-fd-route` (e.g., `s279p01-web-fd-route`).

### Step 3: Edit the Route to Shutter the Site

1. Click the **...** (three dots) button at the right-hand end of the route row and click **Edit route**.
2. Scroll down to the **Routing details** section:
   - **Origin group**: Change the selection from the default web origin group (`<prefix>-web-fd-origin-group`) to the shutter origin group (`<prefix>-shutter-fd-origin-group`).
   - **Origin path**: Type `/shutter`.
   - **Rules**: In the rules dropdown, ensure both `<prefix>SecurityRules` and `<prefix>ShutterRules` are selected. (Do not de-select `<prefix>SecurityRules` as it contains critical security.txt redirect paths).
3. Click the **Update** button at the bottom of the page.
4. Click **Save** on the Front Door manager page to commit and propagate the changes globally.

### Step 4: Revert the Changes to Restore the Site

To restore normal operations and route traffic back to the main App Service:
1. Go back to the **Front Door manager** and edit the route `<prefix>-web-fd-route`.
2. Scroll to the **Routing details** section:
   - **Origin group**: Change back to `<prefix>-web-fd-origin-group`.
   - **Origin path**: Delete the text `/shutter` so that the field is **blank/empty**.
   - **Rules**: De-select `<prefix>ShutterRules` so that only `<prefix>SecurityRules` remains selected.
3. Click **Update** and then **Save** to apply the changes.

## Post-Failover Validation Checks

Once either method is executed, wait **1–2 minutes** for the DNS and routing configurations to propagate globally across Azure Front Door edge POPs, then perform the following validation:

1. **Verify Anonymous Shutter Access:**
   - Open a browser in incognito mode and navigate to your service URL: `https://[your-service-domain]/`
   - You should see the GOV.UK-branded "Sorry, the service is unavailable" page.
2. **Verify Route Rewriting (Deep Paths):**
   - Navigate to a nested path: `https://[your-service-domain]/some-random-page-path`
   - The address bar should remain `/some-random-page-path`, but the content served must still be the GOV.UK "Sorry, the service is unavailable" page.
3. **Verify Strict Security Isolation:**
   - Try accessing the Storage Account blob URL directly (e.g. `https://<account>.blob.core.windows.net/shutter/index.html`).
   - It **must** return an HTTP `403 Forbidden` response, verifying that anonymous public access is correctly blocked and that only Azure Front Door is authorized to read the content.
