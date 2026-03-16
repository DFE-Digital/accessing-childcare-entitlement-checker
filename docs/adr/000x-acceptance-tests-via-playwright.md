---
status: proposed
---

# Acceptance Tests in tickets will be implemented via Playwright.

## Context and Problem Statement

This ADR assumes the project follows standard testing practices; e.g. [test pyramid](https://martinfowler.com/articles/practical-test-pyramid.html), unit, integration and end to end tests.

This decision concerns **only** how ticket-level acceptance criteria are verified.

Each screen of the eligility form is delivered via a Jira ticket. These contain multiple BDD style acceptance criteria (AC) describing expected UI behaviour. For example in [AC-446](https://dfedigital.atlassian.net/browse/AC-446):

```
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
* Cognitive burden & ergonomics - developer experience. (DX)

## Considered Options

Options would lie somewhere in the space described by the following three different dimensions:

Dimension: test types (how)

* manual tests
* unit tests, component tests, or integration tests
* Playwright tests - covering only user journeys and not individual AC
* Playwright tests - covering each individual AC.

Dimension: granularity of tests (what)
* All tests (including all browser matrix)
* All tests on one browser
* Some tests

Dimension: Frequency of tests (when)
* Every change (push)
* Every merge to main
* Every release to UAT/staging environment
* Every release to production environment

## Decision Outcome

Acceptance criteria will be implemented as Playwright tests via Reqnroll. (Gherkin)

Each acceptance criteria will be represented by one Playwright test.

Tests will run:

- on every push
- across the supported browser matrix

### Consequences

#### Positive

- Acceptance criteria become executable specifications.
- Behaviour described in tickets is automatically verified.
- Regression tests accumulate naturally as features are delivered
- Test steps can be reused when writing end-to-end full user journey tests

#### Negatives

- Browser tests are slower than lower-level tests.
- Tests may require maintenence when UI changes.
- CI runtime will increase

### Confirmation

Compliance is via code review; checking that the implementation includes a test covering any AC specified in the ticket.

## Pros and Cons of the Options

### Manual Testing

Manual testing would involve having the project available in some environment (probably the dev or UAT/staging environment) and running through each AC.

* Good, because we don't have to write automated tests, which takes upfront time.
* Bad, because manual tests takes ongoing time.
* Bad, because manual testing is error prone.
* Bad, because manual testing is boring. (bad DX)
* Bad, because depending on the other dimensions, we may need to test repeatedly many times across many browsers.

If we have 5 tests per page, and 40 pages; (see [Lucid](https://lucid.app/lucidspark/11f9401a-a9db-4a4a-99c9-380cb616e52b/edit?invitationId=inv_2d683b33-11f9-427d-bbab-8dbd10491348&page=0_0#) inc. expectant parent flow) and then twelve browsers (see [service manual](https://www.gov.uk/service-manual/technology/designing-for-different-browsers-and-devices)) then that totals 2,400 individual acceptance tests. I don't think it's practically possible to do this manually, which means **some elements in the ticket will not be tested to the required standard**.

That's fine if we judge the risk of a failure to be low; but it's worth noting explicitly.

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
* Bad, because running the tests takes some _unknown_ amount of time in CI/CD.
