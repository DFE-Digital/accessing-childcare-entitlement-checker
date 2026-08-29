---
title: Manage feature flags
layout: sub-navigation
sectionKey: How-to guides
order: 8
includeInBreadcrumbs: true
eleventyNavigation:
  parent: How-to guides
  key: Manage feature flags
---
Configure and toggle feature flags to enable, disable, or test in-development application capabilities locally or within automated test suites.

## Toggle feature flags locally

Manage feature flags for local execution by editing the local JSON configuration file.

1. **Create the file** `src/Dfe.Acec.Web/appsettings.Local.json` if it does not already exist.
2. **Configure the feature flags block** using the following structure:
   ```json
   {
     "FeatureManagement": {
       "HmrcIntegration": true
     }
   }
   ```
3. **Change the boolean value** to `true` to enable the feature, or `false` to disable it. 
4. **Restart the web application** to apply the configuration change.

## Configure feature flags for test execution

Verify application behaviour under different feature flag states during automated tests.

### Configure flags for E2E and A11y tests
Our Playwright test suites (E2E and A11y) load flag states via `TestSettings`.

1. **Open the local configuration file** (`appsettings.Local.json` inside the respective test project folder).
2. **Add the flag configuration** to your `TestSettings` block:
   ```json
   {
     "TestSettings": {
       "TestUrl": "http://localhost:5252/",
       "HmrcIntegrationEnabled": true
     }
   }
   ```
3. **Change the value** of `HmrcIntegrationEnabled` to toggle the simulated integration state during browser automation runs.
