---
title: Bootstrap Terraform State Storage in Azure
layout: page
showPagination: true
order: 11
sectionKey: Decisions
eleventyNavigation:
  parent: Decisions
---

## Context

Terraform requires a remote backend to store state files and coordinate locking across users and pipelines. In Azure, this is commonly implemented using an Azure Storage Account and Blob Container.

This creates a bootstrap ("Day 0") problem:

* Terraform should manage infrastructure declaratively.
* Terraform requires state storage before it can manage infrastructure.
* The remote state backend cannot be created by the same Terraform configuration that depends on it.

Historically, teams have solved this through manual setup ("clickops") or one-off scripts. These approaches introduce inconsistency, documentation drift, and operational risk.

We need a repeatable, automated mechanism to provision the Azure resources required for Terraform state storage before the main Terraform deployment process begins.

### Requirements

* Fully automate creation of Terraform backend resources.
* Minimise manual intervention.
* Support repeatable deployments across environments and subscriptions.
* Align with Infrastructure as Code (IaC) principles.
* Be simple to execute in CI/CD pipelines.
* Use native Azure tooling where possible.

### Bootstrap Resources

The bootstrap process provisions:

* Resource Group
* Storage Account
* Blob Container for Terraform state
* Diagnostic settings

These resources are managed separately from the main Terraform configuration.

## Options Considered

### Option 1: Manual Creation (Portal / ClickOps)

Provision the Storage Account and Container manually through the Azure Portal.

#### Pros

* Simple to understand.
* No additional tooling required.
* Quick for proof-of-concept environments.

#### Cons

* Not repeatable.
* Difficult to audit.
* Prone to configuration drift.
* Creates operational dependency on manual documentation.
* Does not scale across environments.

#### Assessment

Suitable only for experimentation or temporary environments.

### Option 2: Azure CLI Script

Use Azure CLI commands to create the required backend resources.

#### Pros

* Fully automatable.
* Easy to execute locally or in CI/CD.
* Minimal dependencies.
* Familiar to Azure administrators.

#### Cons

* Imperative rather than declarative.
* Harder to understand desired state.
* Idempotency requires additional handling.
* Can become difficult to maintain as bootstrap requirements grow.

#### Assessment

A pragmatic solution but diverges from Infrastructure as Code principles.

### Option 3: ARM Template

Use an Azure Resource Manager (ARM) template to deploy backend resources.

#### Pros

* Native Azure deployment mechanism.
* Declarative.
* Supports repeatable deployments.
* Well-supported by Azure APIs.

#### Cons

* Verbose JSON syntax.
* More difficult to author and maintain.
* Increasingly superseded by Bicep.
* Lower readability compared with modern IaC approaches.

#### Assessment

Technically viable but not preferred for new development.

### Option 4: Bicep Template

Use a dedicated Bicep template to provision Terraform backend resources.

#### Pros

* Declarative Infrastructure as Code.
* Native Azure language.
* More concise and maintainable than ARM.
* Supports modularisation and parameterisation.
* Easily integrated into CI/CD pipelines.
* Idempotent deployments.
* Suitable for future expansion of bootstrap resources.

#### Cons

* Introduces an additional IaC technology alongside Terraform.
* Requires Bicep knowledge within the team.
* Bootstrap codebase must be maintained separately.

#### Assessment

Provides the best balance of automation, maintainability, and Azure-native support.

## Decision

We will implement Terraform backend bootstrapping using Bicep.

A dedicated bootstrap deployment will provision the Azure resources required to host Terraform state before any Terraform execution occurs.

The bootstrap deployment will:

1. Create the Resource Group.
2. Create the Storage Account.
3. Create the Terraform state Blob Container.
4. Create the Log Analytics Workspace.
5. Monitoring and diagnostics.

Terraform configurations will assume the backend already exists and will not attempt to create or manage these resources.

## Consequences

### Positive

* Fully automated Day 0 infrastructure.
* Eliminates manual backend setup.
* Repeatable across environments and subscriptions.
* Aligns with Infrastructure as Code practices.
* Azure-native deployment mechanism.
* Easier onboarding and environment creation.

### Negative

* Additional deployment step before Terraform can run.
* Team must maintain both Terraform and Bicep codebases.
* Bootstrap infrastructure lifecycle is managed separately from Terraform-managed resources.

### Risks

* Bootstrap resources become a special-case deployment path.
* Changes to backend architecture require updates to bootstrap templates and deployment pipelines.

### Mitigations

* Keep bootstrap scope intentionally small.
* Store bootstrap code alongside platform infrastructure repositories.
* Version and test bootstrap templates through CI/CD.
* Document bootstrap execution as part of environment provisioning.

## Outcome

A dedicated Bicep-based bootstrap process will provide the Azure Storage Account and associated resources required for Terraform state management, removing the manual "chicken and egg" problem and establishing a repeatable Day 0 deployment pattern for environments.
