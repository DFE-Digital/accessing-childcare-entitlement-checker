# Accessing Childcare Entitlement Checker

## Documentation requirements

Any future content creation, refactoring, or updates to markdown files must follow these standards:
- **Diátaxis framework:** Adhere to the directory-based tones and structures specified below.
- **Heading casing:** Use sentence case for all headings (e.g., "Local environment setup", not "Local Environment Setup").
- **UK English:** Use British spelling consistently (e.g., "minimise", "colour", "licence" as a noun).
- **Language:** All output must strictly follow ASD-STE100 (Simplified Technical English) guidelines.

### Directory standards (Diátaxis)

| Directory                                       | Tone & Goal                                                               | Key Guidelines                                                            |
|:------------------------------------------------|:--------------------------------------------------------------------------|:--------------------------------------------------------------------------|
| **Tutorials**<br>`docs/content/tutorials/`      | **Friendly & welcoming**<br>Guide newcomers step-by-step.                 | Use inclusive language ("we", "you"). Focus on learning by doing.         |
| **How-To Guides**<br>`docs/content/how-to/`     | **Action-driven & direct**<br>Solve a specific task or operational event. | Strip away theory. Use imperative/command headers and direct steps.       |
| **Explanations**<br>`docs/content/explanation/` | **Conceptual & contextual**<br>Build a robust mental model.               | Focus on "why". Reframe steps into system lifecycle explanations.         |
| **Reference**<br>`docs/content/reference/`      | **Austere & objective**<br>Provide scannable specifications.              | Neutral third-person (no pronouns). Organise data using tables and lists. |

## Local development and testing requirements

- You cannot use the Azure CLI locally.
- You cannot run Terraform Plan or Terraform Apply locally.
- Run the unit and integration test suites first.
- The E2E and A11y test suites require the web application to run.
- Run linting tools after you change any code. For example, run `terraform fmt` or `dotnet format`.
- Follow the branching and commit standards documented in `docs/content/explanation/ways-of-working.md`.
- Always restore NuGet packages in locked mode (`dotnet restore --locked-mode`) and restore local tools (`dotnet tool restore`) before building or formatting.
- You must run `dotnet build` before executing test targets, as the unit and integration tests are configured to run with `--no-build`.

## Agent operation requirements

- You must create a detailed plan before you change any code. Use "Plan Mode" if your platform has it available to draft and approve the plan before you start implementing changes.

