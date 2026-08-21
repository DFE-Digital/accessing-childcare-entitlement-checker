---
title: Use release branches with trunk-based development for staging and production releases
layout: sub-navigation
order: 9
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions

---
**Date:** 2026-05-25  
**Decision Makers:** Engineering & Product  
**Technical Story:** This decision supports reliable production releases and simplified hotfix workflows. It retains trunk-based development practices.

## Context

The current development model follows a trunk-based approach:

* Developers integrate changes frequently into `main`.
* The CI/CD pipeline continuously validates the `main` branch.
* The CI/CD pipeline performs the following tasks:
    * Builds application artefacts
    * Executes automated tests
    * Executes end-to-end (E2E) tests
    * Executes accessibility (a11y) checks
    * Deploys application updates automatically to development and test environments.

This model supports:

* Fast feedback loops
* High integration frequency
* Fewer merge conflicts
* Continuous delivery readiness

However, several challenges arise when promoting changes to staging and production environments:

1. The `main` branch continues to evolve during release validation.
2. Production issues may require urgent hotfixes independent of ongoing development.
3. Releasing directly from `main` increases risk. Unrelated in-progress changes might enter production.
4. Stabilising and patching a production release is difficult. Uninterrupted development on `main` makes this harder.

### Required capabilities

* Stable release candidates.
* Controlled promotion to staging and production.
* Ability to apply hotfixes cleanly.
* Minimal disruption to trunk-based workflows.

## Decision

Trunk-based development remains the standard. The `main` branch is the primary integration branch. The project adds release branches for staging and production deployments.

## Branching model

### Trunk branch

* The `main` branch remains the single integration branch.
* Developers merge feature work continuously.
* CI/CD automatically validates and deploys to development and test environments.

### Release branches

When preparing a release:

* The team creates a release branch from `main`.
* Naming convention:

```text
releases/vX.Y
```

The release branch functions as:

* The stabilisation branch for the release.
* The deployment source for staging and production.

The team allows only the following changes on a release branch:

* Bug fixes.
* Release-critical configuration changes.
* Documentation and version updates.
* Approved hotfixes.

Developers cannot add new features after creating the branch.

## Deployment flow

### Development lifecycle

```text
Feature Branches
  ↓
main
  ↓
CI/CD Validation
(Build + Unit + E2E + A11y)
  ↓
Deploy to Development & Test
```

### Release lifecycle

```text
main
  ↓
Create release branch
  ↓
CI/CD Validation
(Build + Unit)
  ↓
Deploy to Staging
  ↓
E2E / A11y / UAT / Regression / Signoff
  ↓
Deploy to Production
```

## Hotfix process

In the event of a production issue:

1. Create a hotfix commit directly against the active release branch.
2. Validate through CI/CD.
3. Deploy the hotfix to staging and production.
4. Cherry-pick the hotfix back into `main`.

This workflow ensures:

* Production stability.
* Continued development on `main`.
* Prevention of fix omission between branches.

## Rationale

This approach balances the integration frequency of trunk-based development with the stability required for production operations.

Release branches provide:

* A stable snapshot of a releasable version.
* Isolation from ongoing development.
* Simplified operational support.
* Controlled production promotion.
* Systematic hotfix management.

This model avoids:

* Long-lived environment branches.
* Complex GitFlow-style branching.
* High merge management overhead.

The project preserves the core principles of trunk-based development. Lightweight release isolation provides stability.

## Alternatives considered

### Deploy directly from `main`

* **Positive:** Simplest workflow, pure trunk-based development, and minimal branching.
* **Negative:** Releases are difficult to stabilise. Hotfixes carry risk. Ongoing development may block releases.
* **Outcome:** The team rejected this option because of operational risk and hotfix complexity.

### Gitflow

* **Positive:** Explicit release and hotfix workflows using a widely known model.
* **Negative:** This model features long-lived branches, high merge complexity, and slow integration. It conflicts with trunk-based principles.
* **Outcome:** The team rejected this option because of unnecessary process overhead.

### Environment branches (`Develop`, `staging`, `production`)

* **Positive:** Clear mapping to physical deployment environments.
* **Negative:** Branches drift. Merge conflicts occur frequently. Traceability is difficult. Manual synchronisation creates overhead.
* **Outcome:** The team rejected this option because of maintenance complexity and drift risk.

## Consequences

* **Positive:** Safer production releases.
* **Positive:** Controlled hotfix handling.
* **Positive:** Stable staging validation.
* **Positive:** Continued rapid integration on `main`.
* **Positive:** Reduced risk during release testing.
* **Positive:** Clear release audit trail.
* **Negative:** Additional branch management.
* **Negative:** Temporary divergence between `main` and the release branch.
* **Negative:** Requirement to back-merge hotfixes.
* **Negative:** Increased CI/CD pipeline complexity.
