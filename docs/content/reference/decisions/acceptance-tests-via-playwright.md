---
title: Acceptance tests in tickets will be implemented via Playwright
layout: sub-navigation
order: 2
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions

---
## Context and problem statement

This ADR operates within standard testing practices, including unit, integration/component, UI, end-to-end (E2E), and manual testing as part of a standard [test pyramid](https://martinfowler.com/articles/practical-test-pyramid.html) model.

Each screen of the eligibility form is delivered via a Jira ticket containing multiple BDD-style acceptance criteria (AC) describing expected UI behavior. For example, in [AC-446](https://dfedigital.atlassian.net/browse/AC-446):

```gherkin
Given the user navigates to the “where do you live” page
When the page loads
Then the page displays the heading “where do you live”, the subtext about country-specific childcare support, and four radio options: England, Scotland, Wales, and Northern Ireland
```

These AC constitute a test script. Without an automated execution mechanism, verification relies on manual testing, which is time-consuming and difficult to reproduce consistently across various browsers and releases.

A repeatable, automated verification mechanism is required to ensure acceptance criteria are satisfied before a ticket is completed.

> [!IMPORTANT]
> This decision concerns **only** how ticket-level acceptance criteria spanning multiple system layers (routing, controller, Razor, validation, and business logic) are verified. It **does not** define standards for unit testing, integration/component testing, end-to-end testing, or manual testing.

## Decision drivers

* **Early Automation:** Integrating automated testing early in the development lifecycle to prevent high retroactive implementation costs.
* **Quality Assurance and Risk Management:** Mitigating the risk of regressions.
* **Cost Efficiency:** Balancing upfront creation costs against long-term maintenance and operational overhead.
* **Developer Experience (DX):** Minimising cognitive load, maintaining test ergonomics, and managing CI/CD build runtimes.

### Test execution speed

A proof of concept demonstrated running tests on a GitHub runner (not deployed to Azure) with an execution speed of approximately 200ms per test. This is a **guideline only** because:

* Test execution duration varies by scope and size.
* Testing may be performed against an active Azure deployment rather than within a runner environment, which may increase execution latency.

### Page and acceptance criteria volume

The system design contains approximately 40 pages, including the expectant parent flow. With an average of 5 acceptance criteria per page, approximately 200 total automated tests are required.

The [GOV.UK Service Manual](https://www.gov.uk/service-manual/technology/designing-for-different-browsers-and-devices) lists 12 supported combinations of browser and operating system. Execution across this full matrix represents approximately 2,400 total test executions.

> [!NOTE]
> This includes testing on Android and iOS platforms, for which a mobile execution strategy is not yet defined.

> [!IMPORTANT]
> This decision applies specifically to ticket-level acceptance criteria. Additional UI or E2E suites may run independently during CI.

## Considered options

Options are categorized across three dimensions:

### Dimension 1: Test type (Method)

* Manual verification.
* Unit, component, or integration testing.
* Playwright user journey testing (coarse-grained E2E journeys).
* Playwright acceptance criteria testing (fine-grained verification per AC).

### Dimension 2: Granularity (Scope)

* Complete execution across the full browser/OS matrix.
* Execution restricted to a single target browser.
* Selected test execution subset.

### Dimension 3: Frequency (Schedule)

* Executed on every push/commit.
* Executed on merge to the `main` branch.
* Executed during deployments to UAT/staging.
* Executed during deployments to production.

## Decision outcome

Acceptance criteria are implemented as Playwright tests using Reqnroll (Gherkin syntax).

Each acceptance criterion is represented by a corresponding Playwright test.

Tests are executed:

* On every Pull Request (PR) push.
* Across the supported browser matrix.

> [!IMPORTANT]
> Testing frequency and browser coverage matrix will be re-evaluated if CI runtime or developer friction exceeds acceptable thresholds. Decisions are subject to review and revision based on operational feedback.

## Consequences

* **Positive:** Acceptance criteria serve as executable technical specifications.
* **Positive:** Ticket-level behaviors are automatically verified.
* **Positive:** Regression test coverage increases systematically as features are developed.
* **Positive:** Test steps can be reused for broader, end-to-end user journeys.
* **Negative:** Browser-based tests are slower than lower-level unit or integration tests.
* **Negative:** Browser-based tests require maintenance when UI elements change.
* **Negative:** CI runtime increases (execution of the full suite across the matrix is estimated at approximately 8 minutes).
* **Negative:** Increases cognitive overhead and test suite size.
* **Negative:** Potential overlap with GDS component tests and custom end-to-end journeys.

## Technical compliance

Compliance is verified during code review. Every pull request must include corresponding automated tests covering the acceptance criteria specified in the associated ticket. UI tests are not measured via standard line-coverage metrics.

## Evaluation of options

### Option A: Manual testing only

* **Positive:** Eliminates upfront automated test development overhead.
* **Negative:** Increases ongoing verification effort and cost.
* **Negative:** Susceptible to human error.
* **Negative:** Repetitive manual task profile.
* **Negative:** Scaling across multiple browser/OS combinations is operationally unfeasible.

### Option B: Lower-level test automation (Unit, component, or integration)

* **Positive:** Guides software architecture and maintains fast execution times.
* **Positive:** Well-suited for business and validation logic.
* **Negative:** Cannot verify cross-stack concerns or end-to-end UI behavior described in acceptance criteria.
* **Negative:** Cannot verify integration between presentation and backend layers.

### Option C: UI-level user journey testing only (Coarse-grained)

* **Positive:** Ensures critical end-to-end user journeys function correctly.
* **Negative:** Individual ticket-level acceptance criteria are not explicitly covered.

### Option D: UI-level acceptance criteria testing (Fine-grained)

* **Positive:** Maps directly to ticket-level acceptance criteria.
* **Negative:** Results in a high volume of small, focused tests.

### Higher granularity of matrix execution

* **Positive:** Maximizes quality assurance and ensures compatibility.
* **Negative:** Overlaps with existing GDS component tests and increases overall execution duration.

### Higher frequency of execution

* **Positive:** Accelerates identification of regression causes.
* **Negative:** Adds execution time to the PR pipeline.
