---
title: Deploy an emergency fix
layout: sub-navigation
order: 2
sectionKey: How-to guides
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
  key: Deploy an emergency fix
---
Follow this runbook to develop, test, and deploy an emergency hotfix to resolve critical production bugs or security vulnerabilities.

## Step 1: Isolate the issue and create a hotfix branch

Create a hotfix branch locally off the active release branch (e.g., `releases/vX.Y`), rather than the unreleased `main` branch. 

Execute the following commands:
```bash
git checkout main
git pull
git checkout releases/v1.1
git checkout -b hotfix/critical-issue-fix
```

## Step 2: Implement your changes and validate them locally

Implement the required code correction within the C# projects, then run the full test suite locally before pushing.

1. **Verify rules engine calculations:**
   ```bash
   dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests
   ```
2. **Verify database and integration dependencies:**
   ```bash
   dotnet test tests/AccessingChildcareEntitlementChecker.IntegrationTests
   ```
3. **Verify user journeys (E2E) in the browser:**
   Ensure the local web application is running, configure `appsettings.Local.json` in the test folder with your local URL, and run:
   ```bash
   dotnet test tests/AccessingChildcareEntitlementChecker.E2eTests --no-build
   ```
4. **Verify accessibility compliance:**
   Ensure the local web application is running, and run the automated accessibility tests:
   ```bash
   dotnet test tests/AccessingChildcareEntitlementChecker.A11yTests --no-build
   ```

## Step 3: Trigger the Pull Request and run automated CI validation

1. Push your branch to GitHub:
   ```bash
   git push -u origin hotfix/critical-issue-fix
   ```
2. Open a Pull Request (PR) targeted specifically at the active release branch (`releases/vX.Y`), **not** `main`.
3. Confirm that the PR triggers the automated validation pipeline.
4. Ensure all checks pass and obtain peer review and approval from at least one other team member.

## Step 4: Merge your Pull Request and deploy the fix

Deployments to our environments are fully event-driven and automated.

1. Merge your approved PR into the active release branch (`releases/vX.Y`).
2. Merging triggers the `Release Pipeline` (`workflow-release.yml`), which automatically executes the following:
   * **Provisioning:** Runs Terraform to apply configuration updates.
   * **Deployment:** Bundles the C# artefacts and deploys them to the **Staging** environment.
   * **Automated Hardening:** Runs automated Playwright E2E and A11y tests against Staging.
   * **Production Promotion:** Upon successful test completion on Staging, the pipeline automatically promotes and deploys the release package directly to **Production**.

## Step 5: Run post-deploy validation checks

Confirm the stability of the production environment.

1. Navigate to the production URL and check that the `/health` endpoint is responding with `200 OK`.
2. Perform a targeted manual sanity test of the affected user journey to confirm that the issue is successfully resolved.

## Step 6: Backport the hotfix to the trunk

Ensure that the bug fix is incorporated into standard development to prevent regression in the next general release.

1. Checkout the `main` branch locally and pull the latest changes:
   ```bash
   git checkout main
   git pull
   ```
2. Create a new branch off `main`:
   ```bash
   git checkout -b hotfix/backport-critical-fix
   ```
3. Cherry-pick the hotfix commit SHA:
   ```bash
   git cherry-pick <HOTFIX_COMMIT_SHA>
   ```
4. Resolve any merge conflicts, push the branch, and open a PR back into `main`.
