---
title: Use Release Branches with Trunk-Based Development for Staging and Production Releases
layout: page
showPagination: true
order: 9
sectionKey: Decisions
eleventyNavigation:
  parent: Decisions

---

**Date:** 2026-05-25  
**Decision Makers:** Engineering & Product  
**Technical Story:** Support reliable production releases and simplified hotfix workflows while retaining trunk-based development practices.

## Context

Engineering currently follows a trunk-based development model:

* Developers integrate changes frequently into `main`
* `main` is continuously validated through CI/CD
* CI/CD pipeline:

    * Builds application artifacts
    * Executes automated tests
    * Executes end-to-end (E2E) tests
    * Executes accessibility (a11y) checks
    * Automatically deploys to:

        * Development environment
        * Test environment

This model enables:

* Fast feedback loops
* High integration frequency
* Reduced merge conflicts
* Continuous delivery readiness

However, challenges arise when promoting to staging and production environments:

1. `main` continues to evolve while a release is being validated.
2. Production issues may require urgent hotfixes independent of ongoing development.
3. Releasing directly from `main` increases risk because unrelated in-progress changes may be included.
4. It is difficult to stabilise and patch a production release while allowing development to continue uninterrupted.

The team requires:

* Stable release candidates
* Controlled promotion to staging and production
* Ability to apply hotfixes cleanly
* Minimal disruption to trunk-based workflows

## Decision

The team will continue using trunk-based development with `main` as the primary integration branch, but will introduce release branches for staging and production deployments.

## Branching Model

### Trunk Branch

* `main` remains the single integration branch
* Developers merge feature work continuously
* CI/CD automatically validates and deploys to:

    * Development
    * Test

### Release Branches

When preparing a release:

* A release branch is created from `main`
* Naming convention:

```text
release/vX.Y
```

The release branch becomes:

* The stabilisation branch for the release
* The deployment source for staging and production

Only the following changes are permitted on a release branch:

* Bug fixes
* Release-critical configuration changes
* Documentation/version updates
* Approved hotfixes

No new features may be added after branch creation.

## Deployment Flow

### Development Lifecycle

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

### Release Lifecycle

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

## Hotfix Process

If a production issue occurs:

1. Create hotfix commit directly against the active release branch
2. Validate through CI/CD
3. Deploy hotfix to staging/production
4. Cherry-pick hotfix back into `main`

This ensures:

* Production stability
* Continued development on `main`
* No loss of fixes between branches

## Rationale

This approach balances:

* Speed of trunk-based development
* Stability required for production operations

Release branches provide:

* A stable snapshot of a releasable version
* Isolation from ongoing development
* Simpler operational support
* Safer production promotion
* Easier hotfix management

The model avoids:

* Long-lived environment branches
* Complex GitFlow-style branching
* Heavy merge management overhead

The approach preserves the core principles of trunk-based development while introducing lightweight release isolation.

## Alternatives Considered

### Deploy Directly from `main`

**Pros:**

* Simplest workflow
* Pure trunk-based development
* Minimal branching

**Cons:**

* Difficult to stabilize releases
* Hotfixes become risky
* Ongoing development may block releases
* Increased production deployment risk

**Outcome:**

Rejected due to operational risk and poor hotfix ergonomics.

### GitFlow

**Pros:**

* Explicit release and hotfix workflows
* Well-known model

**Cons:**

* Long-lived branches
* Higher merge complexity
* Slower integration cycles
* Conflicts with trunk-based principles

**Outcome:**

Rejected because it introduces unnecessary process overhead.

## Environment Branches (`develop`, `staging`, `production`)

**Pros:**

* Clear deployment mapping

**Cons:**

* Branch drift
* Frequent merge conflicts
* Difficult traceability
* Manual synchronization burden

**Outcome:**

Rejected due to maintenance complexity and drift risk.

## Consequences

**Positive:**

* Safer production releases
* Easier hotfix handling
* Stable staging validation
* Continued fast integration on `main`
* Reduced risk during release testing
* Clear release audit trail

**Negative:**

* Additional branch management
* Temporary divergence between `main` and release branch
* Requirement to back-merge hotfixes
* Slightly more complex CI/CD pipelines
