---
title: Entitlement checker design
layout: page
showPagination: true
order: 1
sectionKey: Decisions
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions

---
## Summary

The recommended proposal as an outcome from Alpha is to build an Entitlements Checker web application, hosted on gov.uk. This checker will improve on current diverse checkers by covering more entitlements, and being a single source of truth going forward. The checker is not designed to confirm eligibility, and is there as a way to signpost users to onward journeys and further investigation. 

There is a stretch goal for Beta to also see how financial calculator features can be incorporated in a meaningful way into the checker, although this is not MVP and does not change the fundamental requirements.

## What’s the problem we are trying to solve?

There are options for how to design and build the entitlement checker, given the current requirements: 

* **Multiple questions** will be asked to check entitlements, including radio and checkbox selection, date of birth entry, text box entry for numbers and free text (to store child names).
* Final answers page will display different results depending on **multiple combinations of answers** such as age of child, income, benefits claimed and disability status.
* There is **no need to store data** beyond current session (no current plans for save and resume).
* The service will be **open to the public**, via a gov.uk website address.
* The schemes being checked **do not change very often**, and change would likely be either easy (thresholds) or extremely complex (new questions or intersections of answers).
* Currently understood KPIs are either on **simple usage statistics**, or surveyed effectiveness.

## What options have we considered?

### Option 1: Get To An Answer
https://github.com/DFE-Digital/get-to-an-answer

https://staging-admin.get-to-an-answer.education.gov.uk/

Get To An Answer is a tool recently built for DfE that displays answer pages given certain question data. It comprises of a CMS that allows for questions to be added with text and answer pages with custom text. Each GTTA instance has its own URL and can be hosted inside an iframe. 

#### Pros

* Service already supports radio and checkboxes and custom answer text
* Easy to mange in the future for new questions, new answer text

#### Cons

* Doesn’t handle complex intersection of answers without building a dedicated answer page per permutation, which would run into hundreds of pages, meaning support would be very challenging
* Tool doesn’t support date or text data entry
* Doesn’t support direct vanity URL

The tool could be updated to meet our needs, but this would require an effective re-write to handle state, and base answer panels on complex combinations of question data. This would require the migration of current users and would also overly complicate what is a simple, fit for purpose tool.

### Option 2: Eligibility Checking Engine
https://github.com.mcas.ms/DFE-Digital/eligibility-checking-engine 

https://eligibility-checking-engine.education.gov.uk/swagger/index.html

The eligibility checking engine is an API primarily used by local authorities to check HMRC codes for childcare eligibility, free school meals, etc. 

#### Pros

* A centralised API for all scheme entitlement is a good ideal in theory

#### Cons

* Entitlements are different from Eligibility
* Currently none of the scheme entitlements are checked by the existing API
* Some entitlements are currently owned by other government agencies - HMRC, DWP - and moving that to this API would be politically challenging and time consuming
* Would need to open this out to allow for public calls to the API

### Option 3: Build

![Describes the technical architecture with a diagram](../adr/images/0001-Technical%20Architecture.png "Technical Architecture")

Given the requirements for a simple web application with no database or user auth, it would be simple to build a .net core MVC web application, hosted in azure and deployed via github actions. 

#### Pros

* Established patterns within DfE
* No integrations
* Easy to maintain
* Use configuration for thresholds
* Changes will be rigorously tested via deployment automation
* Google analytics sufficient for MVP KPIs

#### Cons

* Its another ‘thing’ that DfE have to own

## What is the recommendation?

Given the low frequency of updates and the simplicity of the current requirements, it is recommended to build a new service.

## What are the consequences of this recommendation?

See [Planning for delivery](https://dfedigital.atlassian.net/wiki/spaces/AC/pages/5883396122). A dev team (2 devs) will need to be hired to build the custom service. Due to the simplicity of the Azure requirements, there is no need for a specialised Dev Ops developer.

Going forward, excellent documentation will be required to allow for ease of supporting the application and managing change.

## Where can I find more details?

https://lucid.app/lucidspark/b7452515-3d80-43d6-8031-cc5114122623/edit