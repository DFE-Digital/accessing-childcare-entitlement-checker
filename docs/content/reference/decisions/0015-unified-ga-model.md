---
title: 0015 Unified GA4 Event Model and Tag Helper Taxonomy
layout: sub-navigation
order: 0
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions
  key: Unified GA Model
---

## Context and problem statement

The service is currently limited to tracking solely via page views and GTM page-data triggers. As user interactions expand to include complex multi-step forms, error tracking, scheme impressions, and outbound click analytics (CTR), relying on page-level tracking alone is insufficient. 

Without a standardized front-end data contract, tracking new interaction events risks creating bespoke GTM triggers and hardcoded Razor scripts, leading to brittle analytics and potential PII leakage.

## Decision drivers

- **Prevent GTM Tag Explosion**: Ad-hoc, event-specific payloads lead to an unmaintainable web of GTM triggers and custom variables.
- **Developer Experience & Maintainability**: A single, predictable ASP.NET Core Tag Helper contract (`govg4-*` / `data-gov-event`) reduces cognitive load and enforces PII safety at compile time.
- **Alignment with GDS Standards**: Aligning directly with established [GOV.UK GA4 implementation guidelines](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/) ensures consistent reporting across government services.
- **Analytics Stability**: Decoupling immutable backend join keys (`action`) from localized, presentational copy (`ui_text`) prevents content updates from breaking analytics dashboards and BigQuery queries.
- **Generic Names**: Wherever possible we have chosen generic names (e.g. metric_name: impression) over specific ones, instead relying on a combination of parameters to drill down into specific data. This allows for more flexibility with how our Data Analyst can use the data 
## Considered options

1. **Option 1: Ad-hoc GTM triggers per interaction type**  
   Create unique HTML attributes and bespoke GTM tags whenever a new component requires tracking (e.g., `data-track-scheme`, `data-track-error`).
2. **Option 2: Native GA4 Ecommerce schema everywhere**  
   Force all events into full GA4 e-commerce array schemas (`items[]` with indices, price, category, etc.).
3. **Option 3: Unified, flat GOV.UK event model with optional GA4 item array (Selected)**  
   Implement a standardized, flat JSON payload contract (`data-gov-event`) emitted via ASP.NET Core Tag Helpers, capable of capturing single-element metrics, form states, and optional e-commerce item lists.

## Decision outcome

We will implement **Option 3**. All client-side analytics events will emit a single, flat JSON payload serialized to the `data-gov-event` DOM attribute.

### Core Payload Schema

The base JSON payload structure (`GovEventPayload`) contains the following properties:

| Parameter Key | Type | Purpose & Justification |
| :--- | :--- | :--- |
| `event` | `string` | Universal GTM custom event trigger (`gov_event`). Allows a single GTM trigger to handle all custom service interactions. |
| `event_name` | `string` | Standardized GA4 event identifier (`select_content`, `form_response`, `form_error`, `view_item_list`). |
| `type` | `string` | Describes the GOV.UK component type (`button`, `card`, `contact`, `input`). |
| `action` | `string` | **Immutable domain join key** (e.g., `EligibleForTaxFreeChildcare`, `submit`). Acts as the stable identifier for BigQuery joins, immune to UI copy edits. |
| `ui_text` | `string` | Presentational copy visible on screen (e.g., `Tax-Free Childcare`, `Continue`). Extracted automatically or overridden via Razor. |
| `section` | `string` | Logical page or form context (e.g., `results_details`, `children_details`). |
| `tool_name` | `string` | Overall service/calculator identifier (e.g., `childcare_eligibility_checker`). |
| `response` | `string` | Non-PII user input data captured during form progression (e.g., selected age or radio option). |
| `outcome` | `string` | Error or validation feedback text (e.g., `Enter a valid date of birth`). |
| `metric_name` | `string` | Generic quantitative metric label (e.g., `impression`, `children_count`, `child_age`). |
| `metric_value` | `int?` | Numeric value associated with `metric_name` for sum/average aggregations in BigQuery. |
| `items` | `array` | Optional array of cut-back GA4 item objects (`item_id`, `item_name`) for top-level list impressions. |
| `method` | `string` | GOV.UK sub-channel mechanism identifier (omitted by default). |
| `search_term` | `string` | On-site search query string (omitted by default). |
| `percent_scrolled` | `string` | Scroll depth tracking percentage (added by GTM "Scroll Depth"). |
| `clarity_session_id` | `string` | The specific session id provided by clarity in "window.clarity.getSessionId()", included to help analysts link up GA4 data with Clairty Data  |
---

### Item Object Schema (Cut-back GA4 Standard)

When tracking array-based impressions (e.g., `view_item_list`), the `items` property uses a simplified GA4 item structure:

- `item_id` (`string`): Immutable scheme or product identifier (e.g., `TaxFreeChildcare`).
- `item_name` (`string`): Human-readable scheme name (e.g., `Tax-Free Childcare`).

*(Note: Positional properties like `index` are intentionally omitted to avoid data fragmentation and noise in standard results reporting).*

---
### Naming convention / guidance

#### General advice
- Snake case will be used, in line with GA standards
- Prefer generic to specific
- Avoid dynamic names

#### event_name
Wherever possible the event name will re-use and existing GOV standard and if that doesnt exist, use a GA standard, because
- This is in line with the Gov approach
- Using these standard terms will make it easier to understand for those already familiar
- Using the standard events most likely has deeper meaning to GA allowing for better dashboards
  
