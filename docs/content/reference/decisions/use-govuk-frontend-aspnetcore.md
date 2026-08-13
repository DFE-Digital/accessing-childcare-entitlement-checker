---
title: Use govuk-frontend-aspnetcore library for GDS elements
layout: sub-navigation
order: 6
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions

---
## Context and problem statement

The project requires the implementation of Government Design System (GDS) components. Multiple implementation options exist.

## Decision drivers

* Reducing lines of code (LOC) through abstraction.
* Preferring framework-native libraries to minimise external dependencies.  
  Note: The project may require additional tools in the future, such as Pa11y via NPM. However, these tools are not part of the core build chain.

## Technical evaluation

### Date validation

The library supports date validation. The [Samples.DateInput sample](https://github.com/x-govuk/govuk-frontend-aspnetcore/tree/main/samples/Samples.DateInput) and the [date-input component documentation](https://github.com/x-govuk/govuk-frontend-aspnetcore/blob/main/docs/components/date-input.md) provide examples and technical specifications.

### Text control in results output (Tab component)

Control of long text strings is a GDS component design consideration, not a library-specific limitation. Raw HTML capabilities support customisation options. Refer to the [tabs component documentation](https://github.com/x-govuk/govuk-frontend-aspnetcore/blob/main/docs/components/tabs.md) for details.

### Organisation usage

At the time of writing, 37 projects within the DfE Digital organisation reference the `GovUk.Frontend.AspNetCore` package.

### Maintenance and release cadence

The package has regular releases. Minor point releases historically lag underlying GDS releases by approximately 14 days. An open pull request supports the [6.0 release](https://github.com/x-govuk/govuk-frontend-aspnetcore/pull/450).

### Component update policy

Minor updates lag GDS by approximately 14 days. Open pull requests manage major updates. The team pins and manually updates the NuGet package after verification to prevent automatic updates.

### Fallback to raw HTML

Tag helpers are optional. The team can customise standard page layouts and templates directly. Refer to the [layout configuration guide](https://github.com/x-govuk/govuk-frontend-aspnetcore?tab=readme-ov-file#4-configure-your-page-template) for details.

### Known limitations

The team has identified no additional limitations.

## Considered options

* **NPM package usage**
* **NuGet package usage**

## Decision outcome

Chosen option: **NuGet package usage**. This option reduces lines of code. It minimises external build dependencies and satisfies the required technical criteria.

### Consequences

* **Positive:** Reduced lines of code (LOC).
* **Positive:** Fewer external dependencies in the build chain.
* **Negative:** Major point releases lag GDS releases by approximately 6 weeks.
