# E2E Tests

## Building

use `dotnet build`

Unfortunately Playwright.NET does not have a great DX and you need to
install it manually. See [the docs](https://playwright.dev/dotnet/docs/intro) for more information.

### Windows

You'll need to make sure you have 
[Powershell 7](https://learn.microsoft.com/en-gb/powershell/scripting/install/install-powershell?view=powershell-7.5) and 
then once you've built the project you can run
`.\tests\AccessingChildcareEntitlementChecker.Tests.E2e\bin\Debug\net8.0\playwright.ps1 install` to install Playwright.

After this you can run 

