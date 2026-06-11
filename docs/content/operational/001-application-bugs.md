---
title: Application bugs
layout: page
showPagination: true
order: 1
sectionKey: Operational
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
A defect, logic error, or configuration issue is deployed to production and impacts parents/carers trying to evaluate their childcare entitlements.

## Impact

Loss of functionality or incorrect eligibility calculations (e.g., miscalculating entitlement hours or tax-free criteria), resulting in frustrated users or misleading eligibility guidance.

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

The quickest response to recover service availability is:

- Roll back the change: Re-deploy the last known stable package zip via GitHub Actions.
- Roll forward with a fix: Commit a bug fix, compile, validate on Staging, and release.

## Recovery

Restore normal operation by executing a rollback or hotfix, then verify correctness with the Playwright test suite.

## Related runbooks

- [Roll back a deployment](/runbooks/001-rollback-deployment/)
- [Deploy an emergency fix](/runbooks/002-deploy-emergency-fix/)
