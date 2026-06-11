---
title: Workflow Naming Conventions
layout: sub-navigation
sectionKey: Developers
order: 9
includeInBreadcrumbs: true
eleventyNavigation:
  key: Workflow Naming
  parent: Developers
---
This project follows a strict naming convention for GitHub Actions workflows to ensure clarity, consistency, and efficient reuse of logic.

## Summary

| Workflow Type | File Pattern     | Trigger           | Responsibility |
|:--------------|:-----------------|:------------------|:---------------|
| Main          | `workflow-*.yml` | Events (Push, PR) | Orchestration  |
| Reusable      | `<verb>-*.yml`   | `workflow_call`   | Implementation |

## Workflow Categories

Workflows are divided into two primary categories based on their purpose and how they are triggered.

### Main Workflows (`workflow-*`)

Main workflows are the top-level orchestrators. They are triggered by GitHub events and typically coordinate multiple reusable workflows to form a complete pipeline.

- File Name Pattern: `workflow-<description>.yml`
- Trigger: Triggered by `push`, `pull_request`, `schedule`, or `workflow_dispatch`.
- Purpose: To define the "what" and "when" of a process (e.g., "On every PR, build and test the code").
- Examples:
    - `workflow-main.yml`: Orchestrates the full CI/CD pipeline for the main branch.
    - `workflow-pr.yml`: Orchestrates validation checks for pull requests.
    - `workflow-zap-scan.yml`: Orchestrates security scanning.

### Reusable Workflows (`<verb>-*`)

Reusable workflows encapsulate specific, repeatable tasks. They are designed to be called by Main workflows or other reusable workflows.

- File Name Pattern: `<verb>-<description>.yml`
- Trigger: Must use `on: workflow_call`.
- Purpose: To define the "how" of a specific task (e.g., "How to build a .NET application").

These should follow the verb-description naming pattern. This makes it immediately clear what action the workflow performs.

| Verb     | Usage                                                                                 | Examples                                  |
|:---------|:--------------------------------------------------------------------------------------|:------------------------------------------|
| `build`  | Tasks related to compilation, unit testing, linting, and artifact creation.           | `build-dotnet.yml`, `build-terraform.yml` |
| `deploy` | Tasks related to infrastructure provisioning and application deployment.              | `deploy-environment.yml`                  |
| `run`    | Tasks that execute functional tests or external tools against a deployed environment. | `run-e2e-tests.yml`                       |

