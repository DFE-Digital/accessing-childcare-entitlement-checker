---
title: Working with browser back/dorward Cache (BFCache) behaviour
layout: page
showPagination: true
order: 12
sectionKey: Decisions
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions
---

## Context

Our service provides a multi-step form journey with:

* Multiple pages and branching paths.
* A final result summary page.
* User journeys where answers can be reviewed and changed before completion.

There are two key points in the journey where users can review their answers:

1. Children summary
2. Check your answers

These pages act as review points where users can validate the information they have entered and make changes before proceeding. Because the journey is stateful, the application state maintained by the backend and the state displayed by the browser can diverge.

A common example:

1. A user completes part of the form.
2. The browser stores a page in its Back/Forward Cache (BFCache).
3. The user navigates backwards.
4. The browser restores the previous page from BFCache.
5. The user sees values that may no longer match the current server-side state.
6. The user continues forward using a page that may contain stale information.

This creates a potential mismatch between:

* Browser state - what the user is currently viewing.
* Application state - what the backend currently considers the user's answers to be.

## Problem

The browser BFCache is controlled by the browser and cannot be reliably managed by the application. There are several approaches that appear to provide control over caching behaviour, but they do not provide a robust solution:

### HTTP cache control headers

HTTP caching directives (for example `Cache-Control`) influence normal HTTP caching behaviour.  However, BFCache is a separate browser mechanism used for fast back/forward navigation and does not consistently respect these directives. Therefore, HTTP caching headers cannot be relied upon to prevent pages being restored from BFCache.

### Client-side lifecycle handling

Client-side scripts can detect some navigation events and attempt to refresh or invalidate state.

However:

* Browsers do not always re-execute JavaScript when restoring a page from BFCache.
* Behaviour varies between browsers.
* This introduces an additional client-side dependency for correctness.

Therefore, client-side handling cannot provide a guaranteed solution.

### Conclusion

There is no reliable mechanism to prevent BFCache usage or force consistent BFCache behaviour. The application must therefore work with BFCache behaviour rather than attempting to control or disable it.

## Decision

The application will treat BFCache restoration as an expected browser behaviour. Pages that represent important state boundaries must explicitly validate state transitions rather than relying on the browser page state being current.

The minimum required change is:

* Rework Children summary and Check your answers pages so that proceeding onwards requires a POST request.
* The POST request will provide an opportunity for the backend to validate the current state before continuing the journey.

This ensures that navigation from these pages becomes a server-validated transition rather than a continuation based solely on potentially stale browser state.

## Options for Resolving State Drift

Once a drift between browser state and backend state is detected, there are two possible approaches.

### Option 1: Last Write Wins

The values currently displayed to the user are considered authoritative.

If the browser state differs from the backend state:

* The values shown on the review page are treated as the user's intended values.
* The backend state is updated to match the values submitted from the page.
* The user continues without interruption.

#### Behaviour

Example:

1. Backend state:

    * Child name: "Alice"

2. Browser BFCache restores an older page:

    * Child name: "Alicia"

3. User continues.

Result:

* "Alicia" becomes the accepted value.

#### Advantages

* Provides a seamless user experience.
* Avoids interrupting the user with conflict handling.
* Matches the common expectation that the page currently being interacted with represents user intent.

#### Considerations

* Backend state may be silently overwritten.
* The application accepts that browser state can become the source of truth.
* Users may unknowingly overwrite newer server-side changes.

### Option 2: State Wins

The backend state is considered authoritative. If the browser state differs from the backend state:

* The drift is detected when the user attempts to continue.
* The user is informed that their displayed answers are no longer current.
* The current backend values are displayed.
* The user can review and continue with the correct state.

#### Behaviour

Example:

1. Backend state:

    * Child name: "Alice"

2. Browser BFCache restores an older page:

    * Child name: "Alicia"

3. User continues.

Result:

* The application detects the mismatch.
* The user is notified.
* "Alice" is shown as the current value.

#### Advantages

* Backend state remains the single source of truth.
* Prevents accidental overwriting of newer data.
* Makes state conflicts explicit.

#### Considerations

* Introduces additional user interaction.
* Users may be surprised by changes to values they are currently viewing.
* Requires conflict detection and presentation logic.

## Consequences

### Positive

* The application no longer depends on controlling browser cache behaviour.
* State validation occurs at meaningful transition points.
* Browser navigation behaviour becomes an expected part of the design rather than an exceptional case.

### Negative

* Review pages require additional server-side validation.
* The application must define how conflicting state is resolved.
* Additional user experience considerations are required when state drift is detected.

## Alternatives Considered

### Attempt to disable BFCache

Rejected because browsers do not provide a reliable application-controlled mechanism to disable BFCache behaviour.

### Force page reloads on browser navigation

Rejected because:

* It relies on client-side behaviour.
* It is not consistently executed after BFCache restoration.
* It adds unnecessary complexity and dependency.

### Ignore BFCache behaviour

Rejected because state drift can result in inconsistent or unexpected user journeys.

## Open Questions

1. Which state should be considered authoritative when browser state and backend state differ?

    * Last Write Wins
    * State Wins

2. Are there additional pages besides Children summary and Check your answers that represent state boundaries and require validation?

3. What level of conflict detection is required?

    * Full state comparison
    * Page-specific fields only
    * Version/token-based detection
