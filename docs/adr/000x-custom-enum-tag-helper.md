# Create a custom tag helper for govuk-frontend-aspnetcore enums

## Context and Problem Statement

The `govuk-frontend-aspnetcore` library allows using a `for` attribute for binding fields; but the `...radios` component doesn't enumerate the enum members; instead these must be written out manually.

This adds a fair bit of repetitive code to the Razor pages; and there are approx 25/40 pages in the prototype that contain radio buttons. Reducing this repetitive code will increase development velocity and reduce maintenance time.

## Decision Drivers

* Ease and speed of development.
* Ease of maintenance.
* Flexibility for future change.

## Considered Options

* Writing out the Razor for each page. (baseline)
* Adding attributes to the viewmodels; and using a custom tag helper to emit the razor.

## Decision Outcome

Use a Razor tag helper for emitting enums.

## Pros and cons of the options

### Writing out the Razor for each page

#### Pros

* Clarity - the structure of the Razor reads a bit closer to the emitted HTML.
* Flexibility with similarity - the layout is flexible; while still being the same for each item.

#### Cons

* More code to write; which feeds into longer dev times and higher maintenance cost.

### Using a custom tag helper

#### Pros

* Less code (speed of development) - it's likely faster to write a single tag helper in the Razor, and add attributes to the enum & view model
* Less maintenance - enforces similarity of emitted HTML; which reduces the risk of bugs.
* Locality of data and separation of responsibilities - moves the display names closer to the view model.
* Still flexible - we can always revert to the underlying `govuk-frontend-aspnetcore` tag helpers if needed for a particular page or question.

#### Cons

* Adds an extra training burden when onboarding a new developer. Note that since the project already includes `govuk-frontend-aspnetcore`; any new developer will need to understand the concept of tag helpers.
* Requires maintaining the tag helper
* Although flexible; if we encounter a circumstance where we can't use the tag helper; we'd need to either modify it or revert to the previous pattern; and this page would then be dissimilar to the other pages.

### Consequences

* Good, because less code to write.
* Good, because easier to maintain.
* Bad, because means building a custom tag helper.
