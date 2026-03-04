# E2E Tests

## Building

use `dotnet build` or build in Visual Studio as usual.

The playwright docs seem to suggest that you must install Powershell to install playwright.net stubs; but in fact
you can use the dotnet tool, which is in the tools manifest for this project.

So to install the playwright deps you just need to run:

```powershell
dotnet tool restore
dotnet tool run playwright install
```

on Linux, you might want to also install any required system dependencies with

```bash
dotnet tool restore
dotnet tool run playwright install --with-deps
```

_TODO: This seems to take a long time to run in the devcontainer; it should just include it as part of the image._

Otherwise, See [the docs](https://playwright.dev/dotnet/docs/intro) for more information - you may
need to install [Powershell 7](https://learn.microsoft.com/en-gb/powershell/scripting/install/install-powershell?view=powershell-7.5) and
then once you've built the project you can run
`.\tests\AccessingChildcareEntitlementChecker.Tests.E2e\bin\Debug\net8.0\playwright.ps1 install`.

## Running

```powershell
$env:TEST_URL = "http://localhost:5252/"; dotnet test --filter "FullyQualifiedName~E2e"
```

```bash
TEST_URL = "http://localhost:5252/"; dotnet test --filter "FullyQualifiedName~E2e"
```

In Visual Studio you can run them from the test explorer, but you'll have to set the URL in the
environment before starting Visual Studio, or else change the hard coded default.

The browser window will be visible on the desktop, but they are configured to run headlessly in GitHub actions - see `PlaywrightHooks.cs`.

The tests only run against Chromium. _TODO: browser matrix is pending_
