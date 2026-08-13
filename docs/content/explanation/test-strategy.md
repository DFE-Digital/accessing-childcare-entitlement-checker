---
title: Test strategy
layout: sub-navigation
sectionKey: Explanation
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Explanation
  key: Test strategy
order: 5
---
This guide shows the testing strategy for the Accessing Childcare Entitlement Checker. It shows the types of tests we run, the tools we use, and the test lifecycle within our CI/CD pipelines.

## Testing principles

We base the test strategy on core engineering principles. These principles make sure that the application is accessible, reliable, and secure:

* **Follow GDS standards**: We structure test validation to make sure that the service is highly accessible, secure, and stable.
* **Continuous Integration and Deployment (CI/CD)**: We put automatic tests in our deployment pipelines to validate all changes.
* **Complete health audits**: We run automatic security checks and professional IT health checks regularly.
* **Use automatic tests first**: We write automatic tests early in the delivery cycle. This keeps quality high and prevents errors.

## The testing pyramid (layered strategy)

The test ecosystem uses a pyramid with multiple layers. Each layer shows a validation boundary. This balances execution speed and isolation with integration depth and realistic user environments.

| Testing Type            |   Visual Volume    |
|:------------------------|:------------------:|
| E2E / BDD User Journeys | `[      #      ]`  |
| Accessibility Testing   | `[     ###     ]`  |
| Performance Testing     | `[    #####    ]`  |
| DAST Security Scans     | `[   #######   ]`  |
| Integration Testing     | `[  =========  ]`  |
| Component Testing       | `[ =========== ]`  |
| Mutation Testing        | `[=============]`  |
| Unit Testing            | `[=============]`  |
| Infrastructure & IaC    | `[=============]`  |

## Core validation layers

We divide the application verification into specific technical testing scopes. Each scope tests a different part of the system.

### Unit testing & mutation testing
* **Concept focus**: We validate single classes, helper methods, and rules in isolation. We also use mutation tests to check the test suite depth and quality.
* **Tools**: [xUnit](https://xunit.net/), [NSubstitute](https://nsubstitute.github.io/), and [Stryker.NET](https://stryker-mutator.io/).
* **Integration**: We run unit tests automatically during the CI flow for each pull request. We run mutation tests manually to find untested paths and logical gaps.

### Component & integration testing
* **Concept focus**: We test the connection between coupled layers (such as controllers, validation, routing, and views). We do this without a browser.
* **Tools**: xUnit and `Microsoft.AspNetCore.Mvc.Testing` (this provides an in-memory test host).
* **Integration**: We run these tests automatically on each pull request in the primary CI pipeline.

### End-to-end (E2E) & accessibility (A11y) testing
* **Concept focus**: We verify that full user journeys comply with the Acceptance Criteria (AC). We also make sure the interface follows WCAG 2.2 AA standards.
* **Tools**: [Reqnroll](https://reqnroll.net/) (for behaviour-driven development specification), [Playwright](https://playwright.dev/) (for browser automation), and [axe-core](https://github.com/dequelabs/axe-core) (for programmatic accessibility evaluation).
* **Integration**: The CI pipeline starts these tests during pull request verification. We target Chromium first. We plan to add other browsers later.

### Security testing (DAST & IaC)
* **Concept focus**: We find security risks in the application. We also find security misconfigurations in our infrastructure files.
* **Tools**: [OWASP ZAP](https://www.zaproxy.org/) and [Checkov](https://www.checkov.io/).
* **Integration**: OWASP ZAP scans run weekly against the Test environment. Checkov checks run on pull requests that change Terraform files.

### Performance & load testing
* **Concept focus**: We make sure that the application meets Non-Functional Requirements (NFRs), such as fast response times and stable concurrent user handling.
* **Tools**: [Azure Load Testing](https://learn.microsoft.com/en-us/azure/load-testing/).
* **Integration**: We run load tests before we release code to production. This makes sure the application is ready.

### Infrastructure validation
* **Concept focus**: We make sure that all Terraform Azure resources comply with cloud security rules and compliance requirements.
* **Tools**: Checkov.
* **Integration**: We run these checks directly in PR workflows to find issues early.

## Summary of tooling

| Type          | Tool                  | Framework                 |
|:--------------|:----------------------|:--------------------------|
| Unit          | xUnit                 | .NET 10                   |
| Mutation      | Stryker.NET           | .NET                      |
| Mocking       | NSubstitute           | .NET                      |
| E2E / BDD     | Reqnroll + Playwright | .NET                      |
| Accessibility | axe-core              | Integrated in E2E         |
| Performance   | Azure Load Testing    | JMeter / Azure            |
| DAST Security | OWASP ZAP             | Docker / Automation Plan  |
| IaC Security  | Checkov               | Terraform Static Analysis |
| Coverage      | Coverlet / SonarQube  | CI Pipeline               |

## Test environments

| Environment | Purpose                         | Testing Performed                     |
|:------------|:--------------------------------|:--------------------------------------|
| Local       | Developer inner loop            | Unit, Component, E2E, A11y, IaC       |
| CI (GitHub) | PR Validation & Release Process | Unit, Component, IaC, E2E (on runner) |
| Test        | Integration / DAST              | ZAP Scans, Manual QA, Load Testing    |
| Staging     | Pre-production Validation       | E2E, A11y                             |
| Production  | Live Service                    | Synthetic monitoring                  |
