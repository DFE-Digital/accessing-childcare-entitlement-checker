---
title: Use GitHub runners for UI tests on PR push
layout: sub-navigation
order: 4
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions

---
## Context and problem statement

The CI pipeline must run UI tests on pull request (PR) pushes. This process avoids the high cost of full ephemeral environments in Azure.

This decision allows other testing methods. For example, the pipeline can run tests against Azure deployments after merging code to the `main` branch.

## Decision drivers

* The pipeline executes tests close to the code changes.
* Minimisation of operational costs. GitHub Actions runners are free for public repositories.
* Simplifying deployment and infrastructure.
* Verifying the application without external database dependencies.

## Considered options

* Disabling UI tests on PR pushes.
* Deploying an ephemeral Azure environment for every PR push.
* Running tests directly on the GitHub runner.

## Decision outcome

Chosen option: **Running tests directly on the GitHub runner**. This approach provides a simple, cost-effective solution within continuous integration (CI).

### Consequences

* **Positive:** The CI pipeline executes tests immediately after code changes.
* **Positive:** This approach simplifies implementation and management compared to Azure deployments.
* **Positive:** This approach reduces operational costs compared to Azure deployments.
* **Neutral:** The runner environment differs from production. However, the team performs post-merge testing on the `main` branch.
* **Negative:** Future database dependencies may require a transition to Azure-based test deployments.
