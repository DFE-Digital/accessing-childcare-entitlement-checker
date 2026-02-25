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

### Build in CI

The workflow at `.github/workflows/build-and-test.yml` builds the solution and runs tests.