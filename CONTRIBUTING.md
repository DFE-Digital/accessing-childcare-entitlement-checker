# Contributing

Contributions to the Childcare Entitlement Checker are welcome. Accepted contributions include bug reports, feature suggestions, documentation updates, and code contributions.

Participation in this project requires adherence to the standards of conduct.

## Code of conduct

This project is governed by the DfE Code of Conduct. Participants must uphold this code. Interactions must remain professional, inclusive, and collaborative.

## Technical documentation

The project maintains a comprehensive [technical documentation site](https://dfe-digital.github.io/accessing-childcare-entitlement-checker/).

Refer to the live documentation for the most up-to-date and in-depth guides:
- [Getting Started](https://dfe-digital.github.io/accessing-childcare-entitlement-checker/tutorials/getting-started/) – Detailed environment setup, building, and running.
- [Ways of Working](https://dfe-digital.github.io/accessing-childcare-entitlement-checker/explanation/ways-of-working/) – Development standards, code quality, and deployment workflows.
- [Branching Strategy](https://dfe-digital.github.io/accessing-childcare-entitlement-checker/explanation/branching-strategy/) – Details on the trunk-based development model.

## Quick getting started

A quick setup requires:
- .NET SDK 10.0.3 (pinned via `global.json`)
- PowerShell 7 (pwsh) (for running Playwright browser setup)

### Local build & test

1. Clone and navigate to the repository:
   ```bash
   git clone git@github.com:DFE-Digital/accessing-childcare-entitlement-checker.git
   cd accessing-childcare-entitlement-checker
   ```

2. Restore .NET tools and build:
   ```bash
   dotnet tool restore
   dotnet build
   ```

3. Run Unit and Component Tests:
   ```bash
   dotnet test tests/Dfe.Acec.Tests.Unit
   ```

4. Format and lint code:
   ```bash
   dotnet format
   ```

## Branching and commits

The project uses trunk-based development. Branch off `main` using the following prefixes:
- `feature/description` for new features
- `fix/description` for bug fixes
- `docs/description` for documentation-only changes

### Commit messages
Use standard structured commit messages (`<type>: <description>`):
- `feat`: A new feature (e.g., `feat: add logic for 15 hours universal entitlement`)
- `fix`: A bug fix
- `docs`: Documentation-only changes
- `test`: Adding or correcting tests
- `refactor`: Code changes that neither fix bugs nor add features
- `chore`: Changes to build processes, tooling, or helper scripts

## Pull requests

All code changes must be submitted via Pull Requests (PRs).

### PR requirements
1. CI Verification: All builds, unit tests, and component tests must pass.
2. Review: At least one approval from a maintainer is required.
3. Tests: All new features or fixes must include unit tests and, where applicable, Reqnroll/Playwright E2E tests.
4. Documentation: Update relevant documents under `/docs/content/` if the change impacts architecture, features, or workflows.
5. Linear History: The project uses Squash and Merge on PR approval to maintain a linear git history.

### Creating a PR
1. Fork the repository and create a branch.
2. Commit changes with clear, descriptive commit messages.
3. Push the branch to the fork and open a Pull Request against the `main` branch.
4. Respond to feedback on the PR thread. The PR is merged after approval and successful checks.
