# Childcare Entitlement Checker

A digital service to help parents and carers check their entitlement for childcare support schemes.

This project will be a multi-step form. It's currently work in progress.

## Live examples

_TODO_

## Nomenclature

_TODO - this section may be a candidate for removal unless it means we should add domain nomenclature?_

## Technical documentation

This is an ASP.NET Core MVC C# application and should follow [Microsoft's coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).

You can use the `dotnet` command or Visual Studio 2026 to run the app.

The repo includes a [devcontainer](https://containers.dev/implementors/json_reference/) with prerequisites
as well as recommended extensions. This can be used in e.g. [vscode](https://code.visualstudio.com/docs/devcontainers/containers) 
or [rider](https://www.jetbrains.com/help/rider/Connect_to_DevContainer.html).

### Before running the app (if applicable)

- Install the [.NET SDK 10.0.3](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) - The projects is pinned to this SDK version with a `global.json`

Then run with `dotnet run --project src/AccessingChildcareEntitlementChecker.Web/`

### Running the test suite

Use `dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests/` to run unit tests and component tests.

To run the full test suite, including end to end (E2E) tests, you can:
- start a local server
- wait for the server to initialise
- run the tests
- kill the local server

Bash:
```bash
dotnet run --project src/AccessingChildcareEntitlementChecker.Web/ & pid=$!; sleep 5; dotnet test; kill $pid
```

Powershell:
```powershell
$proc = Start-Process dotnet -ArgumentList "run --project src/AccessingChildcareEntitlementChecker.Web" -PassThru; dotnet test; Stop-Process $proc
```

### Further documentation

- [CONTRIBUTING.md](/CONTRIBUTING.md) - Information for external contributors
- `docs/`
  - [development.md](/docs/development.md) - Detailed information on development

## Licence

[MIT LICENCE](./LICENCE)
