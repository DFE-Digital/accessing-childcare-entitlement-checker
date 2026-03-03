# Contributing

## Recommended Extensions

### Visual Studio

- The [Reqnroll](https://marketplace.visualstudio.com/items?itemName=Reqnroll.ReqnrollForVisualStudio2022) extension might be handy.

### Visual Studio Code

- Gherkin extension (?)

You may want to restore tools using `dotnet tool restore`

## Running Tests

You'll likely want to exclude E2E tests from a normal run.

```powershell
dotnet test --filter "FullyQualifiedName!~E2e"
```

```bash
dotnet test --filter "FullyQualifiedName!~E2e"
```
