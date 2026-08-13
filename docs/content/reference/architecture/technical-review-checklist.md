---
title: Technical review checklist
layout: sub-navigation
sectionKey: Reference
order: 8
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Architecture
  key: Technical review checklist
---
Compliance with government technical review standards is tracked via this checklist. It encompasses governance, build and deployment, infrastructure and data architecture, software engineering, security, and monitoring.

## Project governance

| Requirement / Standard                                              | Status | Reference / Action                                                                                                                                                                                               |
|:--------------------------------------------------------------------|:------:|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Branching policy in place and documented                            |   ✅    | [Branching Strategy](/explanation/branching-strategy/)                                                                                                                                                         |
| Commits / PRs linked to stories and enforced by policy              |   ✅    | Referenced in the PR Template and [Ways of Working](/explanation/ways-of-working/#branching-and-commits)                                                                                                                     |
| Process for PR review in place and documented                       |   ✅    | [Ways of Working](/explanation/ways-of-working/#pull-requests)                                                                                                                                                 |
| Repository access control in place and documented                   |   ✅    | [Security Architecture](/reference/architecture/security-architecture/#deployment-identity-github-actions-runner)                                                                                                       |
| Secure secrets management in place and strategy documented          |   ✅    | [Disclosure of Secrets](/reference/operational/disclosure-of-secrets/) and [Rotate Secrets](/how-to/runbooks/rotate-secrets/)                                                                                             |
| Code is open sourced                                                |   ✅    | Refer to the repository license file                                                                                                                                                                                                 |
| Contributor guidance, local environment setup, and build documented |   ✅    | [Getting Started](/tutorials/getting-started/), [Ways of Working](/explanation/ways-of-working/), and [CONTRIBUTING.md](../../../CONTRIBUTING.md)                                                             |
| Code has an appropriate open-source license                         |   ✅    | Refer to the repository license file                                                                                                                                                                                                 |
| Documentation is stored alongside source code and is complete       |   ✅    | [Technical Documentation](/)                                                                                                                                                                                     |
| Architecture Decision Records (ADRs) are managed correctly          |   ✅    | [Architecture Decision Records](/reference/decisions/)                                                                                                                                                                     |
| Test strategy and plan in place and documented                      |   ✅    | [Test Strategy](/explanation/test-strategy/)                                                                                                                                                                         |
| RAID Log in place and correctly managed                             |   ⏳    | Managed externally in project management tools (Jira/Confluence)                                                                                                                                                 |
| Architecture documentation is complete and up-to-date               |   ✅    | [Architecture Information](/reference/architecture/)                                                                                                                                                                       |
| Data Retention strategy in place and documented                     |   ✅    | [Security Architecture](/reference/architecture/security-architecture/#no-database--zero-persistence-strategy) and 30-day log retention in [Deployment Architecture](/reference/architecture/deployment-architecture/#logging) |
| Disaster Recovery plan in place, tested, and documented             |   ✅    | [Regional Failover Runbook](/how-to/runbooks/regional-failover/) and [Regional Failure](/reference/operational/regional-failure/)                                                                                         |
| Fully documented release process and path-to-live                   |   ✅    | [Release Process](/how-to/release-process/)                                                                                                                                                               |
| Operational Runbooks are fully documented and tested                |   ✅    | [Runbooks Index](/how-to/runbooks/)                                                                                                                                                                                     |

## Build and deployment

| Requirement / Standard                                        | Status | Reference / Action                                                                                                                    |
|:--------------------------------------------------------------|:------:|:--------------------------------------------------------------------------------------------------------------------------------------|
| Full CI pipeline implemented (builds predominantly green)     |   ✅    | PR Validation Workflow                                                                                                                |
| CD strategy in place with a documented path-to-live           |   ✅    | [Release Process](/how-to/release-process/) and [Deployment Architecture](/reference/architecture/deployment-architecture/)           |
| Infrastructure as Code (IaC) tooling versioned alongside code |   ✅    | [Terraform Bootstrap](/how-to/terraform-bootstrap/) and the infra directory                                                       |
| Automated unit testing with agreed coverage metrics           |   ✅    | Executed in the dotnet build workflow                                                                                                             |
| Automated functional/E2E tests with build-fail validation     |   ✅    | [Test Strategy](/explanation/test-strategy/) and E2E test project                                                                         |
| Automated dynamic security tests (API/Web spidering/DAST)     |   ✅    | [ZAP Scanning Guide](/how-to/zap-scan-guide/) and [ZAP Report](/reference/testing/zap-report/)                                           |
| Automated static security tests (dependency/SAST analysis)    |   ✅    | Executed in the infrastructure build and dotnet build workflows                                                                           |
| Automated cross-browser tests                                 |   ✅    | Configured in the nightly cross-browser tests workflow and [Cross-Browser Testing Strategy](/reference/testing/cross-browser/)                                |
| Automated accessibility tests                                 |   ⏳    | In progress/planned; documented in [Accessibility Test Plan](/reference/testing/accessibility-test-plan/) with workflow placeholders            |
| Automated performance tests                                   |   ⏳    | Planned/defined in [Test Strategy](/explanation/test-strategy/#performance--load-testing) but not currently integrated in the CI pipeline |
| Build scripts documented and versioned alongside code         |   ✅    | [Workflow Naming Conventions](/reference/workflow-naming-conventions/)                                                            |
| Automated Software Bill of Materials (SBOM) generation        |   ⏳    | Not currently configured                                                                                                              |
| DORA metrics recorded to monitor development lifecycle        |   ⏳    | Not currently tracked                                                                                                                 |


## Infrastructure architecture

| Requirement / Standard                                        | Status | Reference / Action                                                                                                                                                                |
|:--------------------------------------------------------------|:------:|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Environment strategy in place and fully documented            |   ✅    | [Deployment Architecture](/reference/architecture/deployment-architecture/#1-local-development) and [Release Process](/how-to/release-process/)                                   |
| Environment access control policies in place and documented   |   ✅    | [Security Architecture](/reference/architecture/security-architecture/#cloud-operations--site-reliability-engineer-sre) and [Unauthorised Access](/reference/operational/unauthorised-access/) |
| Scalability, costs, and volumetrics considered and documented |   ✅    | [Non-Functional Requirements](/reference/architecture/non-functional-requirements/)                                                                                                      |
| Infrastructure naming strategy in place and documented        |   ✅    | [infra/terraform/locals.tf](../../../infra/terraform/locals.tf)                                                                                                                      |
| Policy as Code to prevent unauthorized service provisioning   |   ✅    | [Test Strategy](/explanation/test-strategy/#infrastructure-testing) and the infrastructure build workflow                                                                             |

## Software architecture/engineering

| Requirement / Standard                                       | Status | Reference / Action                                                                                                                                                                                                                        |
|:-------------------------------------------------------------|:------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Code naming and style conventions agreed and documented      |   ✅    | [Ways of Working](/explanation/ways-of-working/#branching-and-commits) and [Workflow Naming Conventions](/reference/workflow-naming-conventions/)                                                                                   |
| Code comments used appropriately                             |   ✅    | [Ways of Working](/explanation/ways-of-working/#pull-requests) (enforced via PR reviews)                                                                                                                                                |
| Code quality monitored automatically by a static tool        |   ✅    | SonarCloud integrated in build dotnet Workflow                                                                                                                                                                                            |
| Appropriate error handling and graceful failure strategy     |   ✅    | [Application Crashes](/reference/operational/application-crashes/), [Security Architecture](/reference/architecture/security-architecture/#stride-assessment--mitigations), and [Third-Party Dependencies](/reference/operational/third-party-dependencies/) |
| Structured logging in place with correct context/correlation |   ✅    | [Program.cs](../../../src/AccessingChildcareEntitlementChecker.Web/Program.cs) (Azure Monitor telemetry integration) and [Deployment Architecture](/reference/architecture/deployment-architecture/#logging)                                        |
| Absolute prevention of secret leak in system logs            |   ✅    | Verified by secret-scanning in pipelines; documented in [Disclosure of Secrets](/reference/operational/disclosure-of-secrets/)                                                                                                                            |

## Security

| Requirement / Standard                                      | Status | Reference / Action                                                                                                                                                                                  |
|:------------------------------------------------------------|:------:|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Appropriate HTTP Security Headers enforced                  |   ✅    | Configured in `Program.cs` and detailed in [Security Architecture](/reference/architecture/security-architecture/#cryptographic-protections)                                                                                     |
| OWASP Top 10 vulnerability scanning (DAST)                  |   ✅    | [ZAP Scanning Guide](/how-to/zap-scan-guide/) and [ZAP Report](/reference/testing/zap-report/)                                                                                                         |
| Data Encrypted at rest                                      |   ✅    | [Security Architecture](/reference/architecture/security-architecture/#no-database--zero-persistence-strategy) and [Deployment Architecture](/reference/architecture/deployment-architecture/#deployed-resources) |
| Data Encrypted in flight (TLS 1.2+ minimum)                 |   ✅    | [Security Architecture](/reference/architecture/security-architecture/#cryptographic-protections)                                                                                                          |
| Only secure, modern cryptographic ciphers permitted         |   ✅    | Enforced via Azure Front Door; documented in [Security Architecture](/reference/architecture/security-architecture/#cryptographic-protections)                                                                       |
| CORS correctly implemented                                  |   ✅    | Not applicable (self-contained MVC application); documented in [Application Architecture](/reference/architecture/application-architecture/)                                                                         |
| Session cookies set securely with appropriate flags         |   ✅    | Configured in `Program.cs` and [Security Architecture](/reference/architecture/security-architecture/#stride-assessment--mitigations)                                                                                 |
| Easy way to report security issues (security.txt published) |   ✅    | Configured in `frontdoor_rules.tf`                                                                                                                                                                  |
| Threat model created, up-to-date, and documented            |   ✅    | [Security Architecture - Threat Modelling (STRIDE)](/reference/architecture/security-architecture/#stride-assessment--mitigations)                                                                         |
| Anti-personas and attack vectors actively documented        |   ✅    | [Security Architecture - Anti-personas](/reference/architecture/security-architecture/#anti-personas) and [Security Architecture - Attack Vectors](/reference/architecture/security-architecture/#attack-vectors) |

## Monitoring and alerting

| Requirement / Standard                                   | Status | Reference / Action                                                                                                       |
|:---------------------------------------------------------|:------:|:-------------------------------------------------------------------------------------------------------------------------|
| Consolidated logs available in near-realtime             |   ✅    | Sent to Log Analytics; documented in [Deployment Architecture](/reference/architecture/deployment-architecture/#logging)                  |
| Real-time User Monitoring (RUM) or diagnostics telemetry |   ✅    | App Insights integrated in `Program.cs` and [Deployment Architecture](/reference/architecture/deployment-architecture/#logging) |
| System alerting strategy in place                        |   ✅    | [Operational Resilience Index](/reference/operational/)                                                                            |
| Alerts implemented, tested, and routed to stakeholders   |   ✅    | [Application Crashes](/reference/operational/application-crashes/#detection)                                                   |
