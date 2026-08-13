---
title: GitHub workflows
layout: sub-navigation
sectionKey: Reference
order: 8
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Reference
  key: GitHub workflows
---
This reference describes the automated integration and delivery pipelines configured within the repository's GitHub Actions environment.

These workflows enforce code quality, run accessibility and end-to-end tests, perform security scanning, and manage resource deployment.

| Workflow name | Trigger event | Purpose | Key secrets / parameters |
| :--- | :--- | :--- | :--- |
| `build-dotnet.yml` | Pull request, push to main | Compiles and runs unit and integration tests for the .NET application. | None (standard test environment) |
| `build-infra.yml` | Pull request, push to main | Validates and lints Terraform and Bicep infrastructure-as-code files. | None (static validation and linting) |
| `deploy-environment.yml` | Workflow dispatch, release | Deploys the infrastructure and web application to a specified environment. | `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID` |
| `run-e2e-tests.yml` | Workflow dispatch, schedule | Executes end-to-end user journey tests using Playwright. | `TEST_BASE_URL` |
| `run-a11y-tests.yml` | Workflow dispatch, schedule | Runs automated accessibility verification checks on application views. | `TEST_BASE_URL` |
| `workflow-main.yml` | Push to main branch | Triggers full build validation, static code analysis, and test suites. | `GITHUB_TOKEN` |
| `workflow-release.yml` | Release tag creation | Automates package compilation, asset release generation, and environment deployments. | `GITHUB_TOKEN`, `AZURE_CREDENTIALS` |
| `workflow-zap-scan.yml` | Schedule, workflow dispatch | Performs OWASP Zed Attack Proxy (ZAP) security vulnerability scanning. | `ZAP_TARGET_URL` |
