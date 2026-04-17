---
id: 0005
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

The current path (Decision 001) relies on hardcoding each page as a unique controller and view. While the development team has introduced "Categories" within the solution explorer to organise these files, new insights from the **[Round 3 beta- planning: Lucidspark]** board show a highly repeatable pattern: ~25 pages of Radio Buttons, ~10 Checkboxes, and various Text Inputs. We must evaluate if the current path remains viable or if a refactor is required to support upcoming Phase 2 and Public Beta requirements.

## New Insights & Drivers for Change

1. **Input Pattern Predictability:** 35+ out of 40 pages follow standard GDS input patterns (Radio Buttons, Checkboxes, Text Boxes). Hardcoding bespoke logic for near-identical input types is an inefficient use of DfE resource and violates fundamental **DRY (Don't Repeat Yourself)** principles.

2. **Scaling for Public Beta Requirements:** Significant future requirements have been "parked" for the Public Beta phase, including:
   * **Analytics & Insights:** Integration of Google Analytics (GA), Google Tag Manager (GTM), and Microsoft Clarity across all 40+ screens.
   * **External Dependencies:** Potential for No. 10 department-led policy pivots and external data dependencies.
   * **Financial Tooling:** A dedicated Financial Calculator in Phase 2.
   Implementing these via a CMS (Option 1) or a Logic Engine (Option 3) allows for global injection of scripts and logic, whereas Option 2 would require manual updates across 40+ separate controllers.

3. **CMS Migration Strategy (The "Stepping Stone"):** Internal discussions have identified a long-term desire to move to a Headless CMS (Contentful). Migrating from a structured JSON data model (Option 3) to Contentful is a straightforward data-mapping exercise. Conversely, migrating from 40+ hardcoded controllers (Option 2) would require a manual, high-cost rewrite. Option 3 acts as the necessary "bridge" to Robert's preferred CMS end-state.

4. **Maintenance & Auditability (AZ-500):** Typos or threshold updates currently require full developer deployment cycles. Furthermore, Service Assessment requires a transparent "Source of Truth" for eligibility rules, which is currently buried in compiled binary code.

## Options Considered

### Option 1: Headless CMS (Contentful)
* **Description:** Direct implementation of a SaaS CMS to manage journey logic and content.
* **Pros:** Best-in-class for non-technical ownership; established in **Care Leavers** project; easily handles global analytics and parked Public Beta features.
* **Cons:** Procurement/onboarding delays may risk the immediate Private Beta timeline.

### Option 2: Hardcoded Logic (Baseline/Current Path)
* **Description:** Continuing with "One Controller Per Page," using solution explorer categories for navigation.
* **Cons:** High technical debt; "Vendor Lock-in"; creates an artificial dependency on developers for minor changes. **Extremely difficult and manual to migrate to a CMS or integrate global analytics in the future.**

### Option 3: Logic Engine Refactor (JSON-led)
* **Description:** Abstracting the journey into a generic "Engine" driven by a centralised JSON schema in Git.
* **Pros:** Zero licensing cost; adheres to DRY principles; acts as a functional prototype and **data-mapping bridge** for an eventual jump to Contentful.
* **Cons:** Initial "Sprint 0" effort required to build the core engine.

## Scoring Matrix
*Scale: 1 (Poor) to 5 (Excellent). Weighted against DfE Digital Standards.*

| Criteria | Weight | Option 1: CMS | Option 2: Hardcoded | Option 3: JSON Engine |
| :--- | :--- | :--- | :--- | :--- |
| Suitability for 40+ Pages | 15% | 5 | 1 | 5 |
| Flexibility (GA/GTM/Calculator) | 15% | 5 | 1 | 4 |
| **Ease of Migration to CMS** | 20% | 5 | **1** | **4** |
| Development Velocity | 15% | 4 | 2 | 5 |
| Infrastructure Cost (SaaS) | 10% | 3 | 5 | 5 |
| Auditability (Governance) | 15% | 5 | 2 | 5 |
| Maintenance & DRY | 10% | 5 | 1 | 5 |
| **Weighted Total** | **100%** | **4.6** | **1.8** | **4.6** |

## Discussion Points for Internal Review
* **The "Migration Bridge":** Does the team agree that Option 3 acts as a functional prototype for Option 1, making the eventual jump to Contentful easier while meeting Private Beta needs?
* **Resource Allocation:** Can we justify the "Sprint 0" effort to build the Engine to ensure we can easily integrate GA/GTM and the Financial Calculator later?
* **Categorisation vs. Refactoring:** Acknowledging that current "Category" organisation helps navigation, does it solve the long-term risk of maintaining 40+ distinct integration points for analytics?

---
*End of ADR*