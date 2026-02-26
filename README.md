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

See https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Alternatively install Visual Studio and select .NET 8 from the individual components tab.

You can verify installed SDKs with:

```
dotnet --list-sdks
```

### Build the solution

#### With dotnet

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

#### With Visual Studio

This project is committed to an "F5 out of the box" experience, and tests are runnable via the test explorer.

### CI/CD

See the workflow at `.github/workflows/build-and-test.yml`

CD is pending implementation - this will be to an Azure Web App via Terraform.