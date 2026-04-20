# ADR 0005: Refactoring Options

## Context and Problem Statement

The Accessing Childcare Entitlement (CEC) spans 40 pages of Government Design System (GDS) form pages.

We're currently working towards the private beta release of the MVP.

In [ADR 0001 Entitlement checker design](./0001-entitlement-checker-design.md) design work resulted in the selection of the "Build" option: a .net core MVC web application, hosted in azure and deployed via github actions. This ADR considers different implementation approaches for that option and documents spike work used to validate the chosen approach.

## Decision Drivers

### Primary drivers

These are unaltered since the original analysis; but are worth restating as primary drivers to choosing an implementation approach.

* Initial build cost: The project is time limited. We need to achieve the MVP requirements on time and at cost.
* Running cost: We need to minimise infrastructure and licensing costs when possible.
* Cost of change: Change is always a factor. In this project the most disruptive kind of change is likely to come from changes in policy that alter eligibility rules.

### Secondary drivers

#### Common data types and input patterns.

As the [design work](https://lucid.app/lucidspark/510fe235-2efe-42af-8dc4-6b46c0aa5a83/edit?invitationId=inv_4fcca63f-2f2b-437e-b228-890279248a44&page=0_0#) approaches completion it is apparent that the forms are extremely consistent. Approximately 25 pages utilise standard GDS Radio Buttons, 5–10 use Checkboxes, and the remainder use Text Boxes. It's sensible to follow the **DRY (Don't Repeat Yourself)** principle and reuse patterns and code wherever possible, to minimise work and reduce maintenance burden.

#### Accomodating future plans

While the priority is of course the current development phase; we can keep in mind emerging requirements for future phases:

* Translation to Welsh and accomodating all UK nations.
* Possible results sharing functionality.
* A "financial calculator" capability.
* Forwarding of input into an application system.

Any system we build should work towards future changes wherever possible.

## Considered Options

### Option 1: Content and rules in ASP.NET MVC C#

This is the "baseline approach" against which the other options are to be evaluated.

#### Pros

* Low upfront cost: Straightforward implementation focussed on the immediate requirements.
* Adaptability: work done can easily "feed forward" into future solutions.
* Tooling and workflow: Content pipeline is the same as the development pipeline and can make use of the same tooling, QA and development practices.

#### Cons

* Limited authoring capability: Editing content requires development resource; and needs to go through the full deployment lifecycle.

### Option 2: Content Management System (CMS) with rules described in C#

The "go-to" solution within DfE for this is a system called "Contentful".

#### Pros

* Building the system on top of a CMS could reduce the cost of change by allowing internal users to modify content themselves; rather than requiring technical support.
* Commonly used solution within DfE.

#### Cons

* Running cost increases due to licensing.
* High upfront cost: requires development time building a content model, and integrating Contentful.
* To realise reduction in ongoing development cost; a content editor must be onboarded to the project. 

#### Evaluation

We created a proof of concept using Contentful; and found that with upfront developer effort, it could allow for editing content as expected.

However; because the content (text and form structure) is extremely tightly bound to the rules; allowing users to edit the content carries extreme risk of breaking the application unless internal users can also edit the rules for checking eligibility.

### Option 3: Logic Engine Refactor (JSON-led)

This was an experimental option where the journey and rules are abstracted into a generic "Engine" driven by a centralised JSON schema.

#### Evaluation

We found that it would be difficult or impossible at this stage to develop a set of rule options that would cover all possible childcare policies.

Since we can't fully anticipate what policy changes might be required, a custom policy rules definition would need to have similar expressiveness to C# itself. This nullifies almost all of the benefit of such a system, except for the centralisation of text resources.

## Decision Outcome

Option 1: Baseline - we will build phase 1 of the app as a straightforward ASP.NET MVC application with no CMS or user-editable rules element. 

We agreed:

* It's best practice to extract text content, and the baseline implementation should do this using dotnet "resource files". This will have similar benefits to the JSON flat file in terms of centralising where text content is stored. This will also be advantageous if future translation to Welsh is needed.
* We should seek to reuse code whenever practical in line with DRY principles.
* We should have a strong focus on cost of change when implementing the rules logic; so that we minimise the effort required to implement future policy changes.
* As per the original ADR; the expected number of changes is low. If the expected number of changes increases; we should look again at the approach; especially if there are frequent content (text) changes without a corresponding change in the eligibility rules.
* During the system lifecycle we may find that some of the rules can be expressed in simple or limited terms; or parameterised in some other way. We should be ready to adapt and make sure the solution is flexible and configurable where needed.