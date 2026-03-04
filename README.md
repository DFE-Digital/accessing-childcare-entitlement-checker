# Childcare Entitlement Checker

A digital service to help parents and carers check their entitlement for childcare support schemes.

This project will be a multi-step form.

## Status

Initial project scaffold - user research is ongoing.

This repository currently contains a minimal ASP.NET Core MVC application with associated unit and integration tests.

### Known Issues/In Progress

- Non functional!
- Does not yet comply with DFE standards as per https://technical-guidance.education.gov.uk/

## Tech Stack

- .NET 8
- ASP.NET Core MVC
- xUnit (unit and integration testing)

The [devcontainer.json](./.devcontainer/devcontainer.json) illustrates prerequisites, see [containers.dev](https://containers.dev/implementors/json_reference/) for more information.

## Project Structure & Docs Index

- [CONTRIBUTING.md](/CONTRIBUTING.md)
- `src/`
  - `AccessingChildcareEntitlementChecker.Web` - contains the MVC application.
- `tests/`
  - `AccessingChildcareEntitlementChecker.Tests.E2e` E2e tests ([README.md](tests/AccessingChildcareEntitlementChecker.Tests.E2e/README.md))
  - `AccessingChildcareEntitlementChecker.Tests.UnitTests` contains controller-level unit tests.

## Developer quick-start

See [CONTRIBUTING.md](/CONTRIBUTING.md) for more detailed information.

### Prerequisites

- [.NET SDK 8.0.418](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - The projects is pinned to this with a `global.json`

You can verify installed SDKs with:

```bash
dotnet --list-sdks
```

For Visual Studio, 2026 is the recommended version.

### Build the solution

From the repository root:

```bash
dotnet build
```

Note that the project uses a [Directory.Build.props](/Directory.Build.props) with `<UseArtifactsOutput />` so
build artifacts will land in `/artifacts`.

### Run locally

In Visual Studio, use F5 to run in the debugger, or Ctrl-F5 to run without debugging.

```bash
dotnet run --project src/AccessingChildcareEntitlementChecker.Web
```

The application will start on a local development URL (for example, `https://localhost:xxxx`).

### Integration and Unit tests

In Visual Studio, you can use the test explorer to run tests.

To run all unit, integration and E2E tests from the command line, use:

```bash
dotnet test
```
