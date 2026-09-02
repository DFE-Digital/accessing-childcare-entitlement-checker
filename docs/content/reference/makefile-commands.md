---
title: Makefile commands
layout: sub-navigation
sectionKey: Reference
order: 9
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Reference
  key: Makefile commands
---
This reference describes the automation commands configured within the repository's `Makefile`.

These commands run development pipelines, verify code format, execute testing targets, manage Terraform configurations, and manage documentation tasks.

## Build and verification commands

| Command | Action description |
| :--- | :--- |
| `make build` | Cleans build outputs, restores NuGet packages in locked mode, verifies formatting, and builds the solution. |
| `make verify` | Executes the complete local verification process. This includes building, running all static analysis, and running tests. |

## Static analysis commands

| Command | Action description |
| :--- | :--- |
| `make inspect-a` | Executes deep InspectCode analysis using JetBrains global tools. Outputs SARIF results to the analysis results folder. |
| `make inspect-r` | Displays a summarized hierarchical list of InspectCode findings grouped by severity level and rule ID with counts. |
| `make inspect-f rule=<RuleId>` | Displays all individual occurrences of a specific rule ID (e.g., `make inspect-f rule=InconsistentNaming`). |
| `make inspect` | Runs both InspectCode analysis and displays the summarized findings (`inspect-a` and `inspect-r`). |

## Testing commands

| Command | Action description |
| :--- | :--- |
| `make test` | Executes all local unit, component, and integration tests, then outputs results to the test results directory. |
| `make test-e2e` | Executes end-to-end browser user journey test scenarios using Playwright. Requires the web application to run first. |
| `make test-a11y` | Executes accessibility tests using axe-core to verify WCAG compliance. Requires the web application to run first. |
| `make playwright-i` | Installs Playwright browser binaries and system dependencies. |

## Terraform infrastructure commands

| Command | Action description |
| :--- | :--- |
| `make tf-i` | Initialises Terraform configuration locally without a backend. |
| `make tf-f` | Spacially cleans and recursively formats all Terraform configuration files in the `infra/` folder. |
| `make tf-v` | Validates Terraform configuration syntax, rules, and resource parameters. |
| `make tf-d` | Generates architectural documentation from Terraform variables and injects it into the deployed infrastructure guide. |
| `make tf` | Executes all local Terraform checks (`tf-i`, `tf-f`, `tf-v`, and `tf-d`). |

## Documentation commands

| Command | Action description |
| :--- | :--- |
| `make docs-c` | Cleans the generated Eleventy static site output directory. |
| `make docs-b` | Installs documentation dependencies and builds the static site locally. |
| `make docs-s` | Starts a local Eleventy development preview server for reviewing documentation changes. |
| `make docs-n` | Flattens markdown documentation files by directory into consolidated notebooks for LLM ingestion. |
