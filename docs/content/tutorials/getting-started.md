---
title: Getting started
layout: sub-navigation
sectionKey: Tutorials
order: 1
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Tutorials
  key: Getting started
---
Let's get your local environment set up! In this guide, we'll walk you through setting up your workspace and building the application. We'll also run it locally and run tests to verify everything works perfectly.

## What you'll need

First things first, let's make sure you have the right tools installed on your machine before we get started:

- **.NET SDK 10.0.3:** This project is pinned to this version. You can double-check this in our `global.json` file.
- **PowerShell 7 (pwsh):** We need this to run the Playwright browser installation scripts across different operating systems.
- **Node.js (LTS version):** Required if you want to preview or build our documentation site locally.
- **A handy IDE:** We highly recommend:
  - Visual Studio 2026
  - VS Code
  - JetBrains Rider

## 1. Set up your workspace

Let's get the repository set up and pull in the dependencies you need:

1. **Clone the repository:** Grab a copy of the code and clone it to your workstation.
2. **Restore local .NET tools:** We use a few local helper tools in this project. Run this command from the repository root to get them restored:
   ```bash
   dotnet tool restore
   ```
3. **Trust HTTPS development certificates:** To avoid security and privacy warnings in your browser when running the application locally over HTTPS, run:
   ```bash
   dotnet dev-certs https --trust
   ```
4. **Install Playwright browsers:** We use Playwright to run end-to-end (E2E) and accessibility (A11y) tests, and it needs its own browser binaries to work. Let's compile the project first, then run the generated PowerShell script to install those browsers:
   
   *Using PowerShell:*
   ```powershell
   dotnet build
   pwsh .\tests\AccessingChildcareEntitlementChecker.E2eTests\bin\Debug\net10.0\playwright.ps1 install
   ```

## 2. Build the solution

To compile and build the whole solution from the repository root, simply run:

```bash
dotnet build
```

*Friendly tip: This project uses `Directory.Build.props` configured with `<UseArtifactsOutput />`. This means all build artefacts are neatly placed under the `/artifacts` folder at the root, rather than cluttering up the individual project folders.*

## 3. Run the web application

Ready to see it in action? You can launch the web application locally with:

```bash
dotnet run --project src/AccessingChildcareEntitlementChecker.Web
```

Once it starts up, you'll see a local development URL in your terminal output (like `https://localhost:xxxx` or `http://localhost:xxxx`). Open that up in your browser to explore the application!

## 4. Verify your setup with tests

Let's run some tests to make sure everything is working as expected! Our test suite is divided into unit/component tests, integration tests, end-to-end (E2E) tests, and accessibility (A11y) tests.

### Unit & component tests
These tests are super fast! They run completely in-memory, so you don't need to have the web application running. They quickly verify our dependency injection, basic routing, and core business logic.

Give them a spin with:
```bash
dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests
```

### Integration tests
These tests verify how our application components interact with external dependencies (like our Redis cache or mock services). Like unit tests, they do not require you to manually launch the web application first.

Run them using:
```bash
dotnet test tests/AccessingChildcareEntitlementChecker.IntegrationTests
```

### End-to-end (E2E) & accessibility (A11y) tests
Both our E2E and A11y tests use Playwright to simulate real user journeys in a live browser. 

#### Configuring the test target
To run these tests, they need to know the local URL of your running web application. We configure this using an `appsettings.Local.json` file in the test folders so you don't have to specify environment variables every time.

Create an `appsettings.Local.json` file in both `tests/AccessingChildcareEntitlementChecker.E2eTests/` and `tests/AccessingChildcareEntitlementChecker.A11yTests/` with the following configuration:

```json
{
  "TestSettings": {
    "TestUrl": "https://localhost:xxxx" 
  }
}
```
*(Be sure to replace `https://localhost:xxxx` with the actual URL your web application runs on!)*

#### Running the E2E tests
Make sure your web application is running locally. Then, run the E2E tests in a separate terminal window:
```bash
dotnet test tests/AccessingChildcareEntitlementChecker.E2eTests --no-build
```

#### Running the accessibility (A11y) tests
Our A11y tests scan our page layouts against WCAG guidelines. Make sure your web application is running, then run:
```bash
dotnet test tests/AccessingChildcareEntitlementChecker.A11yTests --no-build
```

*Note: When running locally, you might see browser windows pop up as the tests run. When they run in our automated CI environment, they will run quietly in "headless" mode.*

## 5. Keep things clean with linting

To keep our codebase neat and consistent, we enforce formatting rules. You can use the built-in .NET formatter to check your code and automatically clean up any formatting quirks:

```bash
dotnet format
```

We validate these formatting rules automatically on pull requests, so running this command before you commit is a great habit to get into!
