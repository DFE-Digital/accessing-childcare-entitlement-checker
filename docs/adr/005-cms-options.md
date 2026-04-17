---
id: 005
title: Refactoring Options for Entitlement Checker Journey Logic
status: Under Review
date: 2026-04-17
author: Senior Lead Architect
consulted: PM, Technical Team, Jon (Hippo), DfE Portfolio Leads
---

# ADR 005: Refactoring Options for Entitlement Checker Journey Logic

## Status
Under Review (Pending internal technical review with Hippo and then with DfE leads)

## Context
The Accessing Childcare Entitlement (CEC) service must deliver a robust, 40+ page eligibility journey for the August 2026 Private Beta. The design of this journey, as captured on the **[Round 3 beta- planning: Lucidspark]** board, shows a highly structured and repeatable flow.

The initial assumption (Decision 001) was based on a smaller-scale journey where hardcoding individual controllers and views was considered sufficient for a rapid start.

While the team has made efforts to organise this growing complexity—such as introducing "Categories" in the solution explorer to group controllers—new insights have emerged which suggest the original architecture may no longer be fit for purpose. We must re-evaluate the technical strategy to avoid severe "Technical Debt" that will likely compromise the August 2026 launch.

## New Insights & Drivers for Change
Since Decision 001 was made, the following technical and operational insights have been uncovered, rendering the previous hardcoded path obsolete:

1.  **Repeatable GDS Design Patterns:** An audit of the 40+ planned pages reveals an ultra-consistent structure. Roughly 25 pages are standard **GDS Radio Buttons** (single-choice), 5–10 are **Checkboxes** (multi-choice), and the remainder are standard **Text Boxes**. Hardcoding 40 unique C# views when the input types are so predictable is a fundamental violation of modern software design principles.

2.  **Maintenance Latency:** Even with controller categorisation, simple typos or threshold updates require full developer deployment cycles (Code Change -> PR -> Merge -> CI/CD). This creates an unsustainable "Change Bottleneck" for the DfE.

3.  **Volume of Policy Logic:** The eligibility branching rules are multi-faceted. Scattered across 40+ controllers, they become "fragmented logic," which is impossible to audit comprehensively for Service Assessment.

4.  **AZ-500 & Governance (Auditability):** Standard DfE architecture requires "Transparent, Rule-Based Logic" (Policy-as-Code). A hardcoded approach buries the "Source of Truth" inside compiled binary files, which fails modern cloud governance standards.

### Reference: Current System Design
The following image provides a high-level overview of the current hardcoded architecture, which this ADR proposes to refactor. The image highlights the code duplication and fragmentation across numerous controllers.

> *[Attached: Picture demonstrating the current design, showing code fragmentation and multiple distinct controllers for individual screens. Filename: image_1.png]*

## Options Considered

### Option 1: Headless CMS Integration (Contentful)
* **Description:** Moving journey content and branching logic into a governed SaaS platform. Proven standard used in the **Care Leavers** project.

### Option 2: Hardcoded Logic (Baseline/Current Path)
* **Description:** Continuing with the "One Controller Per Page" pattern, relying on solution explorer categories for organisation.

### Option 3: Logic Engine Refactor (JSON-led)
* **Description:** Abstracting the journey into a generic "Engine" (handling radio buttons, checkboxes, and text inputs) driven by a centralised JSON schema stored in Git. Standard used in the DfE **Benchmarking** portfolio.

## Scoring Matrix
*Scale: 1 (Poor) to 5 (Excellent). Weighted against DfE Digital Standards.*

| Criteria | Weight | Option 1: CMS | Option 2: Hardcoded | Option 3: JSON Engine |
| :--- | :--- | :--- | :--- | :--- |
| **Suitability (Field Pattern Matching)** | 20% | 5 | **1** | 5 |
| Flexibility (Policy Changes) | 20% | 5 | 1 | 4 |
| Development Velocity | 15% | 4 | 2 | 5 |
| Infrastructure Cost (SaaS) | 10% | 3 | 5 | 5 |
| Auditability (Governance) | 15% | 5 | 2 | 5 |
| Maintenance & DRY Principles | 10% | 5 | **1** | 5 |
| Scaling & Resilience | 10% | 5 | 5 | 5 |
| **Weighted Total** | **100%** | **4.6** | **2.0** | **4.8** |

### Critical Failure Analysis of Option 2 (Hardcoded)
The scoring matrix shows a critical fail (2.0) for the hardcoded path. The key drivers are:
* **"CRITICALLY LOW" Suitability**: In a journey where ~35 out of 40 pages are just Radio Buttons or Checkboxes, continuing to build bespoke code for each screen is functionally redundant. The best refactoring category is input type (e.g., a "Radio Button Controller" that can render any single-choice question based on data).
* **"EXTREME" Maintenance Overhead**: Controller categorisation only improves navigation for developers; it does not solve the underlying technical debt of code repetition.

## Discussion Points for Internal Review
Before a final decision is recommended to the client, the following points require internal consensus:
* Are the technical leads (Matt and Steve) satisfied with the JSON schema definitions for the GDS input types?
* What is the velocity impact of pausing bespoke view creation for 1–2 sprints to build the "Generic Engine"?
* How do we communicate the "burden of proof" of discovery insights to the DfE in a way that respects the original discovery effort?

