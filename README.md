# Childcare Entitlement Checker

A digital service to help parents and carers check their entitlement for childcare support schemes.

## Status

Initial project scaffold.

This repository currently contains a minimal ASP.NET Core MVC application with associated unit and integration tests.

## Tech Stack

- .NET 8
- ASP.NET Core MVC
- xUnit (unit and integration testing)

The [devcontainer.json](./.devcontainer/devcontainer.json) illustrates prerequisites, see [containers.dev](https://containers.dev/implementors/json_reference/) for more information.

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

### Recommended Extensions


#### Visual Studio

- The [Reqnroll](https://marketplace.visualstudio.com/items?itemName=Reqnroll.ReqnrollForVisualStudio2022) extension might be handy.

#### Visual Studio Code

- Gherkin extension (?)

### Build the solution

From the repository root:

```
dotnet build
```

### Run locally

In Visual Studio, use F5 to run in the debugger, or Ctrl-F5 to run without debugging.

```
dotnet run --project src/AccessingChildcareEntitlementChecker.Web
```

The application will start on a local development URL (for example, `https://localhost:xxxx`).

### Integration and Unit tests

In Visual Studio, you can use the test explorer to run tests.

To run all unit, integration and E2E tests from the command line, use:

```
dotnet test
```

### E2E tests

See the documentation at [tests/AccessingChildcareEntitlementChecker.Tests.E2e/README.md](tests/AccessingChildcareEntitlementChecker.Tests.E2e/README.md).