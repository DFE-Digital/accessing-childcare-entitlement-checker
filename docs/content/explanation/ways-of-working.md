---
title: Ways of working
layout: sub-navigation
sectionKey: Explanation
order: 1
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Explanation
  key: Ways of working
---
This guide shows the engineering principles, architectural designs, and team rules for the project. These rules help the team keep high code quality and constant delivery readiness.

## Engineering standards and conventions

The development environment uses a modern, stable stack and clear patterns. These patterns help separate different code parts and keep the code easy to read and maintain.

### Technology stack
* **Runtime**: .NET 10.0
* **Web Framework**: ASP.NET Core MVC
* **Testing Ecosystem**: xUnit, NSubstitute, Reqnroll (Gherkin), and Playwright
* **Infrastructure**: Terraform and Azure cloud services

### Architectural integrity and code quality
* **Automatic checks**: The build system runs code style and quality checks at compile time using .NET analysers (`EnforceCodeStyleInBuild`). This makes code reviews easy.
* **Standards**: The project follows standard C# rules and uses Government Design System (GDS) patterns on the frontend to make sure the interface is accessible.
* **Separate parts**: The codebase separates the stateless `RulesEngine` (core logic) from the stateful `Web` application (sessions and views).

### Routing conventions
To make navigation easy, the application uses centralised routing in [WebApplicationExtensions.cs](/src/AccessingChildcareEntitlementChecker.Web/WebApplicationExtensions.cs). We do not use attribute-based routing on single controllers. Centralised routing gives you one file to see and update the full user journey.

## Source control and history curation

The project uses Git to keep a clear history of changes. This history follows the trunk-based development model shown in the [Branching strategy](/explanation/branching-strategy/) guide.

### Branch taxonomy
We categorise branch names by prefix to show their purpose:
* **Feature Development**: `feature/description`
* **Defect Remediation**: `fix/description`
* **Documentation updates**: `docs/description`
* **Releases**: `releases/vX.Y`

### Semantic commit history
To help maintain the codebase, commit your changes in small, complete units. Use the format `<type>: <description>` for commit messages. This makes the history easy to read and helps generate automatic logs:

* `feat`: A new feature or capability
* `fix`: A resolution to an identified issue
* `docs`: Updates strictly to documentation files
* `style`: Structural improvements that do not alter execution behavior (such as formatting or whitespace adjustments)
* `refactor`: Structural changes that improve maintainability without adding features or fixing bugs
* `test`: Adding missing tests or correcting existing tests
* `chore`: Maintenance of the build pipeline, dependencies, or auxiliary tools

*Example*: `feat: add logic for 15 hours universal entitlement`

## The pull request lifecycle

To merge code into the `main` or `releases/*` branches, you must use a pull request (PR). Pull requests are spaces for team reviews and quality checks.

### Quality gates and requirements
You must meet these conditions before you merge a PR:
* **CI checks**: The automatic build and all unit and component tests must pass.
* **Team review**: At least one maintainer must review and approve the PR.
" **Test coverage**: All new features and changed logic must be verified by automated tests. The type of test should be proportionate to the change.
* **Documentation**: You must update the documentation in `/docs/content/` when you change the architecture or workflows.

### Integration process
1. **Open a PR**: Open a PR that targets the `main` branch.
2. **Add details**: Write a clear description and add links to tracking tickets.
3. **Merge**: When all checks pass, merge the PR. We use "Squash and Merge" to keep a clean, linear history.

## Validation and testing paradigm

We verify software quality using the layered tests shown in the [Test strategy](/explanation/test-strategy/) guide.

* **Continuous checks**: Unit and component tests run automatically on each pull request.
* **E2E tests**: We run E2E tests to validate full user journeys.
* **Accessibility (A11y)**: We integrate accessibility checks directly into our E2E tests.
* **Security checks**: We run weekly OWASP ZAP scans against the Test environment. We run Checkov checks on pull requests to find infrastructure issues, and JetBrains inspectcode in the CI pipeline to upload static analysis findings to GitHub Code Scanning.

## Deployment and delivery workflows

We use automation to build, test, and release the software safely and quickly.

### Automated continuous integration (CI)
Each pull request starts the `workflow-pr.yml` pipeline. This pipeline compiles the code, runs tests, and checks the code structure.

### Continuous delivery (CD) pipelines
* **Continuous integration environments**: Merges into `main` trigger automatic deployments to the Development and Test environments.
* **Pre-Production staging**: The system deploys release candidates from `releases/*` branches to the Staging environment. We use this environment for user tests, accessibility audits, and E2E tests.
* **Production**: We deploy to the Production environment only from a stable, tested release branch.

## Documentation as code

We treat documentation as part of the codebase.
* **Format**: We write documentation in Markdown. We write diagrams in Mermaid format to keep them version-controlled.
* **Decisions**: We record architectural choices as Architectural decision records (ADRs) in the `docs/content/decisions` folder.
