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

1. **Azure Cloud Shell (Recommended)** - Quick, script-driven toggle that prevents syntax errors.
2. **Azure Portal (ClickOps)** - Manual configuration through the Azure Portal interface.

## Using Azure Cloud Shell (Recommended)

This method uses the pre-installed Azure CLI inside the browser-based Azure Cloud Shell. It executes the failover script located in the repository.

### Step 1: Open Azure Cloud Shell

1. Log into the [Azure Portal](https://portal.azure.com).
2. Click the **Cloud Shell** icon (`>_`) in the top-right toolbar.
3. Ensure the environment dropdown is set to **Bash**.

### Step 2: Retrieve the Failover Script

If you do not have the repository cloned in your Cloud Shell session, you can download the script directly from the repository using `curl` or `wget`, or clone the codebase:

```bash
# Clone the repository if not already present
git clone https://github.com/DFE-Digital/accessing-childcare-entitlement-checker.git
cd accessing-childcare-entitlement-checker/infra/scripts
chmod +x failover.sh
```

### Step 3: Execute the Failover

To find your target `<environment_prefix>`, check the environment code (e.g. `d01` for development, `t01` for test, `s01` for staging, `p01` for production).

#### To Enable the Shutter (Failover):
Run the script with the environment prefix and `shutter`:

```bash
./failover.sh <environment_prefix> shutter
# Example: ./failover.sh p01 shutter
```

This script will:
- Safely update the Front Door Route (`<prefix>-web-fd-route`) to forward traffic to the `shutter-origin-group`.
- Set the route's **Origin path** to `/shutter` so assets are fetched from the correct container directory.
- Associate both the `SecurityRules` and `ShutterRules` rule sets, ensuring that critical security redirects remain functional while the site is shuttered.

#### To Disable the Shutter (Restore Service):

Run the script with the environment prefix and `restore`:

```bash
./failover.sh <environment_prefix> restore
# Example: ./failover.sh p01 restore
```

This script will:

- Revert the Front Door Route to point back to the main App Service `web-fd-origin-group`.
- Clear the route's **Origin path** (sets it to empty).
- Restore the route's association to use only the `SecurityRules` rule set.

## Manual Toggle via Azure Portal (ClickOps)

Use this method if you do not have access to Cloud Shell or prefer using the graphical user interface.

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
