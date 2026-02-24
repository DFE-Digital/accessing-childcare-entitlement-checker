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

### Required GitHub Secrets

- `AZURE_CLIENT_ID`
- `AZURE_TENANT_ID`
- `AZURE_SUBSCRIPTION_ID`

These are for Azure OIDC login in GitHub Actions.

### Required GitHub Repository Variables

- `AZURE_RESOURCE_GROUP_NAME`

`AZURE_RESOURCE_GROUP_NAME` must already exist. The Terraform deployment uses that resource group's location automatically.

### Deploy trigger

Deploy runs on every branch push.

### Important note on Terraform state

This is a deliberately simple setup. Terraform state is local to each workflow run.

For long-term/repeatable deployments, configure a remote backend (for example Azure Storage) so Terraform can track existing infrastructure across runs.
