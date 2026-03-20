---
status: "proposed"
---

# Use govuk-frontend-aspnetcore library for GDS elements

## Context and Problem Statement

This project uses GDS components. It's possible to implement this in a variety of ways.

## Decision Drivers

* less lines of code (LOC)
* framework-native library, rather than introducing NPM dependency.
  * n.b. - we're probably going to need npm at some point (Pa11y?) but then again, that won't be part of the core build chain

### Specific questions that need addressing

#### can we implement validation of e.g. dates

Yes, see [example](https://github.com/x-govuk/govuk-frontend-aspnetcore/tree/main/samples/Samples.DateInput) and [docs](https://github.com/x-govuk/govuk-frontend-aspnetcore/blob/main/docs/components/date-input.md)

#### do we have control over long strings of text in the results output. (Tab conponent) 

I don't think this is specifically an issue with the nuget package. It's a GDS issue more generally; so covered under the "dropping down to raw HTML" q.

See [docs](https://github.com/x-govuk/govuk-frontend-aspnetcore/blob/main/docs/components/tabs.md) for tabs docs.

#### is this wrapper widely used across other DfE repos?

At time of writing it's referenced by [37 projects](https://github.com/search?q=org%3ADFE-Digital++%3CPackageReference+Include%3D%22GovUk.Frontend.AspNetCore%22+%2F%3E&type=code) across the DFE Digital org.

A [fork of the repo](https://github.com/DFE-Digital/govuk-frontend-aspnetcore) exists in the DFE Digital org.

#### is it actively maintained, and what’s the release cadence?

There are [Regular releases](https://github.com/x-govuk/govuk-frontend-aspnetcore/releases) and the last release lagged the underlying GDS release by 14 days.

There's currently [a pull request open](https://github.com/x-govuk/govuk-frontend-aspnetcore/pull/450) for upgrading to the latest 6.0 release.

#### What happens when http://GOV.UK  updates a component, how quickly does the wrapper follow? Are our team happy if the components get updated automatically in the future?

As above the last release (a point release) lagged by 14 days. There's an open PR for the major point update which was on Feb 9th.

> Are our team happy if the components get updated automatically in the future?

We'd pin the nuget package and only update it when we're ready to take and test the changes.

#### Can we easily drop down to raw HTML if we hit edge cases? That this approach doesn't cater for - maybe like the results page?

Yes, you don't have to use the tag helpers at all. You can also [customise the various layouts](https://github.com/x-govuk/govuk-frontend-aspnetcore?tab=readme-ov-file#4-configure-your-page-template) etc.

#### Are there known limitations we should be aware of?

I don't think there any other additional limitations.

## Considered Options

* Using npm package
* Using nuget package

## Decision Outcome

Chosen option: "Using nuget package" - because it's a .NET oriented solution, uses tag helpers to reduce
LOC, and meets the other criteria outlined in decision drivers.

### Consequences

* Good, because fewer LOC
* Good, because updated via Nuget and doesn't require a separate NPM build step
* Bad, because currently lags major point releases by 6 weeks
