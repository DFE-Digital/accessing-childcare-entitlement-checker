---
status: proposed
---

# Acceptance Tests in tickets will be implemented via Playwright.

## Context and Problem Statement

This ADR assumes the project follows standard testing practices; e.g. [test pyramid](https://martinfowler.com/articles/practical-test-pyramid.html) with unit, integration/component, ui tests, end to end (full user journey) tests and manual tests.

[!IMPORTANT]
This decision concerns **only** how ticket-level acceptance criteria are verified. It **does not** make any statement on unit testing, integration/component testing, end to end testing, or manual testing.

Each screen of the eligility form is delivered via a Jira ticket. These contain multiple BDD style acceptance criteria (AC) describing expected UI behaviour. For example in [AC-446](https://dfedigital.atlassian.net/browse/AC-446):

```gherkin
Given I have navigated to the “where do you live” page
When the page loads
Then I should see the heading “where do you live”, the subtext about childcare support differing by Country, and four radio button options: England, Scotland, Wales and Northern Ireland
```

These AC constitute a test script. Without an automated mechanism to run them, verification relies on manual testing, which is time-consuming and difficult to repeat across browsers and releases.

Therefore, we need a repeatable automated mechanism to verify that AC are satisified before a ticket is considered complete.

## Decision Drivers

* Prefer automation early - it's difficult to add retrospectively. Scale back later if the costs exceed the benefits.
* Level of quality assurance & risk of change.
* Upfront cost, ongoing and maintenance costs.
* Cognitive burden & ergonomics
  * developer experience (DX) of developing and maintaining tests
  * runtime in CI

### Speed of tests

Using the initial stub tests, we pushed a [proof of concept](https://github.com/DFE-Digital/accessing-childcare-entitlement-checker/actions/runs/23252641658) (n.b. link will expire) of running the tests using a GitHub runner. (i.e. not deployed to Azure) This resulted in it running two small tests in ~400ms; for
an average of 200ms per test. This is a **guideline only** as:

* each test will naturally differ in size and duration.
* we may test on a deployment to Azure rather than within a GitHub runner

Both factors may increase or decrease test duration.

### Number of pages and AC

The current design [Lucid](https://lucid.app/lucidspark/11f9401a-a9db-4a4a-99c9-380cb616e52b/edit?invitationId=inv_2d683b33-11f9-427d-bbab-8dbd10491348&page=0_0#) contains approx. 40 pages inc. expectant parent flow.

The current Jira tickets e.g. [AC-446](https://dfedigital.atlassian.net/browse/AC-446) contain approx. 5
acceptance criteria per page. So we can expect to write somewhere in the region of 200 tests total.

The [service manual](https://www.gov.uk/service-manual/technology/designing-for-different-browsers-and-devices)) describes 12 combinations of browser and operating system, equalling ~2,400 executions for a comprehensive run of acceptance criteria. This includes testing on Android and iOS - although at the moment we don't have an implementation strategy for that.

[!IMPORTANT]
This decision concerns **only** how ticket-level acceptance criteria are verified, and there may be additional UI or E2E tests, which may or may not run during CI.

## Considered Options

Options would lie somewhere in the space described by the following three different dimensions:

### Dimension: test types (how)

* manual tests
* unit tests, component tests, or integration tests
* Playwright tests - covering only user journeys and not individual AC
* Playwright tests - covering each individual AC.

### Dimension: granularity of tests (what)

* All tests (including all browser matrix)
* All tests on one browser
* Some tests

### Dimension: Frequency of tests (when)

* Every change (push)
* Every merge to main
* Every release to UAT/staging environment
* Every release to production environment

## Decision Outcome

Acceptance criteria will be implemented as Playwright tests via Reqnroll. (Gherkin)

Each acceptance criteria will be represented by one Playwright test.

Tests will run:

* on every push to a PR
* across the supported browser matrix

[!IMPORTANT]
We will scale this back if it becomes too detrimental to developer experience; e.g. running tests only on pushes to `main`, or reconsidering the testing approach entirely. No ADR is set in stone and can always be superceded by another one.

### Consequences

#### Positive

* Acceptance criteria become executable specifications.
* Behaviour described in tickets is automatically verified.
* Regression tests accumulate naturally as features are delivered
* Test steps can be reused when writing end-to-end full user journey tests

#### Negatives

* Browser tests are slower than lower-level tests.
* Browser tests are more fragile than lower-level tests. Tests may require maintenence when UI changes.
* CI runtime will increase - if we run the full test suite that would be `2400*200ms = 8 minutes`
* Large test suite - possible cognitive burden
* Tests may overlap with:
  * GDS component tests
  * end to end/full user journey tests

### Confirmation

Compliance is via code review; checking that the implementation includes a test covering any AC specified in the ticket.

This is because UI tests are not easily or conventionally gated via coverage metrics.

## Pros and Cons of the Options

### Manual Testing

Manual testing would involve having the project available in some environment (probably the dev or UAT/staging environment) and running through each AC.

* Good, because we don't have to write automated tests, which takes upfront time.
* Bad, because manual tests takes ongoing time.
* Bad, because manual testing is error prone.
* Bad, because manual testing is boring. (bad DX)
* Bad, because depending on the other dimensions, we may need to test repeatedly many times across many browsers.

### unit tests, component tests, or integration tests

Unit testing

* Good, because it helps inform the code architecture and style.
* Good, because they are fast.
* Good, because they are excellent for covering business logic.
* Bad, because they don't cover the full stack. I can't see how you could write unit tests that cover the AC.
* Bad, because they don't test integration between backend and frontend.

### Playwright tests - covering only user journeys and not individual AC

* Good, because it ensures the full end to end user journey works
* Bad, because the AC doesn't describe a full user journey test - these types of tests can't cover the AC

### Playwright tests - covering each individual AC.

* Good, because they can trivially cover the AC
* Bad, because it's a lot of small tests

### Higher Granularity

* Good, because the more granular tests we run, the better the quality assurance. We also need to cover the granularity of the AC
* Neutral, because we may be tested some elements that have already been tested. (GDS components)
* Bad, because the more tests we run, the slower a full test run is
* Bad, because the more tests we have, the more they need correcting when a change occurs

### Higher Frequency

* Good, because the more often we run tests, the easier it is to see which change caused a regression
* Good, because running the suite is anticipated to take around 10 minutes.
* Bad, because this adds ten minutes to CI/CD.
