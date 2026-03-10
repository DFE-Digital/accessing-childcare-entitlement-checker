# Development

## Status

Initial project scaffold - user research is ongoing.

This repository currently contains a minimal ASP.NET Core MVC application with associated unit and integration tests.

## Known Issues/In Progress

- Not yet feature complete!
- Does not yet comply with DFE standards as per https://technical-guidance.education.gov.uk/

## Tech Stack

- .NET 10
- ASP.NET Core MVC
- xUnit (unit and integration testing)

## Building and running on your workstation

### Build the solution

From the repository root:

```bash
dotnet build
```

Note that the project uses a [Directory.Build.props](/Directory.Build.props) with `<UseArtifactsOutput />` so
build artifacts will land in `/artifacts`.

### Run locally

```bash
dotnet run --project src/AccessingChildcareEntitlementChecker.Web
```

The application will start on a local development URL (for example, `https://localhost:xxxx`).

### Devcontainer

The repo includes a [devcontainer](https://containers.dev/implementors/json_reference/) with prerequisites
as well as recommended extensions. This can be used in e.g. [vscode](https://code.visualstudio.com/docs/devcontainers/containers) 
or [rider](https://www.jetbrains.com/help/rider/Connect_to_DevContainer.html).

## Integration, Unit and E2E tests

### Unit tests

To run the unit tests:

```bash
dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests
```

### E2E tests

#### Installing dependencies

To run the E2E tests, you'll either need to use the devcontainer; 
or make sure you first install Playwright dependencies:

```powershell
dotnet tool restore
dotnet tool run playwright install
```

on Linux, you might want to also install any required system dependencies with

```bash
dotnet tool restore
dotnet tool run playwright install --with-deps
```

The devcontainer includes deps in the docker images, and installs the Playwright browsers via a `postCreateCommand`.

The above command using the dotnet tool works fine; but [the Playwright.net docs](https://playwright.dev/dotnet/docs/intro) suggest
that you need to install [Powershell 7](https://learn.microsoft.com/en-gb/powershell/scripting/install/install-powershell?view=powershell-7.5) and
then once you've built the project you can run
`.\tests\AccessingChildcareEntitlementChecker.Tests.E2e\bin\Debug\net8.0\playwright.ps1 install`.

#### Running tests

You can run E2E tests against a local instance, or a deployed version of the web app
by specifying a `TEST_URL` environment variable:

```powershell
$env:TEST_URL="http://localhost:5252/"; dotnet test tests/AccessingChildcareEntitlementChecker.Tests.E2e/
```

```bash
TEST_URL="http://localhost:5252/"; dotnet test tests/AccessingChildcareEntitlementChecker.Tests.E2e/
```

The browser window will be visible on the desktop, but they are configured to run headlessly in GitHub actions - see `PlaywrightHooks.cs`.

The tests only run against Chromium. _TODO: browser matrix is pending_

## CI/CD

See the workflow at `.github/workflows/pr-checks.yml` which builds the solution and runs tests.

CD is pending implementation - this will be to an Azure Web App via Terraform.

## More tips

### `dotnet`

You may want to restore tools using `dotnet tool restore`

- use `dotnet format` to lint the codebase. This will be checked in QA. See below for setting up a Git hook.
- To update packages use `dotnet tool run dotnet-outdated -u` (or Visual Studio)

### Visual Studio

Visual Studio 2026 is the recommended version.

- the project will build out of the box. use F5 to run in the debugger, or Ctrl-F5 to run without debugging.
- You can use the test explorer to run tests
  - To run the E2E tests, you'll need to start the app without debugging first.
  - you may also need to set an environment variable BEFORE starting Visual Studio
- Recommended extensions:
  - [Reqnroll](https://marketplace.visualstudio.com/items?itemName=Reqnroll.ReqnrollForVisualStudio2022)

#### Visual Studio Code

- See the [devcontainer.json](/.devcontainer/devcontainer.json) for recommended extension ids

#### Git

You can add a local Git `pre-push` hook to run formatting checks before code is pushed.

Create `.git/hooks/pre-push` with:

```bash
#!/usr/bin/env bash
set -euo pipefail

echo "Running dotnet format..."
dotnet format AccessingChildcareEntitlementChecker.sln --verify-no-changes --no-restore
```

Then make it executable:

```bash
chmod +x .git/hooks/pre-push
```

If formatting issues are found, the push is blocked until they are fixed.
