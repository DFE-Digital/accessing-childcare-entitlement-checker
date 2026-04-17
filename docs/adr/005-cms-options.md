# ADR 002: Content Management Strategy for Entitlement Checker

**Status:** Proposed
**Date:** 17 April 2026
**Author:** Senior Lead Architect

## Context and Problem Statement

The Accessing Childcare Entitlement (CEC) service must handle a complex, high-stakes eligibility journey (40+ pages). We need a mechanism to manage the journey flow, question content, and logic rules. We must decide between using a headless CMS, a purely hardcoded approach, or a hybrid "Middle Ground" solution to ensure delivery for Private Beta in August 2026.

## Decision Options

### Option 1: Headless CMS (Contentful)

*Logic and content are managed in an external SaaS platform and fetched via API.*

* **Note:** Currently utilised by the **Care Leavers** project within the same DfE portfolio, providing a proven precedent for procurement and security assurance.

### Option 2: Hardcoded (C# / Razor)

*Every page, question, and routing rule is a unique View/Controller action.*

### Option 3: "Middle Ground" (JSON-led Logic Engine)

*Logic is stored as structured JSON files within the Git repository; the application is a generic "Engine" that renders pages based on this data.*

## Scoring Matrix

*Scale: 1 (Poor) to 5 (Excellent). Weighted against DfE Digital Standards.*

| Criteria | Weight | Option 1: CMS | Option 2: Hardcoded | Option 3: JSON Engine |
| :--- | :--- | :--- | :--- | :--- |
| **Suitability for 40+ Pages** | 20% | 5 | 1 | 5 |
| **Flexibility (Policy Changes)** | 20% | 5 | 1 | 4 |
| **Development Velocity** | 15% | 4 | 2 | 5 |
| **Infrastructure Cost (SaaS)** | 10% | 3 | 5 | 5 |
| **Auditability (AZ-500)** | 15% | 5 | 2 | 5 |
| **Maintenance & DRY** | 10% | 5 | 1 | 5 |
| **Scaling & Resilience** | 10% | 5 | 5 | 5 |
| **Weighted Total** | **100%** | **4.6** | **2.0** | **4.8** |

## Comparative Analysis

### Option 1: Headless CMS (Contentful) - [Score: 4.6]

* **Pros:** Best-in-class for non-technical content management. It empowers Policy Leads to own the "Source of Truth."
* **Cons:** The primary hurdle is the **SaaS Onboarding Process**. However, this is significantly mitigated by the existing use of Contentful in the **Care Leavers** project, establishing a clear pathway for DfE security and legal approval.

### Option 2: Hardcoded (C# / Razor) - [Score: 2.0]

**This approach is strongly rejected based on the following critical failures:**

* **Violation of DRY Principle:** Requires 40+ near-identical Controllers and Views. This leads to "Copy-Paste" technical debt where logic is duplicated, increasing the likelihood of bugs.
* **Maintenance Nightmare:** A single global change (e.g., updating a common footer or a shared threshold) requires manual edits to 40+ separate files.
* **Poor Auditability (AZ-500):** Policy logic is buried deep within imperative C# code. There is no single "Source of Truth" for eligibility rules, making it impossible for non-technical stakeholders to verify accuracy.
* **Inflexible Routing:** Complex branching becomes a nested `if/else` mess that is difficult to unit test and prone to "Broken Link" errors.
* **Vendor Lock-in & Financial Risk:** This approach creates an artificial, permanent dependency on developers for even minor content changes. This results in poor "Value for Money" for the DfE and appears designed to sustain long-term contractor dependency rather than project efficiency.
* **Impact:** Choosing this would create a "Developer Bottleneck," making it statistically unlikely to meet the August 2026 deadline.

### Option 3: JSON-led Logic Engine - [Score: 4.8]

* **Pros:** The strongest technical choice for the POC. It provides the same architectural benefits as a CMS but with zero procurement delay and 100% Git-based auditability. It enforces strict separation of concerns.

## Delivery & Delivery Indications

| Metric | Option 1: CMS | Option 2: Hardcoded | Option 3: JSON Engine |
| :--- | :--- | :--- | :--- |
| **Dev Time Concentration** | Start (Integration) | End (Bug-fixing debt) | Middle (Engine build) |
| **Policy Change Speed** | Minutes (Sighted) | Days (Dev Sprint) | Hours (PR Cycle) |
| **August 2026 Confidence** | High | **Critically Low** | **Maximum** |

## AZ-500 & Compliance Mapping

1. **Supply Chain Security:** **Option 3** offers the highest security by keeping all logic within the DfE-controlled Git and Azure perimeter.
2. **Governance:** Option 1 and 3 both support **Transparent, Rule-Based Logic**, allowing policy leads to verify accuracy without reading code. Option 2 creates "Dark Logic" buried in binary files, which is a failure of modern governance.

## Recommendation

We **strongly reject Option 2** as it is functionally, financially, and architecturally unfit for a project of this complexity.

We recommend **Option 3** as the primary engine for the POC/Beta launch, with the architecture built against the **`ICmsFormService` interface**. This allows for a seamless "Hot-Swap" to **Option 1 (Contentful)** as soon as the onboarding process is finalised, ensuring the DfE benefits from a proven enterprise tool already in use by neighbouring teams.

*End of ADR*