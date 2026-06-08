---
title: Deploy an emergency fix
layout: page
showPagination: true
order: 2
sectionKey: Runbooks
eleventyNavigation:
  parent: Runbooks
---

This runbook covers the procedure for developing, testing, and deploying an emergency hotfix to resolve critical bugs or security vulnerabilities.

## Step 1: Isolate the Issue & Branch
1. Create a hotfix branch locally from the current stable tag or release branch (e.g., `release/vX.Y`), rather than the unreleased `main` branch.
   ```bash
   git checkout -b hotfix/critical-issue-fix release/v1.1
   ```

## Step 2: Implement and Validate Locally
1. Implement the required code correction within the C# projects.
2. Build the solution and run all local unit tests to ensure that the rules engine has not regressed:
   ```bash
   dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests
   ```
3. Run the Playwright end-to-end tests locally to verify that browser interactions and routing remain fully operational:
   ```bash
   # Ensure the web app is running in the background, then execute:
   dotnet test tests/AccessingChildcareEntitlementChecker.Tests.E2e
   ```

## Step 3: Trigger the Pull Request and Automated CI Gates
1. Push your branch to GitHub and open a Pull Request (PR) targeted at the current active release branch.
2. The pull request will trigger the automated validation pipeline (`workflow-pr.yml`).
3. Ensure all checks—including .NET compilation, NuGet package safety, unit tests, and E2E browser tests—pass successfully.
4. Have at least one team member peer-review and approve the code changes.

## Step 4: Execute the Deploy
1. Merge the PR. This action will trigger the `Deploy Environment` workflow to:
   - Run Terraform to apply any minor configuration patches.
   - Zip the newly compiled `.NET 10` artifact.
   - Execute an atomic `az webapp deploy` (ZipDeploy) to replace the running runtime on the Azure Linux Web App.
2. Verify the deployment on the target environment (e.g. `Staging`).
3. Once staging is approved, trigger the Production run.

## Step 5: Post-Deploy Verification
1. Access the production URL and check that the `/health` endpoint is green.
2. Perform a sanity test on the live user journey to verify the fix is active and correct.
