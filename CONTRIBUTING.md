# Contributing

Thank you for your interest in contributing to the Childcare Entitlement Checker! We welcome contributions of all kinds, including bug reports, feature suggestions, documentation updates, and code contributions.

By participating in this project, you agree to abide by our standards of conduct.

## Code of conduct

This project is governed by the DfE Code of Conduct. By participating, you are expected to uphold this code. Please ensure interactions remain professional, inclusive, and collaborative.

## Technical Documentation

We maintain a comprehensive, [Technical Documentation Site](https://dfe-digital.github.io/accessing-childcare-entitlement-checker/)

Please refer to the live documentation for the most up-to-date and in-depth guides:
- [Getting Started Guide](https://dfe-digital.github.io/accessing-childcare-entitlement-checker/developers/01-getting-started/) – Detailed environment setup, building, and running.
- [Ways of Working](https://dfe-digital.github.io/accessing-childcare-entitlement-checker/developers/02-ways-of-working/) – Development standards, code quality, and deployment workflows.
- [Branching Strategy](https://dfe-digital.github.io/accessing-childcare-entitlement-checker/developers/03-branching-strategy/) – Details on our trunk-based development model.

## Quick Getting Started

For a quick setup, ensure you have:
- .NET SDK 10.0.3 (pinned via `global.json`)
- PowerShell 7 (pwsh) (for running Playwright browser setup)

### Local Build & Test

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
   dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests
   ```

4. Format and lint code:
   ```bash
   dotnet format
   ```

## Branching and Commits

We use trunk-based development. Please branch off `main` using the following prefixes:
- `feature/description` for new features
- `fix/description` for bug fixes
- `docs/description` for documentation-only changes

### Commit Messages
We recommend using standard structured commit messages (`<type>: <description>`):
- `feat`: A new feature (e.g., `feat: add logic for 15 hours universal entitlement`)
- `fix`: A bug fix
- `docs`: Documentation-only changes
- `test`: Adding or correcting tests
- `refactor`: Code changes that neither fix bugs nor add features
- `chore`: Changes to build processes, tooling, or helper scripts

## Pull Requests

All code changes must be submitted via Pull Requests (PRs).

### PR Requirements
1. CI Verification: All builds, unit tests, and component tests must pass.
2. Review: At least one approval from a maintainer is required.
3. Tests: All new features or fixes must include unit tests and, where applicable, Reqnroll/Playwright E2E tests.
4. Documentation: Update relevant documents under `/docs/content/` if the change impacts architecture, features, or workflows.
5. Linear History: We use Squash and Merge on PR approval to maintain a linear git history.

### Creating a PR
1. Fork the repository and create your branch.
2. Commit your changes with clear, descriptive commit messages.
3. Push your branch to your fork and open a Pull Request against our `main` branch.
4. Respond to feedback on the PR thread. Once approved and checks pass, the PR can be merged.
