---
title: Dependabot
layout: sub-navigation
sectionKey: Developers
eleventyNavigation:
  parent: Developers
  key: Dependabot
order: 3
---

This repository uses an enhanced Dependabot configuration designed to improve:

- Dependency security
- Supply chain resilience
- Pull request maintainability
- Operational stability
- Upgrade review quality

The configuration intentionally balances security responsiveness with developer experience by reducing unnecessary pull request noise while still ensuring dependencies remain current.

## Weekly Dependency Updates

All ecosystems are configured to run on a weekly schedule. Weekly updates provide a good balance between:

- Timely security remediation
- Reduced CI/CD churn
- Lower pull request fatigue
- Easier dependency review management

Daily updates can create excessive operational noise in active repositories, especially across multiple ecosystems.

Security advisories and Dependabot security alerts still provide rapid visibility into critical vulnerabilities when required.

## Ecosystems Covered

The configuration currently manages updates for:

| Ecosystem      | Purpose                                         |
|----------------|-------------------------------------------------|
| NuGet          | .NET application dependencies                   |
| GitHub Actions | CI/CD workflow dependencies                     |
| Terraform      | Infrastructure provider and module dependencies |

This ensures both application and infrastructure supply chains are monitored.

## Cooldown Windows

Cooldown periods delay newly published versions before Dependabot creates pull requests. This reduces exposure to:

- Malicious package releases
- Recalled versions
- Ecosystem regressions
- Bad upstream publishes

The strategy intentionally varies by ecosystem:

| Ecosystem      | Cooldown Strategy                                     |
|----------------|-------------------------------------------------------|
| NuGet          | Short stabilization period                            |
| GitHub Actions | Short stabilization period                            |
| Terraform      | Longer stabilization due to provider instability risk |

Security updates remain prioritised independently.

## Blocking Automatic Major Version Updates

Major version updates are ignored by default. Major upgrades frequently include:

- Breaking changes
- Behavioural changes
- Infrastructure risk
- Large testing requirements

These upgrades should be handled manually with proper validation rather than automatically merged through routine dependency management.

## Automatic Rebasing

Dependabot pull requests automatically rebase against the target branch. This helps:

- Keep security fixes mergeable
- Reduce stale pull requests
- Avoid unnecessary manual intervention
- Minimise merge conflicts

## Restricting External Code Execution

External code execution during dependency resolution is disabled where supported. 

Some package ecosystems allow arbitrary scripts during dependency evaluation.

Disabling this reduces supply chain attack surface during automated dependency processing.

## Grouped Patch Updates

Patch updates are grouped into consolidated pull requests. Patch releases are generally:

- Low risk
- Backwards compatible
- High volume

Grouping them reduces pull request volume significantly while maintaining update coverage.

## Vendor-Based Minor Grouping

Minor updates are grouped by vendor or ecosystem domain. This improves:

- Review clarity
- Dependency compatibility tracking
- Operational maintainability

It also makes rollback and troubleshooting easier if issues occur.
