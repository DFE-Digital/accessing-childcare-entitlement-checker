# E2E Tests

## Building

use `dotnet build` or build in Visual Studio as usual.

The playwright docs seem to suggest that you must install Powershell to install playwright.net stubs; but in fact
you can use the dotnet tool, which is in the tools manifest for this project.

So to install the playwright deps you just need to run 

```
dotnet tool restore
dotnet tool run playwright install
```

on Linux, you might want to also install any required system dependencies with

```
dotnet tool restore
dotnet tool run playwright install --with-deps
```

_TODO: This seems to take a long time to run in the devcontainer; it should just include it as part of the image._

Otherwise, See [the docs](https://playwright.dev/dotnet/docs/intro) for more information - you may
need to install [Powershell 7](https://learn.microsoft.com/en-gb/powershell/scripting/install/install-powershell?view=powershell-7.5) and 
then once you've built the project you can run
`.\tests\AccessingChildcareEntitlementChecker.Tests.E2e\bin\Debug\net8.0\playwright.ps1 install`.

## Running

Run using `dotnet test` on the command line, or in Visual Studio you can run them from the test explorer.

They currently run against the test deployment at https://sjm-test-webapp-feature-initial-gith-c02a9811.azurewebsites.net/ but you can alter this url in the test file

_TODO: needs to be some sort of env var that devs can set on workstations?_

They are configured to run headlessly, but if you want to watch them; change the boolean in `PlaywrightHooks.cs`.

_TODO: should this be a build flag?_

The tests only run against Chromium. _TODO: browser matrix is pending_
