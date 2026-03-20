---
status: "proposed"
---

# Use govuk-frontend-aspnetcore library for GDS elements

## Context and Problem Statement

This project uses GDS components. It's possible to implement this in a variety of ways.

## Decision Drivers

* can we implement validation of e.g. dates
* do we have control over long strings of text in the results output. (Tab conponent)
* is this wrapper widely used across other DfE repos?
* is it actively maintained, and what’s the release cadence?
* What happens when http://GOV.UK  updates a component, how quickly does the wrapper follow? Are our team happy if the components get updated automatically in the future?
* Can we easily drop down to raw HTML if we hit edge cases? That this approach doesn't cater for - maybe like the results page?
* Are there known limitations we should be aware of?
* The Razor is definitely cleaner. Number of lines of code. (LOC)

## Considered Options

* Using npm package
* Using nuget package

## Decision Outcome

Chosen option: "Using nuget package" - because it's a .NET oriented solution, uses tag helpers to reduce
LOC, and meets the other criteria outlined in decision drivers.

### Consequences

* Good, because fewer LOC
* Good, because updated via Nuget and doesn't require a separate NPM build step