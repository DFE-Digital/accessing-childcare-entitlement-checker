---
title: Dependabot
layout: sub-navigation
sectionKey: Reference
order: 5
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Reference
  key: Dependabot

---
The repository uses an enhanced Dependabot configuration. This configuration improves:

- Dependency security
- Supply chain resilience
- Pull request maintainability
- Operational stability
- Upgrade review quality

The configuration balances security responsiveness with developer experience. It reduces unnecessary pull request noise and ensures dependencies remain current.

## Weekly dependency updates

The configuration schedules weekly updates for all ecosystems. Weekly updates balance:

- Timely security remediation
- Reduced CI/CD churn
- Lower pull request fatigue
- Easier dependency review management

Daily updates can create excessive operational noise in active repositories, especially across multiple ecosystems.

Security advisories and Dependabot security alerts still provide rapid visibility into critical vulnerabilities when required.

## Ecosystems covered

The configuration currently manages updates for:

| Ecosystem      | Purpose                                         |
|----------------|-------------------------------------------------|
| NuGet          | .NET application dependencies                   |
| GitHub Actions | CI/CD workflow dependencies                     |
| Terraform      | Infrastructure provider and module dependencies |

This process monitors both application and infrastructure supply chains.

## Cooldown Windows

Cooldown periods delay newly published versions before Dependabot creates pull requests. This reduces exposure to:

- Malicious package releases
- Recalled versions
- Ecosystem regressions
- Bad upstream publishes

The strategy intentionally varies by ecosystem:

| Ecosystem      | Cooldown Strategy                                     |
|----------------|-------------------------------------------------------|
| NuGet          | Short stabilisation period                            |
| GitHub Actions | Short stabilisation period                            |
| Terraform      | Longer stabilisation due to provider instability risk |

The system continues to prioritise security updates independently.

## Blocking automatic major version updates

The configuration ignores major version updates by default. Major upgrades frequently include:

- Breaking changes
- Behavioural changes
- Infrastructure risk
- Large testing requirements

The team must handle major upgrades manually with proper validation instead of using automated routine updates.

## Automatic rebasing

Dependabot pull requests automatically rebase against the target branch. This helps:

- Keep security fixes mergeable
- Reduce stale pull requests
- Avoid unnecessary manual intervention
- Minimise merge conflicts

## Restricting external code execution

The configuration disables external code execution during dependency resolution where supported. 

Some package ecosystems allow arbitrary scripts during dependency evaluation.

Disabling scripts reduces the supply chain attack surface during automated dependency processing.

## Grouped patch updates

The tool groups patch updates into consolidated pull requests. Patch releases are generally:

- Low risk
- Backwards compatible
- High volume

Grouping them reduces pull request volume significantly while maintaining update coverage.

## Vendor-based minor grouping

The tool groups minor updates by vendor or ecosystem domain. This improves:

- Review clarity
- Dependency compatibility tracking
- Operational maintainability

It also makes rollback and troubleshooting easier if issues occur.
