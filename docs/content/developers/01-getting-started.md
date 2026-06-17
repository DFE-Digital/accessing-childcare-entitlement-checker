---
title: Getting started
layout: sub-navigation
sectionKey: Developers
order: 1
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Developers
  key: Getting started
---
This guide provides step-by-step instructions for setting up your local environment, building the solution, running the application, and executing tests.

## Prerequisites

Before setting up the repository, ensure you have the following installed on your machine:

- .NET SDK 10.0.3: This project is pinned to this version. You can verify this in `global.json`. 
- PowerShell 7 (pwsh): Required for running Playwright browser installation scripts on all platforms.
- Recommended IDE:
  - Visual Studio 2026.
  - VS Code.
  - JetBrains Rider.

## Local environment setup

1. Clone the repository to your workstation.
2. Restore local .NET tools:
   From the repository root, run:
   ```bash
   dotnet tool restore
   ```
3. Install Playwright browsers:
   Playwright requires specific browser binaries to run E2E tests. To install them, first build the solution, then run the generated PowerShell script:
   
   *Using PowerShell:*
   ```powershell
   dotnet build
   pwsh .\tests\AccessingChildcareEntitlementChecker.Tests.E2e\bin\Debug\net10.0\playwright.ps1 install
   ```

## Building and running the application

### Build the solution

From the repository root, build the solution using:

```bash
dotnet build
```

*Note: The project uses `Directory.Build.props` configured with `<UseArtifactsOutput />`, meaning all build artifacts are placed under `/artifacts` rather than in-project `bin` directories.*

### Run the web application

To launch the web application locally:

```bash
dotnet run --project src/AccessingChildcareEntitlementChecker.Web
```

Once started, the application will be accessible via the local development URL shown in your console output (e.g., `https://localhost:xxxx` or `http://localhost:xxxx`).

## Verifying setup (Running tests)

Testing is split into Unit/Component tests and End-to-End (E2E) tests.

### Unit & component tests

These tests run in-memory and do not require the web application to be running. They check dependency injection, basic routing, and core logic.

To execute them:

```bash
dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests
```

### End-to-end (E2E) tests

E2E tests use Playwright to simulate user interactions against a running application instance.

First, ensure the web application is running (e.g., at `http://localhost:5252/`). Then, execute the tests with the `TEST_URL` environment variable set:

```bash
dotnet test tests/AccessingChildcareEntitlementChecker.Tests.E2e --no-build
```

By default, the browser will run in headless mode when running in CI, but may open on your desktop locally.

## Common tasks

### Formatting and linting

We enforce code formatting rules across the codebase. You can use the built-in dotnet formatter to lint and automatically fix format issues:

```bash
dotnet format
```

This formatting is validated during the CI process on pull requests.

## Next steps

To learn more about how we work, check out the following guides:

- [Ways of Working](../02-ways-of-working)
- [Branching Strategy](../03-branching-strategy)
- [Documentation Guide](../04-documentation-guide)
- [Decision Process](../05-decision-process)
