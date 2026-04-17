---
id: ADR 0005
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

The current path relies on hardcoding each page as a unique controller and view. While the development team has recently introduced "Categories" within the solution explorer to organise these files, this refactoring is primarily navigational. It makes the solution easier to navigate for developers, but we are evaluating if this approach provides the necessary agility for global changes, policy transparency, or non-technical authoring as the service scales.

## New Insights & Drivers for Change

1. **Input Pattern Predictability:** Audit of around 40+ planned pages—as seen in the **[Project Design Overview](../images/Round%203%20beta-%20planning-pages.png)** and the **[Round 3 beta- planning: Lucidspark](https://lucid.app/lucidspark/510fe235-2efe-42af-8dc4-6b46c0aa5a83/edit?invitationId=inv_4fcca63f-2f2b-437e-b228-890279248a44&page=0_0#)** board—reveals an ultra-consistent structure. Approximately 25 pages utilise standard GDS Radio Buttons, 5–10 use Checkboxes, and the remainder use Text Boxes. Abstracting these into repeatable patterns is being evaluated against the **DRY (Don't Repeat Yourself)** principle.

2. **Scaling for Public Beta Requirements:** Significant future requirements have been identified for the Public Beta phase that benefit from a centralised configuration approach:
    * **Analytics & Insights:** Integration of Google Analytics (GA), Google Tag Manager (GTM), and Microsoft Clarity across all 40+ screens.
    * **External Dependencies:** Potential for No. 10 department-led policy pivots and external data dependencies.
    * **Financial Tooling:** A dedicated Financial Calculator planned for Phase 2.
    Option 2 would require manual, repetitive updates across 40+ separate controllers (potentially inside both Get and Post methods) to implement these, whereas Options 1 and 3 allow for global injection.
   * **Rule Engine Parallelism:** We are at the initial stage of defining the Rule Engine architecture. Implementing Option 1 or 3 provides a data-driven interface that can consume Rule Engine outputs far more efficiently than a bespoke controller-based architecture.
   * **Separation of Concerns (SoC):** Following DfE best practices, we aim to separate "Presentation Logic" (C# code) from "Domain Content" (Question text/Logic). Options 1 and 3 allow the C# code to remain content-agnostic, focusing solely on rendering, while the journey logic resides in a governed data file or CMS.
   * **URL Agility & SEO:** Public Beta testing often requires refining URL slugs for user clarity and SEO. Option 2 requires static route definitions for every controller, whereas Options 1 and 3 support dynamic routing based on data-driven slugs, allowing for instant SEO updates.
   * **Accessibility & WCAG Compliance:** DfE services must meet WCAG 2.2 standards. Options 1 and 3 allow for "Centralised Accessibility Fixes," where a correction to an aria-label or fieldset legend in a single component template propagates across all 40+ pages. Option 2 introduces a risk of inconsistent compliance, as fixes must be manually replicated across 40+ views.
3. **Authoring & Content Ownership:** 

    * **Option 1 (Contentful):** Provides a native interface for Policy Leads and Editors to author content directly without developer intervention.
    * **Option 3 (JSON):** Centralises content but remains technical to author, requiring a developer or technical editor to manage the JSON schema.
    * **Option 2 (Hardcoded):** Content ownership is technically bound to the codebase; changes require code modification and a full deployment cycle.

4. **CMS Migration Strategy (The "Stepping Stone"):** DfE portfolios frequently utilise Headless CMS solutions (e.g., Contentful) for services like 'Care Leavers'. Migrating from a structured JSON model (Option 3) to a CMS is a straightforward mapping exercise. Conversely, migrating from 40+ hardcoded controllers (Option 2) would necessitate a manual extraction of logic. Option 3 acts as a technical "bridge" to an eventual CMS end-state.

## Options Considered

### Option 1: Headless CMS (Contentful)
* **Description:** SaaS platform for managing journey logic and content.
* **Pros:** Best-in-class for non-technical ownership; removes developer dependency for content updates.
* **Cons:** :
  * **Onboarding:** Procurement and onboarding lead times may impact the immediate Private Beta timeline.
  * **Compile-time Safety:** CMS content is fetched at runtime via API.
    * **Mitigation:** Can be offset by using strongly typed C# clients and code generation tools to map CMS schemas to C# classes, ensuring type integrity at build-time.

### Option 2: Hardcoded Logic (Baseline Strategy)
* **Description:** Continuing with "One Controller Per Page," using solution explorer categories for navigation.
* **Pros:** 
    * **Compile-time Safety:** Utilises the C# compiler to catch type and logic errors at build time.
    * **Simplicity:** High familiarity for the current team with no additional abstraction layers.
* **Cons:**
    * **Extensive Maintenance Surface Area:** Global changes (e.g., analytics tags or GDS updates) require manual edits to 40+ separate files.
    * **Limited Authoring Capability:** Content management is technically coupled to the engineering team.
    * **Mixing of Concerns:** Policy logic, UI text, and routing are tightly coupled within the C# controllers. This increases the "Blast Radius" of simple content changes, as any edit requires touching the core application code.
  * **Rigid Routing:** URL structures and SEO metadata are "hard-wired" to controllers, making route changes a deployment-dependent task.
   * **Compliance Risk:** Maintaining high accessibility standards across 40+ unique files increases the likelihood of "Human Error," where one screen might miss a critical compliance update.
    
    * **Migration Complexity:** Manual effort required to move logic to a CMS or integrate a dynamic Rule Engine in the future.

### Option 3: Logic Engine Refactor (JSON-led)
* **Description:** Abstracting the journey into a generic "Engine" driven by a centralised JSON schema.
* **Pros:** 
   * **Zero licensing cost**; adheres to DRY principles; acts as a data-mapping bridge for a future CMS transition 
   * supoorts **Dynamic Routing** where URL slugs are governed by the JSON schema rather than static C# attributes.
   * and ensures **Systemic Accessibility**, where compliance is enforced through shared, audited templates rather than repetitive bespoke code.
   * and enforces **Separation of Concerns**, making the codebase easier to test and maintain.
* **Cons:** 

    * **Loss of Compile-time Validation:** Journey logic errors are caught at runtime. 
    * **Mitigation:** Can be offset by using JSON Schema validation and automated unit tests to "walk" the journey during the CI build to ensure link integrity.

## Scoring Matrix
*Scale: 1 (Poor) to 5 (Excellent).*

| Criteria | Weight | Option 1: CMS | Option 2: Baseline | Option 3: JSON Engine |
| :--- | :--- | :--- | :--- | :--- |
| **Initial Simplicity** | 10% | 2 | **5** | 3 |
| **Compile-time Safety** | 10% | **3** | **5** | **3** |
| **Suitability for 40+ Pages** | 15% | 5 | 2 | 5 |
| **Global Flexibility (GA/GTM)** | 15% | 5 | 2 | 4 |
| **Ease of CMS Migration** | 20% | 5 | 1 | 4 |
| **Authoring Independence** | 10% | 5 | 1 | 3 |
| **Development Velocity** | 10% | 4 | 3 | 4 |
| **Maintenance Sustainability** | 10% | 5 | 2 | 5 |
| **Weighted Total** | **100%** | **4.4** | **2.5** | **4.1** |

## Discussion Points for Internal Review
* **The "Migration Bridge":** Does the team agree that Option 3 simplifies the future move to a CMS compared to the current manual approach?
* **Resource Allocation:** Can we justify the effort to build the Engine to ensure we can easily integrate global analytics, manage dynamic SEO routes, and satisfy upcoming Public Beta requirements?
* **Future-Proofing:** How do we ensure the journey architecture we choose today is compatible with the upcoming Rule Engine definitions?
* **WCAG Maintenance:** Does the team agree that a centralized template approach (Option 3) reduces the risk of accessibility regressions compared to managing 40+ separate Razor views?
* **Architectural Cleanliness:** How much value does the team place on decoupling our content from our code to ensure we meet GDS standards for long-term service sustainability?
* **POC:** Does it make sense to see the POC I did earlier?

---
### Reference Documentation
* **Project Design Overview:** ![User Flow Diagram](../images/Round%203%20beta-%20planning-pages.png)
* **Journey Map:** [Round 3 beta- planning: Lucidspark](https://lucid.app/lucidspark/510fe235-2efe-42af-8dc4-6b46c0aa5a83/edit?invitationId=inv_4fcca63f-2f2b-437e-b228-890279248a44&page=0_0#)

*End of ADR*
