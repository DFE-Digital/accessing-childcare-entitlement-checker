---
title: Add GA4 Parameter (to the unified model)
layout: sub-navigation
order: 2
sectionKey: How-to guides
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
  key: Add GA4 Parameter
---
Follow this runbook when a new GA4 data element is needed, which hopefully will be very rare

**NOTE**: New custom dimensions can take 24/48 hours to populate

## 1 Pre-Check
### Step 1.1: Is it needed?
Can we re-use an existing standardized payload field (`action`, `ui_text`, `section`, `metric_name`, `metric_value`) Avoid creating bespoke parameters if one of these fits

### Step 1.2: PII Evaluation
Ensure we will never intentionally or unintentionally capture Personally Identifiable Information.

### Step 1.3: Naming conventions Evaluations
Are we following the naming conventions establised in (the ADR)[#reference/decisions/0015-unified-ga-model.md]

## 2 GTM Setup

### Step 2.1: Create the Data Layer Variable
- Navigate to Variables -> User-Defined Variables -> New
    - *Type*: Data Layer Variable
    - *Version*: Version 2 (or latest)
    - Save

### Step 2.2: Attach parameter to our Tag
- Navigate to Tabs
- Select the custom `ga event`  
- Event Parameters -> Add Row
    - *Parameter Name*: snake_case (no squiggly brackets `{{ "{{}}" }}`) for how it should appear in GA
    - *Value*: Press + and select variable from step 1 (this will have squiggly brackets `{{ "{{}}" }}`)
- (Optional) if the param applies to all events , put it in the GA4 Configuration Tag
- Save

## 3 GA Setup

### Step 3.1: Register Custom Dimension (Text only)
- Admin (gear icon)
- Data Display -> Custom Definitions
- Create Custom Dimension
    - *Dimension Name*: Human readable (not snake case)
    - *Scope*: Event
    - *Description*: Describe what is captures and where it comes from
    - *Event parameter*: Select (or type) the exact parameter key used in GTM

### Step 3.2: Register Custom Metric (Numeric)
- Admin (gear icon)
- Data Display -> Custom Definitions -> Custom Metrics **tab**
- Create Custom Metric
    - *Metric Name*: Human readable (not snake case)
    - *Scope*: Event
    - *Description*: Describe what is captures and where it comes from
    - *Event parameter*: Select (or type) the exact parameter key used in GTM
    - *Unit of measurement*: Select appropriate
- Save

## Checklist
- Parameter confirmed needed
- Parameter confirmed PII risk-free
- GTM Variable Created
- GTM Tag updated and mapped
- GA4 Custom dimension/metric registered in GA4 Admin
- Tested using Debug View
- ADR 0015 taxonmony followed,
- new ADR created with reasons for new parameter




