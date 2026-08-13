---
title: Working with browser back/forward Cache (BFCache) behaviour
layout: sub-navigation
order: 12
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions
---

## Context

The service provides a multi-step form journey featuring:

* Multiple pages and branching paths.
* A final result summary page.
* Journeys where answers can be reviewed and changed before completion.

There are two key points in the journey where answers can be reviewed:

1. Children summary
2. Check your answers

These pages function as review points where entered information is validated and modified before proceeding. Due to the stateful nature of the journey, the backend application state and the browser-displayed state can diverge.

Example scenario:

1. A user completes a portion of the form.
2. The browser caches the page in its Back/Forward Cache (BFCache).
3. The user navigates backward.
4. The browser restores the previous page from BFCache.
5. Restored values may no longer align with the server-side state.
6. The user continues forward using a page displaying stale information.

This results in a potential mismatch between:

* **Browser state:** The view presented to the user.
* **Application state:** The record maintained by the backend.

## Problem statement

The browser BFCache is controlled by the client browser and cannot be reliably managed by the server application. While some approaches appear to provide control over caching behaviour, they do not offer a robust solution:

### HTTP cache control headers

HTTP caching directives (such as `Cache-Control: no-store` or `no-cache`) are often assumed to prevent pages from being stored in or restored from the BFCache. However, BFCache is a separate, memory-based browser mechanism designed for instant back/forward navigation (preserving the live JavaScript heap and DOM state) and does not function like the standard HTTP disk or memory cache.

Historically, some browsers treated `Cache-Control: no-store` (CCNS) as a signal to exclude a page from BFCache. However:
* The HTML specification does not require `Cache-Control` headers to block BFCache.
* Modern browsers prioritise user experience and performance.
* Google Chrome allows caching pages with `Cache-Control: no-store` under specific safety heuristics. These heuristics include checking for cookie modifications, active connections, or sensitive subresource fetches.
* Other browsers handle CCNS differently or block BFCache entirely, leading to inconsistent behavior across platforms.

Consequently, HTTP caching headers are not a guaranteed mechanism to prevent pages from being restored from BFCache. Relying on them to force page reloads or manage state drift is an anti-pattern.

### Client-side lifecycle handling

Client-side scripts can detect some navigation events and attempt to refresh or invalidate state. However, because there is no fallback when JavaScript is disabled, client-side script dependency is not a viable option.

### Conclusion

No reliable mechanism exists to prevent BFCache usage or guarantee consistent behavior across all browsers. The application must operate alongside BFCache behavior rather than attempting to control or disable it.

## Decision

The application treats BFCache restoration as standard browser behavior. Pages representing state boundaries must explicitly validate state transitions rather than assuming browser page state is current.

The required architectural modifications are:

* Restructure the Children Summary and Check Your Answers pages to require a POST request to proceed.
* Utilise the POST request to validate the current session state on the backend before continuing the journey.

This ensures that navigation from these pages becomes a server-validated transition rather than a continuation based solely on potentially stale browser state.

**Option 3: State Mismatch Error and Restart** is the selected implementation approach. Upon detecting a token mismatch during a boundary POST request, the event is logged for telemetry and a custom 400 Bad Request page is returned, requiring the journey to be restarted.

## Options for resolving state drift

Once a drift between browser state and backend state is detected, three potential approaches exist:

### Option 1: Last Write Wins

Values currently displayed to the user are treated as authoritative.

If browser and backend states differ:

* The values shown on the review page are treated as the intended user input.
* The backend state is updated to match the submitted values.
* The journey continues without interruption.

#### Behaviour

Example:

1. Backend state:
    * Child name: "Alice"
2. Browser BFCache restores an older page:
    * Child name: "Alicia"
3. The user continues.

Result: "Alicia" becomes the accepted value.

#### Advantages

* Provides a seamless user experience.
* Avoids interrupting the journey with conflict handling.
* Aligns with the expectation that the active page represents user intent.

#### Considerations

* Backend state may be silently overwritten.
* The application accepts browser state as the source of truth.
* Newer server-side changes may be overwritten without user awareness.

### Option 2: State Wins

The backend state is treated as authoritative. If the browser state differs from the backend state:

* The drift is detected when attempting to continue.
* The user is notified that the displayed answers are no longer current.
* The current backend values are displayed.
* The user reviews and continues with the correct state.

#### Behaviour

Example:

1. Backend state:
    * Child name: "Alice"
2. Browser BFCache restores an older page:
    * Child name: "Alicia"
3. The user continues.

Result:
* The application detects the mismatch.
* The user is notified of the mismatch.
* "Alice" is shown as the current value.

#### Advantages

* Backend state remains the single source of truth.
* Prevents accidental overwriting of newer data.
* Makes state conflicts explicit.

#### Considerations

* Introduces additional user interaction.
* Changes to viewed values may cause user confusion.
* Requires conflict detection and presentation logic.

### Option 3: State Mismatch Error and Restart (Chosen)

The backend state is treated as authoritative. Rather than attempting reconciliation or automatic refreshes, state drift is treated as session invalidation.

If browser and backend states differ (detected via a mismatched `CorrelationId` token):

* The system logs an explicit structured warning for telemetry and alerting.
* The system redirects the user to a custom 400 Bad Request error page explaining that a session error occurred.
* The page provides a "Start again" button link to restart the journey from the beginning.

#### Behaviour

Example:

1. Backend state:
    * Child name: "Alice"
2. Browser BFCache restores an older page:
    * Child name: "Alicia"
3. The user continues.

Result:
* The application detects the mismatched correlation token.
* A 400 Bad Request page is displayed with a session error message and a "Start again" option.

#### Advantages

* Simple and robust backend state management—no complex conflict-handling logic required.
* Predictable behavior that eliminates subtle drift edge cases.
* Easy telemetry integration (logs, counts) to measure how often BFCache drift actually occurs.

#### Considerations

* Hard friction for users encountering the issue, forcing them to start the form over (mitigated by the low frequency of BFCache drift in standard user paths).

## Consequences

* **Positive:** The application no longer depends on controlling browser cache behaviour.
* **Positive:** State validation occurs at meaningful transition points.
* **Positive:** Browser navigation behaviour becomes an expected part of the design rather than an exceptional case.
* **Negative:** Review pages require additional server-side validation.
* **Negative:** The application must define how conflicting state is resolved.
* **Negative:** Additional user experience considerations are required when state drift is detected.

## Alternatives considered

### Attempt to disable BFCache

Rejected because browsers do not provide a reliable, application-controlled mechanism to disable BFCache behaviour.

### Force page reloads on browser navigation

Rejected because this approach:

* Relies on client-side behavior.
* Is not consistently executed after BFCache restoration.
* Adds unnecessary complexity and dependencies.

### Ignore BFCache behaviour

Rejected because state drift can result in inconsistent or unexpected user journeys.

## Open questions

1. Which state should be considered authoritative when browser state and backend state differ?
    * Last Write Wins
    * State Wins
    * **State Mismatch Error and Restart (Chosen)**

2. Are there additional pages besides Children summary and Check your answers that represent state boundaries and require validation?

3. What level of conflict detection is required?
    * Full state comparison
    * Page-specific fields only
    * **Version/token-based detection (Chosen)**
