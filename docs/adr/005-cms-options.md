---
id: 0005
title: Refactoring Options for Entitlement Checker Journey Logic
status: Under Review
date: 2026-04-17
author: Hemant Khanduja
consulted: PM, Technical Team, TA from hippo and DfE
---

# ADR 0005: Refactoring Options for Entitlement Checker Journey Logic

## Status
Under Review (Pending internal technical review with Hippo and then with DfE leads)

## Context
The Accessing Childcare Entitlement (CEC) service requires a sustainable architecture to manage an extensive 40+ page eligibility journey. We must ensure the August 2026 Private Beta deadline is met while maintaining high accuracy, security, and "Value for Money" for the DfE. 

The current path (Decision 001) relies on hardcoding each page as a unique controller and view. While "Categories" have been introduced to organise these files, new insights from the **[Round 3 beta- planning: Lucidspark]** board show a highly repeatable pattern: ~25 pages of Radio Buttons, ~10 Checkboxes, and various Text Inputs. We must evaluate if the current path remains viable or if a refactor is required to meet the August 2026 delivery window.

## New Insights & Drivers for Change

1.  **Input Pattern Predictability:** 35+ out of 40 pages follow standard GDS input patterns. Hardcoding bespoke logic for identical input types is an inefficient use of DfE resource.
2.  **Scalability of Content Updates:** Typo fixes or threshold changes currently require a full developer deployment cycle.
3.  **CMS Migration Strategy:** Internal discussions have identified a desire to move to a Headless CMS (Contentful). We have uncovered that migrating from a structured JSON data model to a CMS is significantly more efficient than attempting to extract logic from 40+ hardcoded C# files.
4.  **Auditability (AZ-500):** Service Assessment requires a transparent "Source of Truth" for eligibility rules, which is currently buried in compiled code.

## Options Considered

### Option 1: Headless CMS (Contentful)
* **Description:** Direct implementation of a SaaS CMS.
* **Pros:** Best-in-class for non-technical ownership; established in **Care Leavers** project.
* **Cons:** Procurement/onboarding delays may risk the immediate Private Beta timeline.

### Option 2: Hardcoded Logic (Baseline/Current Path)
* **Description:** Continuing with "One Controller Per Page," using solution explorer categories for navigation.
* **Pros:** Familiar to the current team; no new platform learning curve.
* **Cons:** High technical debt; "Vendor Lock-in"; **Extremely difficult and manual to migrate to a CMS in the future.**

### Option 3: Logic Engine Refactor (JSON-led)
* **Description:** Abstracting the journey into a generic "Engine" driven by a centralised JSON schema.
* **Pros:** Zero licensing cost; adheres to **DRY** principles; **provides a clear data-mapping path for a future CMS migration.**
* **Cons:** Initial "Sprint 0" effort required to build the core engine.

## Scoring Matrix
*Scale: 1 (Poor) to 5 (Excellent). Weighted against DfE Digital Standards.*

| Criteria | Weight | Option 1: CMS | Option 2: Hardcoded | Option 3: JSON Engine |
| :--- | :--- | :--- | :--- | :--- |
| Suitability for 40+ Pages | 15% | 5 | 1 | 5 |
| Flexibility (Policy Changes) | 15% | 5 | 1 | 4 |
| **Ease of Migration to CMS** | 20% | 5 | **1** | **4** |
| Development Velocity | 15% | 4 | 2 | 5 |
| Infrastructure Cost (SaaS) | 10% | 3 | 5 | 5 |
| Auditability (Governance) | 15% | 5 | 2 | 5 |
| Maintenance & DRY | 10% | 5 | 1 | 5 |
| **Weighted Total** | **100%** | **4.6** | **1.8** | **4.6** |

## Discussion Points for Internal Review
To reach a consensus between the architectural views of Robert (CMS-first) and the project leads, we should discuss:
* **The "Migration Bridge":** Does the team agree that Option 3 acts as a functional prototype for Option 1, making the eventual jump to Contentful easier?
* **Resource Allocation:** Can we justify the "Sprint 0" effort to build the Engine if it guarantees we meet the August 2026 deadline?
* **Maintenance:** How do we mitigate Robert's concerns regarding JSON file size and complexity as the journey hits 40+ pages?

---
*End of ADR*