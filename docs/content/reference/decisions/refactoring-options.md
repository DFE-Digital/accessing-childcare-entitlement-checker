---
title: Refactoring options
layout: sub-navigation
order: 7
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions

---
## Context and problem statement

The Accessing Childcare Entitlement (CEC) application spans approximately 40 Government Design System (GDS) form pages.

Development is moving towards the private beta release of the MVP.

The project team selected the "Build" option in [ADR 0001 Entitlement checker design](/reference/decisions/entitlement-checker-design/). This option is a .NET Core MVC web application hosted in Azure and deployed via GitHub Actions. This ADR evaluates different implementation approaches for that option and documents spike work used to validate the selected approach.

## Decision drivers

### Primary drivers

These drivers remain unchanged since the original analysis and guide the choice of implementation approach:

* **Initial build cost:** The team must deliver MVP requirements on time and within budget.
* **Running cost:** The team must minimise infrastructure and licensing costs.
* **Cost of change:** Eligibility rules are subject to policy updates, making the cost of change a primary factor.

### Secondary drivers

#### Common data types and input patterns

Form pages exhibit high consistency. Approximately 25 pages utilise standard GDS Radio Buttons. Five to ten pages use Checkboxes, and the remainder use Text Boxes. The team prioritises reusable patterns and components in line with the DRY (Don't Repeat Yourself) principle to minimise maintenance overhead.

#### Future requirements

Emerging requirements for future phases include:

* Translation to Welsh and support for all UK nations.
* Results-sharing functionality.
* Financial calculator capabilities.
* Data forwarding into external application systems.

The system design must accommodate these future extension points.

## Considered options

### Option 1: Content and rules in ASP.NET MVC C#

This is the baseline approach against which other options are evaluated.

#### Pros

* **Low upfront cost:** The approach focuses on direct implementation of immediate requirements.
* **Adaptability:** Work completed feeds directly into future extension points.
* **Tooling and workflow:** The content pipeline aligns with the development pipeline, utilising existing QA, development, and deployment workflows.

#### Cons

* **Limited authoring capability:** Editing content requires development resources. All updates must pass through the full deployment lifecycle.

### Option 2: Content Management System (CMS) with rules described in C#

This option utilises "Contentful", a common solution within DfE.

#### Pros

* **Author autonomy:** Non-technical editors can complete content modifications. This potentially reduces ongoing development costs.
* **Established tooling:** Widely used and supported within DfE.

#### Cons

* **Increased running cost:** Additional licensing fees for Contentful.
* **High upfront cost:** Requires development time to design the content model and integrate the CMS API.
* **Operational dependencies:** The team must onboard and train non-technical content editors.

#### Evaluation

The team created a proof of concept using Contentful. The test confirmed that content editing functions as expected.

However, modifying content independently carries a high risk of application failure. This occurs because the form structure and text couple tightly to eligibility rules. A rules engine dictates the required inputs, their structure, and how they map to outputs. This creates a hard dependency on the form configuration. The form layer must remain constrained and validated against rules engine requirements.

### Option 3: Logic engine refactor (JSON-led)

An experimental option abstracts the journey and rules into a generic engine driven by a centralised JSON schema.

#### Evaluation

Spike work indicated that developing a generic rules schema for all potential childcare policies is highly complex.

Future policy changes are unpredictable. A custom rules definition schema would require expressiveness comparable to C# itself. This nullifies the benefits of abstraction, except for centralised text management.

## Decision outcome

The team selected **Option 1: Baseline**. Phase 1 implements a standard ASP.NET MVC application with no CMS or user-editable rules.

The following principles were established:

* The team extracts text content into standard .NET resource files (`.resx`). This provides centralised text management and supports future translation to Welsh.
* The team prioritises code reuse in line with DRY principles.
* Rules logic implementation focuses on minimising the cost of change to ease future policy updates.
* The expected frequency of changes is low. The team must re-evaluate the architectural approach if change frequency increases. This is especially true for text changes without eligibility rule updates.
* The team may simplify or parameterise rules over time. The solution design is configurable and adaptable to such requirements.
