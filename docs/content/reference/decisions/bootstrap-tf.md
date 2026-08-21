---
title: Bootstrap Terraform state storage in Azure
layout: sub-navigation
order: 11
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions
---
## Context

Terraform requires a remote backend to store state files and coordinate locking across users and pipelines. Azure Storage Accounts and Blob Containers commonly host this remote backend.

This creates a bootstrap ("Day 0") problem:

* Terraform should manage infrastructure declaratively.
* Terraform requires state storage before it can manage infrastructure.
* The primary Terraform configuration cannot create its own remote state backend.

Historically, administrators used manual configuration or one-off scripts to resolve this issue. These approaches introduce inconsistency, documentation drift, and operational risk.

A repeatable, automated mechanism must provision Azure resources for Terraform state storage before the primary Terraform deployment begins.

### Requirements

* Full automation of Terraform backend resource creation.
* Minimisation of manual intervention.
* Support for repeatable deployments across environments and subscriptions.
* Alignment with Infrastructure as Code (IaC) principles.
* Execution within standard CI/CD pipelines.
* Use of native Azure tooling where possible.

### Bootstrap resources

The bootstrap process provisions:

* Resource Group
* Storage Account
* Blob Container for Terraform state
* Diagnostic settings

These resources are managed separately from the main Terraform configuration.

## Options considered

### Option 1: Manual creation (Azure Portal)

Provision the Storage Account and Container manually through the Azure Portal.

#### Positive

* Simple to understand.
* The process requires no additional tooling.
* Quick for proof-of-concept environments.

#### Negative

* Not repeatable.
* Difficult to audit.
* Prone to configuration drift.
* Creates operational dependencies on manual documentation.
* Does not scale across environments.

#### Assessment

Suitable only for experimentation or temporary environments.

### Option 2: Azure CLI script

Use Azure CLI commands to create the required backend resources.

#### Positive

* Fully automatable.
* Easy to execute locally or in CI/CD.
* Minimal dependencies.
* Familiar to Azure administrators.

#### Negative

* Imperative rather than declarative.
* Harder to parse desired state.
* Idempotency requires additional custom handling.
* Can become difficult to maintain as bootstrap requirements grow.

#### Assessment

A pragmatic solution but diverges from Infrastructure as Code principles.

### Option 3: ARM template

Use an Azure Resource Manager (ARM) template to deploy backend resources.

#### Positive

* Native Azure deployment mechanism.
* Declarative.
* Supports repeatable deployments.
* Azure APIs provide full support.

#### Negative

* Verbose JSON syntax.
* Difficult to author and maintain.
* Bicep supersedes ARM templates.
* Lower readability compared with modern IaC approaches.

#### Assessment

Technically viable but not preferred for new development.

### Option 4: Bicep template

Use a dedicated Bicep template to provision Terraform backend resources.

#### Positive

* Declarative Infrastructure as Code.
* Native Azure language.
* Concise and maintainable compared to ARM.
* Supports modularisation and parameterisation.
* Integrates into CI/CD pipelines.
* Idempotent deployments.
* Suitable for future expansion of bootstrap resources.

#### Negative

* Introduces an additional IaC technology alongside Terraform.
* Requires Bicep knowledge for codebase maintenance.
* The team must maintain the bootstrap codebase separately.

#### Assessment

Provides the best balance of automation, maintainability, and Azure-native support.

## Decision

The project implements Terraform backend bootstrapping using Bicep.

A dedicated bootstrap deployment provisions the Azure resources required to host Terraform state before any Terraform execution occurs.

The bootstrap deployment performs the following tasks:

1. Creates the Resource Group.
2. Creates the Storage Account.
3. Creates the Terraform state Blob Container.
4. Creates the Log Analytics Workspace.
5. Configures monitoring and diagnostics.

Terraform configurations assume the backend already exists and do not attempt to create or manage these resources.

## Consequences

### Positive

* Fully automated Day 0 infrastructure.
* Eliminates manual backend setup.
* Repeatable across environments and subscriptions.
* Aligns with Infrastructure as Code practices.
* Azure-native deployment mechanism.
* Simplifies environment onboarding.

### Negative

* Additional deployment step before Terraform can execute.
* Requires maintenance of both Terraform and Bicep codebases.
* A separate process manages the lifecycle of the bootstrap infrastructure.

### Risks

* Bootstrap resources become a special-case deployment path.
* Changes to backend architecture require updates to bootstrap templates and deployment pipelines.

### Mitigations

* Keep bootstrap scope intentionally small.
* Store bootstrap code alongside platform infrastructure repositories.
* Version and test bootstrap templates through CI/CD pipelines.
* Document bootstrap execution as part of environment provisioning.

## Outcome

A dedicated Bicep-based bootstrap process provides the Azure Storage Account and associated resources. This removes the manual bootstrap problem. It establishes a repeatable deployment pattern for environments.
