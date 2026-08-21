---
title: Third-party dependency failures
layout: sub-navigation
order: 9
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
External package registries, hosting platforms, or third-party code packages become unavailable, affecting the ability to build, validate, or serve the tool.

## Key dependencies

As a fully self-contained rules engine, the Entitlement Checker has zero runtime external API or database dependencies (such as external notifications or analytics APIs), which ensures high runtime robustness.

Active dependencies are limited to:

1. Azure Cloud Platform (Core Infrastructure)
2. NuGet / Package Feed (Build/Release stage)
3. GitHub Actions (CI/CD Pipeline)

### Dependency: Azure cloud platform
- Impact: Loss of hosting, causing service downtime.
- Prevention: Hosting is established on high-availability Azure App Services and Azure Front Door in the UK South region.
- Detection: Automatic service-health alerts are received from the Azure status page.
- Recovery: Service resumption depends on Azure platform recovery or the initiation of a regional failover to a backup region.

### Dependency: NuGet / package feed
- Impact: Inability to run CI/CD builds or deploy hotfixes due to missing packages.
- Prevention: The codebase uses lock files and NuGet caching inside GitHub Actions to ensure deterministic builds during package registry degradation.
- Detection: Package restore failures during GitHub Actions compilation steps.
- Recovery: Service resumption depends on NuGet registry recovery. Under emergency conditions, a release artifact is built locally on a developer workstation and deployed manually via Azure CLI.

### Dependency: GitHub actions
- Impact: Inability to execute pull request checks, run Terraform provisions, or deploy zip updates.
- Prevention: Code and configurations are fully versioned in Git. Infrastructure can be provisioned and managed locally using developer Terraform tools and the Azure CLI.
- Detection: GitHub runner timeouts or execution errors.
- Recovery: GitHub status monitoring is conducted. In emergency situations, deployments are executed locally from an authenticated terminal using Terraform and Azure CLI scripts.

## Related runbooks

- [Regional failover](/how-to/runbooks/regional-failover/)
- [Investigate service degradation](/how-to/runbooks/investigate-service-degradation/)
