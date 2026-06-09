---
title: Third-party dependency failures
layout: page
showPagination: true
order: 9
sectionKey: Operational
eleventyNavigation:
  parent: Operational
---

External package registries, hosting platforms, or third-party code packages become unavailable, affecting our ability to build, validate, or serve the tool.

## Key Dependencies

Because the Entitlement Checker is a fully self-contained rules engine, it has zero runtime external API or database dependencies (such as external notifications or analytics APIs), making the runtime exceptionally robust.

Our active dependencies are limited to:

1. Azure Cloud Platform (Core Infrastructure)
2. NuGet / Package Feed (Build/Release stage)
3. GitHub Actions (CI/CD Pipeline)

### Dependency: Azure Cloud Platform
- Impact: Loss of hosting, causing service downtime.
- Prevention: Hosted on high-availability Azure App Services and Azure Front Door in the UK South region.
- Detection: Automatic service-health alerts from the Azure status page.
- Recovery: Wait for Azure platform recovery or initiate a regional failover to a backup region.

### Dependency: NuGet / Package Feed
- Impact: Inability to run CI/CD builds or deploy hotfixes due to missing packages.
- Prevention: The codebase uses lock files and NuGet caching inside GitHub Actions to ensure deterministic builds even during package registry degradation.
- Detection: Package restore failures during GitHub Actions compilation steps.
- Recovery: Wait for NuGet registry recovery. Under emergency conditions, build a release artifact locally on a developer workstation and deploy it manually via Azure CLI.

### Dependency: GitHub Actions
- Impact: Inability to execute pull request checks, run Terraform provisions, or deploy zip updates.
- Prevention: Code and configurations are fully versioned in Git. Infrastructure can be provisioned and managed locally using developer Terraform tools and Azure CLI if necessary.
- Detection: GitHub runner timeouts or execution errors.
- Recovery: Monitor GitHub status. In emergency situations, deployments can be executed locally from an authenticated terminal using Terraform and Azure CLI scripts.

## Related runbooks

- [Regional failover](/runbooks/006-regional-failover/)
- [Investigate service degradation](/runbooks/007-investigate-service-degradation/)
