# Accessing Childcare Entitlement Checker

## Documentation
- Follow the Diátaxis structure and directory-specific tones defined in `docs/content/explanation/documentation-guide.md`.
- Use sentence case for headings.
- Use UK English.
- Use ASD-STE100 (Simplified Technical English).

## Development
- Create a detailed plan before changing code. Use Plan Mode when available.
- Follow the branching and commit standards in `docs/content/explanation/ways-of-working.md`.
- Do not use the Azure CLI locally.
- Do not run Terraform Plan or Terraform Apply locally.
- Use the Makefile targets instead of equivalent commands where possible.
- Restore NuGet packages with `dotnet restore --locked-mode`.
- Restore local tools with `dotnet tool restore`.

## Testing and analysis
- After changing C# code, run `make build` and `make test`.
- Before completing a substantial change, run `make verify`.
- Run `make test-e2e` or `make test-a11y` only when the change requires those tests and the web application is running.
- After changing Terraform, run `make tf-f` and `make tf-v`.
- After changing documentation, run the relevant documentation Makefile targets.
- Fix compiler and analyser errors before completing a change.
- Do not suppress analyser findings without a documented reason.
- Review the final diff before completing the task.
- Report any checks that could not be run and why.
