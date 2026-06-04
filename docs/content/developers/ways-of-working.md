---
title: Ways of Working
layout: sub-navigation
sectionKey: Developers
eleventyNavigation:
  parent: Developers
  key: Ways of Working
order: 1
---

This document defines the development standards and operational workflows for the project.

## Development Standards

### Technology Stack
* Runtime: .NET 10.0
* Web Framework: ASP.NET Core MVC
* Testing: xUnit, NSubstitute, Reqnroll (Gherkin), Playwright
* Infrastructure: Terraform, Azure

### Code Quality
* Static Analysis: .NET Analyzers are enabled and enforced on build (`EnforceCodeStyleInBuild`).
* Formatting: Follow standard C# coding conventions and GDS design patterns for the frontend.
* Architecture: Adhere to the [Application Architecture](../architecture/application-architecture.md). Maintain a strict separation between the stateless `RulesEngine` and the stateful `Web` application.

## Branching and Commits

We follow a Trunk-based development model as detailed in the [Branching Strategy](branching-strategy.md).

### Branch Naming
* Feature Branches: `feature/description`
* Bug Fixes: `fix/description`
* Documentation: `docs/description`
* Releases: `release/vX.Y`

### Commit Messages
Commits should be atomic and descriptive. While not a strictly enforced standard, we recommend following a structured format such as `<type>: <description>` to help maintain a clear project history:

* `feat`: A new feature
* `fix`: A bug fix
* `docs`: Documentation only changes
* `style`: Changes that do not affect the meaning of the code (white-space, formatting, etc)
* `refactor`: A code change that neither fixes a bug nor adds a feature
* `test`: Adding missing tests or correcting existing tests
* `chore`: Changes to the build process or auxiliary tools and libraries

Example: `feat: add logic for 15 hours universal entitlement`

## Pull Requests

All changes to `main` and `release/*` branches must be made via Pull Requests.

### Requirements
1. Pass CI: All builds, unit tests, and component tests must pass.
2. Review: At least one approval from a maintainer is required.
3. Tests: New features must include unit tests and, where applicable, Reqnroll/Playwright E2E tests covering the Acceptance Criteria (AC).
4. Documentation: Update relevant documents in `/docs` if the change impacts architecture or workflows.

### Process
1. Open a PR against `main`.
2. Ensure the PR description links to any relevant issues or Jira tickets.
3. Once approved and CI passes, the author is responsible for merging (**Squash and Merge** is required to maintain a linear history).

## Testing Strategy

Quality is verified through a 9-layer pyramid as defined in the [Test Strategy](../testing/test-strategy.md).

* Unit & Component Tests: Run on every PR.
* E2E Tests: Run on every PR (currently Chromium only).
* Accessibility: In progress - being integrated into E2E suites.
* Security (DAST): OWASP ZAP scans run weekly against the Test environment.
* Infrastructure: `checkov` validates Terraform changes on PR.

## Deployment Workflow

### Continuous Integration (CI)
Every push to a PR triggers the `ci.yml` workflow, which handles:
* Building the solution
* Running Unit and Component tests
* Static analysis checks

### Continuous Delivery (CD)
* Development/Test Environments: Merges to `main` trigger automatic deployment to the Dev and Test environments.
* Staging Environment: Deployed from `release/*` branches for final UAT and A11y/E2E validation.
* Production: Controlled release from a stabilized `release/*` branch.

## Documentation

Documentation is treated as first-class code:
* Use Markdown for text and [Mermaid](https://mermaid.js.org/) for diagrams.
* Store architectural decisions in `docs/adr/`.
* Keep the [README](../../../README.md) and [Ways of Working](ways-of-working.md) updated as the project evolves.
 updated as the project evolves.
