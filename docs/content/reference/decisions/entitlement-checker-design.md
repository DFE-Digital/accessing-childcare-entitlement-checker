---
title: Entitlement Checker design
layout: sub-navigation
order: 1
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions

---
## Summary

The proposal resulting from the Alpha phase is to build an Entitlements Checker web application hosted on GOV.UK. This checker improves upon existing disparate checkers by consolidating multiple entitlements into a single source of truth. The checker is designed to signpost users to appropriate onward services rather than formally confirming eligibility.

A stretch goal for the Beta phase is to evaluate the integration of financial calculator features, though this is outside the core MVP scope and does not alter fundamental requirements.

## Problem statement

The design and implementation of the entitlement checker must satisfy the following parameters:

* **Diverse question types:** Support for radio buttons, checkboxes, date of birth entries, numeric text inputs, and free-text inputs (for child names).
* **Complex logic branching:** Results display varies based on combinations of inputs (e.g., child age, household income, benefit status, and disability status).
* **Session-only storage:** No requirement to persist user data beyond the active session.
* **Public availability:** Accessible via a public GOV.UK domain.
* **Low policy volatility:** Eligibility rules change infrequently; updates are expected to involve either simple threshold adjustments or structural logic modifications.
* **KPI measurement:** Success metrics are based on simple usage statistics and surveyed effectiveness.

## Evaluated options

### Option 1: "Get to an Answer"
* Repo: https://github.com/DFE-Digital/get-to-an-answer
* Staging Admin: https://staging-admin.get-to-an-answer.education.gov.uk/

"Get to an Answer" is an existing DfE tool that displays answer pages based on question data. It includes a CMS for managing questions and answer text. Each instance is hosted on a unique URL and can be embedded within an iframe.

#### Pros

* Supports radio buttons, checkboxes, and custom answer text.
* Low maintenance overhead for standard content updates.

#### Cons

* Does not support complex, intersecting rules without creating a dedicated answer page for every possible permutation (resulting in hundreds of pages).
* Lacks support for date or numeric inputs.
* Does not support direct vanity URLs.

#### Evaluation

Updating this tool to support state management and complex rule processing would require a substantial rewrite. This would introduce migration overhead for existing users and increase system complexity.

### Option 2: Eligibility Checking Engine (ECE)
* Repo: https://github.com/DFE-Digital/eligibility-checking-engine (MCAS link removed/standardized)
* Swagger: https://eligibility-checking-engine.education.gov.uk/swagger/index.html

The ECE is an API used by local authorities to check HMRC codes for childcare eligibility and free school meals.

#### Pros

* Centrally manages scheme entitlement logic.

#### Cons

* Entitlements are distinct from formal eligibility checks.
* None of the targeted scheme entitlements are currently supported by the ECE API.
* Ownership of some targeted entitlements rests with other government departments (e.g., HMRC, DWP), making central integration complex and time-consuming.
* Requires exposing the API to public, unauthenticated calls.

### Option 3: Custom Build

![Technical Architecture](../adr/images/0001-Technical%20Architecture.png "Technical Architecture")

A custom .NET Core MVC web application, hosted in Azure and deployed via GitHub Actions, without a database or persistent user authentication.

#### Pros

* Aligns with established DfE development patterns.
* No complex external integrations required.
* Low maintenance overhead.
* Thresholds are managed via standard configuration.
* Automated testing is built into the deployment pipeline.
* Standard analytics (e.g., Google Analytics) satisfy MVP KPI requirements.

#### Cons

* Creates an additional application requiring long-term maintenance and ownership by DfE.

## Recommendation

Based on the low frequency of updates and the nature of the requirements, a custom web application (Option 3) is recommended.

## Consequences

* Refer to the delivery documentation.
* Development requires a two-developer team to implement the service.
* Standard Azure platform configurations do not require a specialized DevOps role.
* Extensive technical documentation is required to support the application and manage future logic updates.

## References

* Interactive Design board: https://lucid.app/lucidspark/b7452515-3d80-43d6-8031-cc5114122623/edit
