---
title: Technical review checklist
layout: sub-navigation
sectionKey: Architecture
order: 8
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Architecture
  key: Technical review checklist
---
This checklist tracks the project's compliance with government technical review standards, encompassing governance, build and deployment, infrastructure and data architecture, software engineering, security, and monitoring.

## Project governance

| Requirement / Standard                                              | Status | Reference / Action                                                                                                                                                                                               |
|:--------------------------------------------------------------------|:------:|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Branching policy in place and documented                            |   ✅    | [Branching Strategy](/developers/03-branching-strategy/)                                                                                                                                                         |
| Commits / PRs linked to stories and enforced by policy              |   ✅    | See PR Template and [Ways of Working](/developers/02-ways-of-working/#branching-and-commits)                                                                                                                     |
| Process for PR review in place and documented                       |   ✅    | [Ways of Working](/developers/02-ways-of-working/#pull-requests)                                                                                                                                                 |
| Repository access control in place and documented                   |   ✅    | [Security Architecture](/architecture/06-security-architecture/#deployment-identity-github-actions-runner)                                                                                                       |
| Secure secrets management in place and strategy documented          |   ✅    | [Disclosure of Secrets](/operational/008-disclosure-of-secrets/) and [Rotate Secrets](/runbooks/004-rotate-secrets/)                                                                                             |
| Code is open sourced                                                |   ✅    | See license file                                                                                                                                                                                                 
| Contributor guidance, local environment setup, and build documented |   ✅    | [Getting Started](/developers/01-getting-started/), [Ways of Working](/developers/02-ways-of-working/), and [CONTRIBUTING.md](../../../CONTRIBUTING.md)                                                             |
| Code has an appropriate open-source license                         |   ✅    | See license file                                                                                                                                                                                                 |
| Documentation is stored alongside source code and is complete       |   ✅    | [Technical Documentation](/)                                                                                                                                                                                     |
| Architecture Decision Records (ADRs) are managed correctly          |   ✅    | [Architecture Decision Records](/decisions/)                                                                                                                                                                     |
| Test strategy and plan in place and documented                      |   ✅    | [Test Strategy](/testing/01-test-strategy/)                                                                                                                                                                         |
| RAID Log in place and correctly managed                             |   ⏳    | Managed externally in project management tools (Jira/Confluence)                                                                                                                                                 |
| Architecture documentation is complete and up-to-date               |   ✅    | [Architecture Information](/architecture/)                                                                                                                                                                       |
| Data Retention strategy in place and documented                     |   ✅    | [Security Architecture](/architecture/06-security-architecture/#no-database--zero-persistence-strategy) and 30-day log retention in [Deployment Architecture](/architecture/03-deployment-architecture/#logging) |
| Disaster Recovery plan in place, tested, and documented             |   ✅    | [Regional Failover Runbook](/runbooks/006-regional-failover/) and [Regional Failure](/operational/004-regional-failure/)                                                                                         |
| Fully documented release process and path-to-live                   |   ✅    | [Release Process](/developers/08-release-process/)                                                                                                                                                               |
| Operational Runbooks are fully documented and tested                |   ✅    | [Runbooks Index](/runbooks/)                                                                                                                                                                                     |

## Build and deployment

| Requirement / Standard                                        | Status | Reference / Action                                                                                                                    |
|:--------------------------------------------------------------|:------:|:--------------------------------------------------------------------------------------------------------------------------------------|
| Full CI pipeline implemented (builds predominantly green)     |   ✅    | PR Validation Workflow                                                                                                                |
| CD strategy in place with a documented path-to-live           |   ✅    | [Release Process](/developers/08-release-process/) and [Deployment Architecture](/architecture/03-deployment-architecture/)           |
| Infrastructure as Code (IaC) tooling versioned alongside code |   ✅    | [Terraform Bootstrap](/developers/06-terraform-bootstrap/) and See infra folder                                                       |
| Automated unit testing with agreed coverage metrics           |   ✅    | See build dotnet workflow                                                                                                             |
| Automated functional/E2E tests with build-fail validation     |   ✅    | [Test Strategy](/testing/01-test-strategy/) and E2E test project                                                                         |
| Automated dynamic security tests (API/Web spidering/DAST)     |   ✅    | [ZAP Scanning Guide](/developers/11-zap-scan-guide/) and [ZAP Report](/testing/99-zap-report/)                                           |
| Automated static security tests (dependency/SAST analysis)    |   ✅    | See build infrastructure workflow and build dotnet workflow                                                                           |
| Automated cross-browser tests                                 |   ✅    | See nightly cross-browser tests workflow and [Cross-Browser Testing Strategy](/testing/03-cross-browser/)                                |
| Automated accessibility tests                                 |   ⏳    | In progress/planned; documented in [Accessibility Test Plan](/testing/02-accessibility-test-plan/) with workflow placeholders            |
| Automated performance tests                                   |   ⏳    | Planned/defined in [Test Strategy](/testing/01-test-strategy/#performance--load-testing) but not currently integrated in the CI pipeline |
| Build scripts documented and versioned alongside code         |   ✅    | [Workflow Naming Conventions](/developers/09-workflow-naming-conventions/)                                                            |
| Automated Software Bill of Materials (SBOM) generation        |   ⏳    | Not currently configured                                                                                                              |
| DORA metrics recorded to monitor development lifecycle        |   ⏳    | Not currently tracked                                                                                                                 |


## Infrastructure architecture

| Requirement / Standard                                        | Status | Reference / Action                                                                                                                                                                |
|:--------------------------------------------------------------|:------:|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Environment strategy in place and fully documented            |   ✅    | [Deployment Architecture](/architecture/03-deployment-architecture/#1-local-development) and [Release Process](/developers/08-release-process/)                                   |
| Environment access control policies in place and documented   |   ✅    | [Security Architecture](/architecture/06-security-architecture/#cloud-operations--site-reliability-engineer-sre) and [Unauthorised Access](/operational/007-unauthorised-access/) |
| Scalability, costs, and volumetrics considered and documented |   ✅    | [Non-Functional Requirements](/architecture/07-non-functional-requirements/)                                                                                                      |
| Infrastructure naming strategy in place and documented        |   ✅    | [infra/terraform/locals.tf](../../../infra/terraform/locals.tf)                                                                                                                      |
| Policy as Code to prevent unauthorized service provisioning   |   ✅    | [Test Strategy](/testing/01-test-strategy/#infrastructure-testing) and see build infrastructure workflow                                                                             |

## Software architecture/engineering

| Requirement / Standard                                       | Status | Reference / Action                                                                                                                                                                                                                        |
|:-------------------------------------------------------------|:------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Code naming and style conventions agreed and documented      |   ✅    | [Ways of Working](/developers/02-ways-of-working/#branching-and-commits) and [Workflow Naming Conventions](/developers/09-workflow-naming-conventions/)                                                                                   |
| Code comments used appropriately                             |   ✅    | [Ways of Working](/developers/02-ways-of-working/#pull-requests) (enforced via PR reviews)                                                                                                                                                |
| Code quality monitored automatically by a static tool        |   ✅    | SonarCloud integrated in build dotnet Workflow                                                                                                                                                                                            |
| Appropriate error handling and graceful failure strategy     |   ✅    | [Application Crashes](/operational/002-application-crashes/), [Security Architecture](/architecture/06-security-architecture/#stride-assessment--mitigations), and [Third-Party Dependencies](/operational/009-third-party-dependencies/) |
| Structured logging in place with correct context/correlation |   ✅    | [Program.cs](../../../src/AccessingChildcareEntitlementChecker.Web/Program.cs) (Azure Monitor telemetry integration) and [Deployment Architecture](/architecture/03-deployment-architecture/#logging)                                        |
| Absolute prevention of secret leak in system logs            |   ✅    | Verified by secret-scanning in pipelines; see [Disclosure of Secrets](/operational/008-disclosure-of-secrets/)                                                                                                                            |

## Security

| Requirement / Standard                                      | Status | Reference / Action                                                                                                                                                                                  |
|:------------------------------------------------------------|:------:|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Appropriate HTTP Security Headers enforced                  |   ✅    | See `Program.cs` and [Security Architecture](/architecture/06-security-architecture/#cryptographic-protections)                                                                                     |
| OWASP Top 10 vulnerability scanning (DAST)                  |   ✅    | [ZAP Scanning Guide](/developers/11-zap-scan-guide/) and [ZAP Report](/testing/99-zap-report/)                                                                                                         |
| Data Encrypted at rest                                      |   ✅    | [Security Architecture](/architecture/06-security-architecture/#no-database--zero-persistence-strategy) and [Deployment Architecture](/architecture/03-deployment-architecture/#deployed-resources) |
| Data Encrypted in flight (TLS 1.2+ minimum)                 |   ✅    | [Security Architecture](/architecture/06-security-architecture/#cryptographic-protections)                                                                                                          |
| Only secure, modern cryptographic ciphers permitted         |   ✅    | Enforced via Azure Front Door; see [Security Architecture](/architecture/06-security-architecture/#cryptographic-protections)                                                                       |
| CORS correctly implemented                                  |   ✅    | Not applicable (self-contained MVC application); see [Application Architecture](/architecture/01-application-architecture/)                                                                         |
| Session cookies set securely with appropriate flags         |   ✅    | See `Program.cs`and [Security Architecture](/architecture/06-security-architecture/#stride-assessment--mitigations)                                                                                 |
| Easy way to report security issues (security.txt published) |   ✅    | Configured in `frontdoor_rules.tf`                                                                                                                                                                  |
| Threat model created, up-to-date, and documented            |   ✅    | [Security Architecture - Threat Modelling (STRIDE)](/architecture/06-security-architecture/#stride-assessment--mitigations)                                                                         |
| Anti-personas and attack vectors actively documented        |   ✅    | [Security Architecture - Anti-personas](/architecture/06-security-architecture/#anti-personas) and [Security Architecture - Attack Vectors](/architecture/06-security-architecture/#attack-vectors) |

## Monitoring and alerting

| Requirement / Standard                                   | Status | Reference / Action                                                                                                       |
|:---------------------------------------------------------|:------:|:-------------------------------------------------------------------------------------------------------------------------|
| Consolidated logs available in near-realtime             |   ✅    | Sent to Log Analytics; see [Deployment Architecture](/architecture/03-deployment-architecture/#logging)                  |
| Real-time User Monitoring (RUM) or diagnostics telemetry |   ✅    | App Insights integrated in `Program.cs` and [Deployment Architecture](/architecture/03-deployment-architecture/#logging) |
| System alerting strategy in place                        |   ✅    | [Operational Resilience Index](/operational/)                                                                            |
| Alerts implemented, tested, and routed to stakeholders   |   ✅    | [Application Crashes](/operational/002-application-crashes/#detection)                                                   |
