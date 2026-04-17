---
id: ADR 0005- Refactoring Options for Entitlement Checker Journey Logic
title: Refactoring Options for Entitlement Checker Journey Logic
status: Under Review
date: 2026-04-17
author: Hemant Khanduja
consulted: PM, Technical Team, TA from Hippo and DfE
---

# ADR 0005: Refactoring Options for Entitlement Checker Journey Logic

## Status
Under Review (Pending internal technical review with Hippo and then with DfE leads)

## Context
The Accessing Childcare Entitlement (CEC) service requires a sustainable architecture to manage an extensive 40+ page eligibility journey. We must ensure the August 2026 Private Beta deadline is met while maintaining high accuracy, security, and "Value for Money" for the DfE. 

The current path relies on hardcoding each page as a unique controller and view. While the development team has recently introduced "Categories" within the solution explorer to organise these files, this refactoring is primarily navigational. It makes the solution a little bit easier to navigate for developers, but it does not reduce the actual volume of code to be maintained, nor does it address the lack of policy transparency or authoring capability for non-technical stakeholders.

## New Insights & Drivers for Change

1. **Input Pattern Predictability:** Audit of around 40+ planned pages as per **Project Design Overview:** ![User Flow Diagram](../images/Round%203%20beta-%20planning-pages.png) on the **[Round 3 beta- planning: Lucidspark](https://lucid.app/lucidspark/510fe235-2efe-42af-8dc4-6b46c0aa5a83/edit?invitationId=inv_4fcca63f-2f2b-437e-b228-890279248a44&page=0_0#)** board reveals an ultra-consistent structure. Roughly around 25 pages are standard GDS Radio Buttons, 5–10 are Checkboxes, and the remainder are Text Boxes. Hardcoding 40 unique C# views for identical input types is an inefficient use of DfE resource and a fundamental violation of **DRY (Don't Repeat Yourself)** principles.

2. **Scaling for Public Beta Requirements:** Significant future requirements have been "parked" for the Public Beta phase that require a global configuration approach:
   * **Analytics & Insights:** Integration of Google Analytics (GA), Google Tag Manager (GTM), and Microsoft Clarity across all 40+ screens.
   * **External Dependencies:** Potential for No. 10 department-led policy pivots and external data dependencies.
   * **Financial Tooling:** A dedicated Financial Calculator in Phase 2.
   Option 2 would require manual, repetitive updates across 40+ separate controllers to implement these, whereas Options 1 and 3 allow for global injection.
   * **Rule Engine Parallelism:** We are at the initial stage of defining the Rule Engine architecture. This engine will be the primary driver for result page generation. As the Rule Engine is still being evaluated, we cannot delay the development of the journey logic until the end of Public Beta. Implementing Option 1 or 3 provides a data-driven interface that can consume Rule Engine outputs far more efficiently than a hardcoded controller-based architecture.



3. **Authoring & Content Ownership:** * **Option 1 (Contentful):** Provides a native interface for Policy Leads and Editors to author content directly without developer intervention.
   * **Option 3 (JSON):** Centralises content but remains "technical" to author, requiring a developer or technical editor to manage the JSON schema.
   * **Option 2 (Hardcoded):** Makes it **almost impossible** for editors to author content, as every change requires a code-level modification and a full deployment cycle.

4. **CMS Migration Strategy (The "Stepping Stone"):** Internal discussions show a desire for Contentful. Migrating from a structured JSON model (Option 3) to a CMS is a straightforward mapping exercise. Conversely, migrating from 40+ hardcoded controllers (Option 2) would require a manual, high-cost rewrite. Option 3 acts as the necessary "bridge" to Robert’s preferred CMS end-state.

## Options Considered

### Option 1: Headless CMS (Contentful)
* **Description:** SaaS platform for managing journey logic and content.
* **Pros:** Best-in-class for non-technical ownership; established in **Care Leavers** project; removes developer dependency for live content changes.
* **Cons:** Procurement/onboarding delays may risk the immediate Private Beta timeline.

### Option 2: Hardcoded Logic (Baseline/Current Path)
* **Description:** Continuing with "One Controller Per Page," using solution explorer categories for navigation.
* **Cons:**
    * **Violation of DRY Principle:** Requires 40+ near-identical Controllers. 
    * **Maintenance Nightmare:** A single global change (e.g., GDS footer or GA tag) requires manual edits to 40+ separate files.
    * **Authoring Failure:** Content ownership is locked within the dev team; editors cannot perform their roles.
    * **Dead-End Architecture:** Extremely difficult and manual to migrate to a CMS or integrate global analytics in the future.
    * **Incompatibility with Future Rule Engine:** Hardcoded logic is resistant to dynamic integration with an flexible/generic Rule Engine

### Option 3: Logic Engine Refactor (JSON-led)
* **Description:** Abstracting the journey into a generic "Engine" driven by a centralised JSON schema in Git.
* **Pros:** Zero licensing cost; strictly adheres to DRY principles; acts as a functional prototype and **data-mapping bridge** for an eventual jump to Contentful.
* **Cons:** Requires technical knowledge for initial JSON mapping; harder for non-technical editors to author than Option 1.

## Scoring Matrix
*Scale: 1 (Poor) to 5 (Excellent). 

| Criteria | Weight | Option 1: CMS | Option 2: Hardcoded | Option 3: JSON Engine |
| :--- | :--- | :--- | :--- | :--- |
| Suitability for 40+ Pages | 15% | 5 | 1 | 5 |
| Flexibility (GA/GTM/Clarity) | 15% | 5 | 1 | 4 |
| **Ease of Migration to CMS** | 20% | 5 | **1** | **4** |
| Authoring/Content Ownership | 10% | 5 | **1** | 3 |
| Development Velocity | 10% | 4 | 2 | 5 |
| Infrastructure Cost (SaaS) | 10% | 3 | 5 | 5 |
| Auditability (Governance) | 10% | 5 | 2 | 5 |
| Maintenance & DRY | 10% | 5 | 1 | 5 |
| **Weighted Total** | **100%** | **4.6** | **1.8** | **4.4** |

## Discussion Points for Internal Review
* **The "Migration Bridge":** Does the team agree that Option 3 acts as a functional prototype for Option 1, making the eventual jump to Contentful easier?
* **Resource Allocation:** Can we justify the "Sprint 0" effort to build the Engine to ensure we can easily integrate GA/GTM and satisfy No. 10 requirements later?
* **Developer Dependency:** How do we mitigate the risk of "Vendor Lock-in" created by the current hardcoded approach, ensuring the DfE can manage content independently in the future?

---
### Reference Documentation
* **Project Design Overview:** ![User Flow Diagram](../images/Round%203%20beta-%20planning-pages.png)
* **Journey Map:** [Round 3 beta- planning: Lucidspark](https://lucid.app/lucidspark/510fe235-2efe-42af-8dc4-6b46c0aa5a83/edit?invitationId=inv_4fcca63f-2f2b-437e-b228-890279248a44&page=0_0#)

