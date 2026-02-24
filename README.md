# Childcare Entitlement Checker

A digital service to help parents and carers check their entitlement for childcare support schemes.

## Status

Initial project scaffold.

This repository currently contains a minimal ASP.NET Core MVC application with associated unit and integration tests.

## Tech Stack

- .NET 8
- ASP.NET Core MVC
- xUnit (unit and integration testing)

## Project Structure

```
src/
  AccessingChildcareEntitlementChecker.Web

tests/
  AccessingChildcareEntitlementChecker.UnitTests
```
- `Web` contains the MVC application.
- `UnitTests` contains controller-level unit tests.

## Developer Setup

### Prerequisites

- .NET 8 SDK

Verify installed SDKs:

```
dotnet --list-sdks
```

### Build the solution

From the repository root:

```
dotnet build
```

### Run locally

```
dotnet run --project src/AccessingChildcareEntitlementChecker.Web
```

The application will start on a local development URL (for example, `https://localhost:xxxx`).

### Run tests

```
dotnet test
```

This will execute both unit and integration tests.

## GitHub Actions deploy (Terraform + Azure Web App)

The workflow at `.github/workflows/build-and-test.yml` now includes a `deploy` job that:

1. Publishes and zips the ASP.NET app.
2. Runs Terraform from `infra/terraform` to create/update, inside an existing Resource Group:
  - Linux App Service Plan (B1)
  - Linux Web App (.NET 8)
3. Deploys the zipped app to the Web App.
4. Uses branch-derived names so each branch gets its own feature stack.

### Required GitHub Environment (`dev`) Secrets

- `AZURE_CLIENT_ID`
- `AZURE_TENANT_ID`
- `AZURE_SUBSCRIPTION_ID`

These are used by Azure OIDC login in GitHub Actions and should be defined on the GitHub environment named `dev`.

### Required GitHub Environment (`dev`) Variables

- `TFSTATE_RESOURCE_GROUP_NAME`
- `TFSTATE_STORAGE_ACCOUNT_NAME`
- `TFSTATE_CONTAINER_NAME`

These values are used by Terraform's `azurerm` backend so workflow runs share remote state in Azure Storage.

If these variables are not set, the deploy workflow bootstraps defaults automatically:

- Resource group: `rg-west-europe`
- Container: `tfstate`
- Storage account: deterministic name derived from repository + subscription ID

Providing explicit values is recommended for production-grade setups.

The workflows currently target the existing hardcoded resource group `rg-sjm-test`.

Both deployment workflows are pinned to the `dev` environment so OIDC federation can target the environment subject claim.

### Deploy trigger

Deploy runs on every branch push.

### Terraform state

Terraform state is stored remotely in Azure Storage via the `azurerm` backend.

The workflow uses a branch-derived state key so each branch stack has isolated state while still persisting across runs.