##### Example `view_item_list`
Comes from the [GovUK Events Page](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#search-events) and this is based on [GA recommended events](https://developers.google.com/analytics/devguides/collection/ga4/reference/events?sjid=805636615090646734-EU&client_type=gtag#view_item_list)

As a standard "result list" type event and structure. Creating our own event name here might seem more inuitive to us now, but might cost us in the long run.

#### metric_name
Should be kept as generic as possible and never dynamic so instead of
`children_count_for_x_session` we simply use `children_count`
and instead of
`impression_EligibleForTaxFreeChildcare` we simple use `impression`

Because we can use combination of parameters to drill down into specific data, overly specific names here would reduce the flexibility with how our Data Analyst can use the data AND create more maintance issues with the dashboards.

### Metric guidance
Remember that if you have 2 tagged items shown, they will have their own event, which GA will add up itself, so for example "1" is a sensible metric value for essentially all impression events to avoid double counting

---

### Parameter Taxonomy Matrix

| Parameter Key | GOV.UK/GTM/GA4 Purpose | Standard Button Example (`Add Child`) | Scheme Impression Example (`Results Page`) | Standard Form Example (`Age Input`) | Form Validation Error Example (`Age Input`) | Reference Source |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **event** | Universal GTM Trigger | `gov_event` | `gov_event` | `gov_event` | `gov_event` | Internal GTM Trigger Pattern |
| **event_name** | Standard GA4 Event Name | `select_content` | `view_item_list` | `form_response` | `form_error` | [GA4 Standard Events](https://support.google.com/analytics/answer/13316687) / [GOV.UK Form Events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events) |
| **type** | GOV.UK Component Type | `button` | `card` | `contact` | `contact` | [GOV.UK Naming Conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |
| **action** | GOV.UK Action Type / Immutable Key | `add child` | `eligible_for_tax_free_childcare` | `submit` | `error` | [GOV.UK Naming Conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |
| **ui_text** | UI Label / Button Text | `Add Child` | `Tax-Free Childcare` | `Continue` | `Continue` | [GOV.UK Form Events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events) |
| **section** | Form / Question Context | `children details` | `results_details` | `How old is X` | `How old is X` | [GOV.UK Form Events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events) |
| **tool_name** | Overall Form Identifier | `childcare_eligibility_checker` | `childcare_eligibility_checker` | `childcare_eligibility_checker` | `childcare_eligibility_checker` | [GOV.UK Form Events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events) |
| **response** | User Input Data | `[omit]` | `[omit]` | `4` | `[omit]` | [GOV.UK Form Events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events) |
| **outcome** | Error / Result Text | `[omit]` | `[omit]` | `[omit]` | `Enter a valid number` | [GOV.UK Form Events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events) |
| **metric_name** | Generic Metric Identifier | `children_count` | `impression` | `child_age` | `child_age` | GA4 Custom Metric |
| **metric_value** | Dynamic Numeric Metric | `1` | `1` | `4` | `[undefined]` | GA4 Custom Metric |
| **method** | GOV.UK Sub-channel Mechanism | `[omit]` | `[omit]` | `[omit]` | `[omit]` | [GOV.UK Naming Conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |
| **search_term** | GOV.UK On-site Search Term | `[omit]` | `[omit]` | `[omit]` | `[omit]` | [GOV.UK Naming Conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |
| **percent_scrolled**| GOV.UK Scroll Depth Percentage | `[omit]` | `[omit]` | `[omit]` | `[omit]` | [GOV.UK Naming Conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |
| **items** | GOV.UK "Result" Items Array | `[omit]` | `[{"item_id": "TaxFreeChildcare", "item_name": "Tax-Free Childcare"}]` | `[omit]` | `[omit]` | [GOV.UK Ecommerce Events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#ecommerce-events) |
| **clarity_session_id** | Clarity documentation/their package | 123 | 123 | 123 | 123 |  https://github.com/microsoft/clarity/blob/b5e0944ef053be9b6ce4faeee6dacf37b206e724/packages/clarity-js/src/data/metadata.ts#L119 & https://learn.microsoft.com/en-us/clarity/setup-and-installation/identify-api  |

---

## Consequences

### Positive
- **Maintainable GTM Architecture**: GTM requires only two core tags/triggers: an auto-firing listener for `view_item_list` on `DOMContentLoaded` and a delegated click listener for interactive events.
- **Refactoring Safety**: Content copy changes in `.resx` localization files will not fragment historical BigQuery data or break GA4 CTR dashboards because joining relies on immutable `action` keys.
- **PII Compliance**: Centralizing serialization within C# Tag Helpers prevents developers from accidentally outputting raw user input fields into analytics payloads.

### Negative / Trade-offs
- **Payload Sparsity**: Unused properties on simpler events serialize as `null` or are omitted during JSON serialization (`JsonIgnoreCondition.WhenWritingNull`), requiring minor discipline when inspecting raw DOM attributes.
- **Strict Adherence Required**: Developers must use the `govg4-*` Tag Helpers consistently rather than writing custom `data-*` attributes manually in Razor.

## References

- [GOV.UK GA4 Naming Conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions)
- [GOV.UK GA4 Form Events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events)
- [GOV.UK GA4 Ecommerce & Search Events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#ecommerce-events)
- [Simo Ahava: Structure of an Event in GA4](https://www.simoahava.com/analytics/implementation-guide-events-google-analytics-4/#structure-of-an-event)