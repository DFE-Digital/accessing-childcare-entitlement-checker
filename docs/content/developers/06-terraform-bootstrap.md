---
title: Terraform state bootstrapping guide
layout: sub-navigation
sectionKey: Developers
order: 6
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Developers
  key: Terraform bootstrapping
---
To manage infrastructure declaratively with Terraform, a remote backend is required to securely store state files (`.tfstate`) and coordinate state locking. However, this introduces a classic Day 0 "chicken-and-egg" problem:

* Terraform needs a remote backend (Azure Storage Account and Blob Container) to run.
* The remote backend cannot be managed by the same Terraform configuration that relies on it.

To solve this, this project implements a fully automated, declarative bootstrapping process using Azure Bicep and Azure CLI, which is seamlessly integrated into our deployment pipelines. 

This guide explains how the bootstrapping architecture works, how it is secured, how it is integrated into CI/CD, and how you can interact with it if necessary.

## Architecture overview

We use Azure Bicep for provisioning the bootstrap infrastructure. The bootstrap phase runs before any Terraform execution. It is scoped at the Azure Subscription level, allowing Bicep to dynamically create or manage the target resource group and populate it with the state backend resources.

```mermaid
sequenceDiagram
    autonumber
    actor pipeline as GitHub Actions Pipeline
    participant bicep as Azure Resource Manager (Bicep)
    participant script as manage-storage-access.sh
    participant storage as Azure State Storage Account
    participant tf as Terraform CLI

    pipeline->>bicep: Deploy main.bicep (subscription scope)
    bicep->>storage: Provision Resource Group & Storage Account
    Note over storage: Hardened by default:<br/>Network access & <br/>shared Key access disabled

    pipeline->>script: Unlock State Storage
    script->>storage: Enable Shared Key & Network Access
    Note over script: Sleeps 30 seconds <br/>for propagation

    pipeline->>tf: Run Terraform (init, plan, apply)
    tf->>storage: Read / Write State (.tfstate)

    pipeline->>script: Lock State Storage
    script->>storage: Disable Shared Key & Network Access
    Note over storage: Returned to hardened <br/>resting state
```

### Components created

The bootstrap resources are defined in `infra/bicep/` and consist of:

1. Resource Group: Isolated specifically for managing state storage (e.g., `s279d01rg-uks-cec-terraform`).
2. Virtual Network & Networking:
   * A dedicated Virtual Network (e.g., `s279d01-uks-cec-vnet-tf-state` with address prefix `10.1.0.0/16`).
   * A custom subnet (e.g., `s279d01-uks-cec-snet-tf-state` with address prefix `10.1.0.0/24`).
   * A custom Network Security Group (NSG) (named `${subnetName}-nsg`) associated with the subnet.
   * A Private DNS Zone named `privatelink.blob.core.windows.net` (`privatelink.blob.${environment().suffixes.storage}`) linked to the Virtual Network (`${vnetName}-link`).
3. Private Endpoint:
   * A private endpoint (named `${storageAccountName}-pe`) that connects the storage account securely to the custom subnet using the `blob` sub-resource.
   * A Private DNS Zone Group to register the endpoint's private IP with the `privatelink.blob.core.windows.net` zone.
4. Storage Account: Hosts the blob state. Hardened by default with:
   * Minimum TLS Version set to `TLS1_2`.
   * Secure transit only (`supportsHttpsTrafficOnly: true`).
   * Disabled Shared Key Access (`allowSharedKeyAccess: false`).
   * Disabled Public Network Access (`publicNetworkAccess: 'Disabled'`).
   * Disabled Public Blob Access (`allowBlobPublicAccess: false`).
   * Default network action of `Deny` with bypass allowed only for `AzureServices`.
   * SKU `Standard_ZRS` (Zone-Redundant Storage) to ensure high availability.
5. Blob Service & Container:
   * Versioning enabled on the blob service (`isVersioningEnabled: true`).
   * Soft-delete retention policies (14 days) enabled for both blobs and containers.
   * A private blob container named `tfstate`.
6. Log Analytics Workspace & Diagnostics:
   * A workspace dedicated to tracking operations (e.g., `279d01-uks-cec-law-tf-state`) with a data retention period of 90 days and the `PerGB2018` SKU.
   * Diagnostic settings on the Storage Account's Blob Service sending logs (`StorageRead`, `StorageWrite`, `StorageDelete`) and transactions to the Log Analytics Workspace for security auditing (named `${storageAccountName}-blob-diag`).