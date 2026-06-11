---
title: Technical Review Checklist
layout: sub-navigation
sectionKey: Architecture
eleventyNavigation:
  parent: Architecture
  key: Technical Review Checklist
order: 7
---

# Technical Review Checklist

This checklist tracks the project's compliance with government technical review standards, encompassing governance, build and deployment, infrastructure and data architecture, software engineering, security, and monitoring.

## Project Governance
- [x] Branching policy in place and documented [[Branching Strategy](/developers/branching-strategy/)]
- [x] Commits / PR's linked to stories and enforced by policy [[Jira PR Template](../../.github/pull_request_template.md) and [Ways of Working](/developers/ways-of-working/#branching-and-commits)]
- [x] Process for PR review in place and documented [[Ways of Working](/developers/ways-of-working/#pull-requests)]
- [x] Repository access control in place and documented [[Security Architecture](/architecture/security-architecture/#deployment-identity-github-actions-runner)]
- [x] Secure secrets management in place and strategy documented [[Disclosure of Secrets](/operational/008-disclosure-of-secrets/) and [Rotate Secrets](/runbooks/004-rotate-secrets/)]
- [x] \[Optional part of the GOV Technology code of practice\] Code is open sourced. [[LICENCE](../../LICENCE)]
- [x] Contributor and developer guidance, environment setup and build documented [[Getting Started](/developers/getting-started/), [Ways of Working](/developers/ways-of-working/), and [CONTRIBUTING.md](../../CONTRIBUTING.md)]
- [x] Code has appropriate license. [[LICENCE](../../LICENCE)]
- [x] Documentation is stored alongside source code and documentation is complete [[Technical Documentation Home](/)]
- [x] Architecture Decisions records stored and managed correctly [[Architecture Decision Records](/decisions/)]
- [x] Test strategy and plan in place and documented [[Test Strategy](/testing/test-strategy/)]
- [ ] RAID Log in place and correctly managed {Managed externally in project management tools like Jira/Confluence}
- [x] Architecture documentation complete and up-to-date [[Architecture Information](/architecture/)]
- [x] Data Retention strategy in place and documented [[Security Architecture](/architecture/security-architecture/#no-database--zero-persistence-strategy) and 30-day log retention in [Deployment Architecture](/architecture/deployment-architecture/#logging)]
- [x] Disaster Recovery plan in place, tested and documented [[Regional Failover Runbook](/runbooks/006-regional-failover/) and [Regional Failure](/operational/004-regional-failure/)]
- [x] Fully documented release process and path-to-live, can anyone in the team create an audited release [[Release Process](/developers/release-process/)]
- [x] Operational Runbooks fully documented and tested [[Runbooks Index](/runbooks/)]

## Build and Deployment
- [x] Full CI pipeline implemented and builds are predominantly green. [[PR Validation Workflow](../../.github/workflows/workflow-pr.yml)]
- [x] A Continuous Deployment/Delivery strategy in place with a clear path to live documented [[Release Process](/developers/release-process/) and [Deployment Architecture](/architecture/deployment-architecture/)]
- [x] Use of infrastructure as code tooling to deploy infrastructure with code versioned alongside tool [[Terraform Bootstrap](/developers/terraform-bootstrap/) and [infra/terraform/](../../infra/terraform/)]
- [x] Automated unit testing with agreed coverage metrics in place (fails build if not met) [[Build dotnet Workflow](../../.github/workflows/build-dotnet.yml)]
- [x] Automated functional tests with build failing on failure [[Test Strategy](/testing/test-strategy/) and [E2E Project](../../tests/AccessingChildcareEntitlementChecker.Tests.E2e/)]
- [x] Automated dynamic security tests (API/Web spidering) [[ZAP Scanning Guide](/developers/zap-scan-guide/) and [ZAP Report](/testing/zap-report/)]
- [x] Automated static security tests (dependency/package analysis) [[Build Infrastructure Workflow](../../.github/workflows/build-infra.yml) and [Build dotnet Workflow](../../.github/workflows/build-dotnet.yml)]
- [x] Automated cross-browser tests [[Nightly Cross-Browser Tests Workflow](../../.github/workflows/workflow-cross-browser.yml) and [Cross-Browser Testing Strategy](/testing/cross-browser/)]
- [ ] Automated accessibility tests {In progress/planned; documented in [Accessibility Test Plan](/testing/accessibility-test-plan/) with workflow placeholders}
- [ ] Automated performance tests {Planned/defined in [Test Strategy](/testing/test-strategy/#performance--load-testing) but not currently integrated in the CI pipeline}
- [x] Build scripts documented and versioned alongside code [[Workflow Naming Conventions](/developers/workflow-naming-conventions/) and [GitHub Workflows](../../.github/workflows/)]
- [ ] Automated software bill of materials generation {Not currently configured}
- [ ] Are DORA metrics recorded to monitor the software development lifecycle {Not currently tracked}

## Infrastructure Architecture
- [x] Environment strategy in place and fully documented [[Deployment Architecture](/architecture/deployment-architecture/#1-local-development) and [Release Process](/developers/release-process/)]
- [x] Environment access controls policies in place and fully documented [[Security Architecture](/architecture/security-architecture/#cloud-operations--site-reliability-engineer-sre) and [Unauthorised Access](/operational/007-unauthorised-access/)]
- [x] Scalability / Costs and Volumetrics considered and documented [[Non-Functional Requirements](/architecture/non-functional-requirements/) and cost considerations in [Refactoring Options ADR](/decisions/0007-refactoring-options/)]
- [x] Infrastructure naming strategy in place and documented [[infra/terraform/locals.tf](../../infra/terraform/locals.tf)]
- [x] Policy (as code) in place to prevent deployment of unauthorised services or networking [[Test Strategy](/testing/test-strategy/#infrastructure-testing) and [Build Infrastructure Workflow](../../.github/workflows/build-infra.yml)]

## Data Architecture
- [x] Data stores documented and technology choice linked to ADT [[Entitlement Checker Design ADR](/decisions/0001-entitlement-checker-design/) and zero-persistence detailed in [Security Architecture](/architecture/security-architecture/#no-database--zero-persistence-strategy)]
- [x] Data models documented and up-to-date [[Application Architecture](/architecture/application-architecture/) and [DTO and Shared Types Ownership ADR](/decisions/0008-dto-and-shared-types-ownership/)]
- [x] Data flows documented and up-to-date [[Application Architecture](/architecture/application-architecture/), [Deployment Architecture](/architecture/deployment-architecture/), and [Security Architecture](/architecture/security-architecture/#cryptographic-protections)]
- [x] Correct provisioning of storage against projected growth {Not applicable due to zero-persistence user data strategy; RA-GRS deployment storage is detailed in [Deployment Architecture](/architecture/deployment-architecture/#deployed-resources)}
- [x] Backup and Restore runbooks in place, tested and documented [[Regional Failover Runbook](/runbooks/006-regional-failover/) and [Regional Failure](/operational/004-regional-failure/)]

## Software Architecture/Engineering
- [x] Naming conventions agreed and documented [[Ways of Working](/developers/ways-of-working/#branching-and-commits) and [Workflow Naming Conventions](/developers/workflow-naming-conventions/)]
- [x] Comments used appropriately [[Ways of Working](/developers/ways-of-working/#pull-requests) (enforced via PR review process)]
- [x] Code quality evaluated/monitored by a tool (SonarQube,Sentry, Codacy) [[Build dotnet Workflow](../../.github/workflows/build-dotnet.yml)]
- [x] Appropriate error handling and graceful failure strategy (Circuit breakers, bulkheads, retry etc..) [[Application Crashes](/operational/002-application-crashes/), [Security Architecture](/architecture/security-architecture/#stride-assessment--mitigations), and [Third-Party Dependencies](/operational/009-third-party-dependencies/)]
- [x] Structured logging in place with correct context and correlation across all dependencies [[Program.cs](../../src/AccessingChildcareEntitlementChecker.Web/Program.cs) (OpenTelemetry Azure Monitor Integration) and [Deployment Architecture](/architecture/deployment-architecture/#logging)]
- [x] No secrets leaked in logs [[Disclosure of Secrets](/operational/008-disclosure-of-secrets/)]

## Security
- [x] Appropriate HTTP Security Headers [[Program.cs](../../src/AccessingChildcareEntitlementChecker.Web/Program.cs) and [Security Architecture](/architecture/security-architecture/#cryptographic-protections)]
- [x] OWASP Top 10 vunerability scanning (OWASP Zap) [[ZAP Scanning Guide](/developers/zap-scan-guide/) and [ZAP Report](/testing/zap-report/)]
- [x] Data Encrypted at rest [[Security Architecture](/architecture/security-architecture/#no-database--zero-persistence-strategy) and [Deployment Architecture](/architecture/deployment-architecture/#deployed-resources)]
- [x] Data Encrypted in flight [[Security Architecture](/architecture/security-architecture/#cryptographic-protections)]
- [x] Only secure ciphers used for encryption [[Security Architecture](/architecture/security-architecture/#cryptographic-protections)]
- [x] CORS correctly implemented {Not applicable as application is a self-contained same-domain MVC web application with no cross-origin API endpoints; see [Application Architecture](/architecture/application-architecture/)}
- [x] Cookies are set as secure and correct flags are set (HTTPS, HttpOnly, Secure, SameSite) [[Program.cs](../../src/AccessingChildcareEntitlementChecker.Web/Program.cs) and [Security Architecture](/architecture/security-architecture/#stride-assessment--mitigations)]
- [x] Easy to report security issues (security.txt included in the repository/published) [[frontdoor_rules.tf](../../infra/terraform/frontdoor_rules.tf)]
- [x] Threat model created, up-to-date and documented [[Security Architecture - Threat Modelling (STRIDE)](/architecture/security-architecture/#stride-assessment--mitigations)]
- [x] Anti-personas, attack vectors and associated mitigations in place and documented [[Security Architecture - Anti-personas](/architecture/security-architecture/#anti-personas) and [Security Architecture - Attack Vectors](/architecture/security-architecture/#attack-vectors)]

## Monitoring and alerting
- [x] Consolidated logs available in near-realtime [[Deployment Architecture](/architecture/deployment-architecture/#logging)]
- [x] User monitoring through Realtime user monitoring or similar (Google Analytics, DataDog, App insights etc.) [[Program.cs](../../src/AccessingChildcareEntitlementChecker.Web/Program.cs) and [Deployment Architecture](/architecture/deployment-architecture/#logging)]
- [x] Alerting strategy in place [[Operational Resilience Index](/operational/)]
- [x] Alerting triggers implemented, tested and routed to correct stakeholders [[Application Crashes](/operational/002-application-crashes/#detection)]
