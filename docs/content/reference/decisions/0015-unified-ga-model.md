---
title: 0015 Unified GA model
layout: sub-navigation
order: 0
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions
  key: Unified GA Model
---
## Context and problem statement

The website is currently limited to tracking solely by pages and page data in GTM.


## Decision drivers

* An ad-hoc approach can lead to an explosion of GTM tags to maintain.
* An established common pattern is easier to maintain and reduces cognitive load
* Gov UK documentation establishes common patterns for events already

## Considered options

* GTM tag per new event
* 

## Decision outcome

Will use a flat model with the following paramaters:
* event       (string)
* event_name  (string)
* type        (string)
* action      (string)
* ui_text     (string)
* section     (string)
* tool_name   (string)
* response    (string)
* outcome     (string)
* metric_name (string)
* metric_value  (int)
* items       (array of item object)
* method      (string)
* search_term (string)
* percent_scrolled  (string)

## Item object
Following a cut back [GA4 standard item object](https://developers.google.com/analytics/devguides/collection/ga4/reference/events?client_type=gtag#add_payment_info_item)

* item name   (string)
* item id     (string)

### Reasons and usage of each parameter
| Parameter Key    | GOV.UK/GTM/GA4 Purpose                | Standard Button Example (Add Child) | Scheme Impression Example (Results Page)                                                                                                                      | Standard Form Example (Age Input) | Form validation error Example (Age Input) | Reference / Documentation Source                                                                                                                                                                                                                           | Reference / Documentation Source2                                                                                                                                                                                              |
| ---------------- | ------------------------------------- | ----------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------- | ----------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| event            | Universal GTM Trigger                 | gov_event                           | gov_event                                                                                                                                                     | gov_event                         | gov_event                                 | Internal GTM Trigger Pattern                                                                                                                                                                                                                               |                                                                                                                                                                                                                                |
| event_name       | Standard GA4 Event Name               | select_content                      | view_item_list                                                                                                                                               | form_response                     | form_error                                | [https://support.google.com/analytics/answer/13316687](https://support.google.com/analytics/answer/13316687)                                                                                                                                               | https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events                                                                                                                     |
| type             | GOV.UK Component Type                 | button                              | card                                                                                                                                                          | contact                           | contact                                   | [https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |                                                                                                                                                                                                                                |
| action           | GOV.UK Action Type                    | add child                           | EligibleForTaxFreeChildcare                                                                                                                                   | submit                            | error                                     | [https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |                                                                                                                                                                                                                                |
| ui_text          | UI Label / Button Text                | Add Child                           | Tax-Free Childcare                                                                                                                                            | Continue                          | Continue                                  | [https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events)                                   |                                                                                                                                                                                                                                |
| section          | Form / Question Context               | children details                    | results_details                                                                                                                                               | How old is X                      | How old is X                              | https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events                                                                                                                                                 |                                                                                                                                                                                                                                |
| tool_name        | Overall Form Identifier               | child_benefit_calculator            | childcare_eligibility_checker                                                                                                                                 | child_benefit_calculator          | child_benefit_calculator                  | https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events                                                                                                                                                 |                                                                                                                                                                                                                                |
| response         | User Input Data                       | string/number                       | [omit]                                                                                                                                                        | 4                                 | [omit]                                    | https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events                                                                                                                                                 |                                                                                                                                                                                                                                |
| outcome          | Error / Result Text                   | string/number                       | [omit]                                                                                                                                                        | [omit]                            | Enter a valid number                      | https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#form-events                                                                                                                                                 |                                                                                                                                                                                                                                |
| metric_name      | Generic Metric Identifier             | children_count                      | impression                                                                                                                                                    | child_age                         | child_age                                 | GA4                                                                                                                                                                                                                                                        |                                                                                                                                                                                                                                |
| metric_value     | Dynamic Numeric Metric                | 1                                   | 1                                                                                                                                                             | 4                                 | [undefined]                               | GA4                                                                                                                                                                                                                                                        |                                                                                                                                                                                                                                |
| method           | GOV.UK Sub-channel / System Mechanism | [omit] (placeholder)                | [omit]                                                                                                                                                        | [omit] (placeholder)              | [omit] (placeholder)                      | [https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |                                                                                                                                                                                                                                |
| search_term      | GOV.UK On-site Search Term            | [omit] (placeholder)                | [omit]                                                                                                                                                        | [omit] (placeholder)              | [omit] (placeholder)                      | [https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |                                                                                                                                                                                                                                |
| percent_scrolled | GOV.UK Scroll Depth Percentage        | [omit] (placeholder)                | [omit]                                                                                                                                                        | [omit] (placeholder)              | [omit] (placeholder)                      | [https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions) |                                                                                                                                                                                                                                |
| items            | GOV.UK "result" items                 | [omit]                              | [ { "item_id": "TaxFreeChildcare", "item_name": "Tax-Free Childcare" }, { "item_id": "UniversalCreditChildcare", "item_name": "Universal Credit Childcare"} ] | [omit]                            | [omit] (placeholder)                      | [https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#ecommerce-events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#ecommerce-events)                       | [https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#search-events](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#search-events) |

## Consequences
* Tight enforcement of this model
* Some events will have unused paramaters

## References

* [Gov GA4 Naming conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/#govuk-ga4-naming-conventions)
* [Gov GA4 Search naming conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#selectcontent-events)
* [Gov GA4 Form Event conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#form-events)
* [Gov GA4 Event conventions](https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#govuk-ga4-events)
* [Simo Ahava, Event Structure](https://www.simoahava.com/analytics/implementation-guide-events-google-analytics-4/#structure-of-an-event)
