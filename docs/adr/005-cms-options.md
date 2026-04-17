---
id: 005
title: Refactoring Options for Entitlement Checker Journey Logic
status: Proposed
date: 2026-04-17
decided_by: Senior Lead Architect
consulted: PM, Technical Team, Hippo Jon
---

# ADR 002: Refactoring Options for Entitlement Checker Journey Logic

## Status
Proposed (Pending PM/Stakeholder Review)

## Context
The Accessing Childcare Entitlement (CEC) service requires a sustainable architecture to manage an extensive 40+ page eligibility journey. We must ensure the August 2026 Private Beta deadline is met while maintaining high accuracy, security, and "Value for Money" for the DfE. 

The current development path involves hardcoding each page as a unique controller/view (Option 2). While the team has recently introduced "Categories" within the solution explorer to help organise these controllers, this refactoring is primarily navigational. It makes the solution a little bit easier to navigate for developers, but it does not reduce the actual volume of code to be maintained, nor does it address the lack of policy transparency for non-technical stakeholders.

## Options Considered

### Option 1: Headless CMS Integration (Contentful)
* **Pros:** Externalises content and logic to a governed platform; proven in the **Care Leavers** project.
* **Cons:** Monthly SaaS costs; requires procurement and security onboarding.

### Option 2: Hardcoded Logic (Existing Path)
* **Pros:** Fast start for isolated pages; no initial platform overhead.
* **Cons:** Violates **DRY (Don't Repeat Yourself)** principles. Even with the current controller categorisation, this approach requires 40+ unique files. This creates massive maintenance overhead and "Vendor Lock-in," as simple content changes remain dependent on full developer sprint cycles.

### Option 3: Logic Engine Refactor (JSON-led)
* **Pros:** Implements "Logic-as-Data" via Git-based JSON (Transparent, Rule-Based Logic); zero licensing cost; high agility for Private Beta.
* **Cons:** Requires technical knowledge for initial JSON mapping.

## Scoring Matrix
*Scale: 1 (Poor) to 5 (Excellent). Weighted against DfE Digital Standards.*

| Criteria | Weight | Option 1: CMS | Option 2: Hardcoded | Option 3: JSON Engine |
| :--- | :--- | :--- | :--- | :--- |
| Suitability for 40+ Pages | 20% | 5 | 1 | 5 |
| Flexibility (Policy Changes) | 20% | 5 | 1 | 4 |
| Development Velocity | 15% | 4 | 2 | 5 |
| Infrastructure Cost (SaaS) | 10% | 3 | 5 | 5 |
| Auditability (AZ-500) | 15% | 5 | 2 | 5 |
| Maintenance & DRY | 10% | 5 | 1 | 5 |
| Scaling & Resilience | 10% | 5 | 5 | 5 |
| **Weighted Total** | **100%** | **4.6** | **2.0** | **4.8** |

## Decision
We will proceed with **Option 3 (Logic Engine Refactor)** for the Private Beta development. We will build this using the `ICmsFormService` interface to ensure a seamless "Hot-Swap" to **Option 1 (CMS)** if the DfE decides to move content ownership to policy leads later.

We **strongly reject Option 2** as it fails to meet DfE standards for maintainability and scalability, regardless of how controllers are categorised in the solution explorer.

## Consequences

### Positive
* **Agility:** Policy changes can be made in hours via JSON updates rather than days via C# code changes.
* **Auditability:** Provides a single "Source of Truth" for eligibility rules that is visible outside of the compiled binary.
* **Professional Alignment:** Matches the standard refactoring patterns used in the **Care Leavers** and **Benchmarking** portfolios.

### Negative
* **Technical Entry Barrier:** Developers must learn the generic "Engine" pattern rather than building bespoke pages.

### Neutral
* **Architecture Handover:** Requires a robust handover to ensure the team understands how to extend the JSON schema.

---
*End of ADR*