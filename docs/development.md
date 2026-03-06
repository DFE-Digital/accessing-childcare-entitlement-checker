# Development

## Status

Initial project scaffold - user research is ongoing.

This repository currently contains a minimal ASP.NET Core MVC application with associated unit and integration tests.

## Known Issues/In Progress

- Non functional!
- Does not yet comply with DFE standards as per https://technical-guidance.education.gov.uk/

## Tech Stack

- .NET 10
- ASP.NET Core MVC
- xUnit (unit and integration testing)

## Building, testing and running on your workstation

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

### Integration and Unit tests

To run all unit, integration and E2E tests from the command line, use:

```bash
dotnet test
```

You'll likely want to exclude the E2E tests from your regular runs, which can be done either
by running tests for a specific project, or using a filter.

```bash
dotnet test --filter "FullyQualifiedName!~E2e"
```


```powershell
dotnet test --filter "FullyQualifiedName!~E2e"
```

## CI/CD

See the workflow at `.github/workflows/build-and-test.yml` which builds the solution and runs tests.

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
- Recommended extensions:
  - [Reqnroll](https://marketplace.visualstudio.com/items?itemName=Reqnroll.ReqnrollForVisualStudio2022)

#### Visual Studio Code

- Recommended extensions:
  - "Cucumber (Gherkin) Full Support" `alexkrechik.cucumberautocomplete`

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
