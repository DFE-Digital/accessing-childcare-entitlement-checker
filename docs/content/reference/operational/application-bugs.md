---
title: Application bugs
layout: sub-navigation
order: 1
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
A defect, logic error, or configuration issue is deployed to production and impacts users evaluating childcare entitlements.

## Impact

Loss of functionality or incorrect eligibility calculations (e.g., miscalculating entitlement hours or tax-free criteria), resulting in misleading eligibility guidance or service disruption.

## Prevention

- Rigorous Test Automation:
  - In-process unit testing of logic across multiple edge cases.
  - Automated browser-level integration testing using Playwright to verify multi-step user journey state, cookie handling, and error screens.
- CI/CD Quality Gates: GitHub Actions pipelines compile code, restore package locks, and execute all unit and Playwright test suites before allowing pulls to `main`.
- Peer Code Reviews: Required peer reviews enforced via GitHub branch protection and `CODEOWNERS`.
- Production-like Environments: Testing deployments on Dev, Test, and Staging environments using Terraform-managed infrastructure identical to Production.

## Detection

- Automated Smoke Tests: Post-deploy pipeline validation checks.
- Application Insights Diagnostics: Querying custom exceptions and 5xx HTTP response codes in Azure Log Analytics Workspace.
- User Feedback & Reports: Feedback submitted by users when hitting error paths.

## Response

Response options to restore service availability include:

- Rollback: Redeployment of the last known stable package zip via GitHub Actions.
- Roll-forward: Commit of a bug fix, compilation, validation on Staging, and release.

## Recovery

Normal operations are restored by executing a rollback or hotfix, followed by verification using the Playwright test suite.

## Related runbooks

- [Roll back a deployment](/how-to/runbooks/rollback-deployment/)
- [Deploy an emergency fix](/how-to/runbooks/deploy-emergency-fix/)
