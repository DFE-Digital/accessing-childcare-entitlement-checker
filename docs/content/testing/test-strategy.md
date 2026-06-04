---
title: Test Strategy
layout: sub-navigation
sectionKey: Testing
eleventyNavigation:
  parent: Testing
  key: Test Strategy
order: 0
---

This document outlines the testing strategy for the Accessing Childcare Entitlement Checker. It defines the types of testing performed, the tools used, and the lifecycle of testing within our CI/CD pipelines.

## Testing Principles

Our strategy is guided by the following principles (as defined in [Constraints and Principles](../architecture/constraints-principles.md)):

* Meets the GDS Service Standard - Ensuring the service is accessible, secure, and reliable.
* Infrastructure as Code & CI/CT/CD - Continuous testing is integrated into our deployment pipelines.
* IT Health Checked - Regular security assessments and health checks.
* Automation First - We automate acceptance criteria early to ensure repeatable quality and prevent regressions.

## The Testing Pyramid (Layered Strategy)

The testing strategy is organised into discrete layers. Each layer builds upon the one below it, increasing in integration and complexity while decreasing in total volume and execution speed.

| Testing Type            |   Visual Volume    |
|:------------------------|:------------------:|
| E2E / BDD User Journeys | `[      #      ]`  |
| Accessibility Testing   | `[     ###     ]`  |
| Performance Testing     | `[    #####    ]`  |
| DAST Security Scans     | `[   #######   ]`  |
| Integration Testing     | `[  #########  ]`  |
| Component Testing       | `[ ########### ]`  |
| Mutation Testing        | `[#############]`  |
| Unit Testing            | `[#############]`  |
| Infrastructure & IaC    | `[#############]`  |

## Testing Types

### Unit Testing & Mutation Testing
* Purpose: Validates individual components in isolation and measures test effectiveness.
* Tools: [xUnit](https://xunit.net/), [NSubstitute](https://nsubstitute.github.io/), [Stryker.NET](https://stryker-mutator.io/).
* Execution: Unit tests run on every push to a Pull Request. Stryker runs periodically/manually to validate suite depth (not yet integrated into every PR).

### Component/Integration Testing
* Purpose: Verifies the interaction between multiple layers (Routing, Controllers, Razor views, Validation) without requiring a full browser.
* Tools: xUnit, `Microsoft.AspNetCore.Mvc.Testing`.
* Execution: Runs on every push to a Pull Request.

### End-to-End (E2E) & Accessibility (A11y) Testing
* Purpose: Verifies individual Acceptance Criteria (AC) and full user journeys, while ensuring WCAG 2.2 AA compliance.
* Tools: [Reqnroll](https://reqnroll.net/), [Playwright](https://playwright.dev/), [axe-core](https://github.com/dequelabs/axe-core).
* Execution: Runs on every push to a Pull Request. Currently runs against **Chromium**; additional browsers and automated accessibility scans are being integrated into the pipeline.

### Security Testing (DAST & IaC)
* Purpose: Identifies vulnerabilities in the running application and misconfigurations in infrastructure code.
* Tools: [OWASP ZAP](https://www.zaproxy.org/), [Checkov](https://www.checkov.io/).
* Execution: ZAP runs weekly against the Test environment. Checkov runs on every PR affecting Terraform files.

### Performance & Load Testing
* Priority: High.
* Tools: [Azure Load Testing](https://learn.microsoft.com/en-us/azure/load-testing/).
* Goal: Ensure the application meets Non-Functional Requirements (NFRs) for response times and concurrent user handling under production-like conditions.

### Infrastructure Testing
* Purpose: Validates Terraform configurations for security best practices and compliance.
* Tools: Checkov.
* Execution: Integrated into the `infra-deploy.yml` workflow and PR checks.

## Summary of Tooling

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

## Test Environments

| Environment  | Purpose                         | Testing Performed                     |
|:------------:|:--------------------------------|:--------------------------------------|
|    Local     | Developer inner loop            | Unit, Component, E2E, A11y, IaC       |
| CI (GitHub)  | PR Validation & Release Process | Unit, Component, IaC, E2E (on runner) |
|     Test     | Integration / DAST              | ZAP Scans, Manual QA, Load Testing    |
|   Staging    | Pre-production Validation       | E2E, A11y                             |
|  Production  | Live Service                    | Synthetic monitoring                  |

