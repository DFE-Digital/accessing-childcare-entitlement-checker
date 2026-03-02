# Contributing

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Building the solution

### With dotnet

From the repository root:

```
dotnet build
```

## Run locally

```
dotnet run --project src/AccessingChildcareEntitlementChecker.Web
```

The application will start on a local development URL (for example, `https://localhost:xxxx`).

## Run tests

```
dotnet test
```

This will execute both unit and integration tests.

### With Visual Studio

This project is committed to an "F5 out of the box" experience, and tests are runnable via the test explorer.

## CI/CD

See the workflow at `.github/workflows/build-and-test.yml` which builds the solution and runs tests.

CD is pending implementation - this will be to an Azure Web App via Terraform.

## More tips

### Add a pre-push hook for linting

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