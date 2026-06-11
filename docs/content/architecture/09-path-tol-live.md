---
title: Path to Live
layout: sub-navigation
sectionKey: Architecture
order: 9
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Architecture
  key: Path to Live
---
The "Path to Live" describes the journey of code changes from a developer's local machine to the production environment, ensuring robust quality assurance, automated regression testing, and controlled promotions.

The diagram below outlines the sequential phases and quality gates:

```mermaid
sequenceDiagram
    autonumber
    actor Dev as Developer
    participant Repo as GitHub Repository
    participant Pipeline as GitHub Actions (CI/CD)
    participant DevEnv as Dev & Test Environments
    participant StageEnv as Staging Environment
    participant ProdEnv as Production Environment

    %% 1. Local Development & Integration
    rect rgb(240, 248, 255)
        note right of Dev: Phase 1: Integration (main)
        Dev->>Repo: Create feature branch & push commits
        Repo->>Pipeline: Trigger CI (Build, Unit Tests)
        Pipeline-->>Repo: CI Status Passed
        Dev->>Repo: Merge Pull Request into main
    end

    %% 2. Continuous Deployment to Dev/Test
    rect rgb(245, 245, 245)
        note right of Dev: Phase 2: Lower Env Verification
        Repo->>Pipeline: Trigger CD on main branch
        Pipeline->>DevEnv: Apply Terraform & Deploy Web App (.NET Zip)
        DevEnv-->>Pipeline: Deployment Successful
    end

    %% 3. Release Stabilization (Staging)
    rect rgb(255, 250, 240)
        note right of Dev: Phase 3: Stabilization (Staging)
        Dev->>Repo: Create release branch (releases/vX.Y)
        Repo->>Pipeline: Trigger Release Pipeline (workflow-release.yml)
        Pipeline->>Repo: Determine next patch version & push Git Tag
        Pipeline->>Pipeline: Build dotnet & Package Infra
        Pipeline->>StageEnv: Apply Terraform & Deploy Web App (Staging)
        Pipeline->>StageEnv: Run Playwright E2E Tests
        StageEnv-->>Pipeline: E2E Tests Pass
    end

    %% 4. Promotion to Production
    rect rgb(240, 255, 240)
        note right of Dev: Phase 4: Production Release
        Dev->>Pipeline: Approve Production Release
        Pipeline->>ProdEnv: Deploy Web App (Parity-verified tag)
        Pipeline->>Repo: Generate GitHub Release for tag
    end
```

### Local Development
* Developers implement features and bug fixes in short-lived feature branches created from `main`.
* Local testing is performed including executing unit/component tests and linting.
* Changes are submitted via a Pull Request (PR) to `main`.

### Integration & Continuous Deployment (Dev & Test)
* Quality Gate: Raising a PR triggers the Continuous Integration (CI) pipeline, executing builds, unit tests, and security scans.
* Upon merging into `main`, GitHub Actions automatically:
    1. Build and compile the ASP.NET Core package.
    2. Apply infrastructure changes using Terraform.
    3. Deploy the application package to both Development and Test environments.
* Continuous feedback is provided to the team as these environments always run the latest integrated code.

### Release Stabilization (Staging)
* When a set of features is ready for release, a release branch is branched off `main` following the naming convention `releases/vX.Y` (where `X.Y` corresponds to the target Major.Minor release version).
* Pushing to a `releases/` branch triggers the Release Pipeline (`workflow-release.yml`):
    * Automatic Versioning: The pipeline validates the branch name, fetches git tags, determines the next patch version (e.g., `v1.2.0` or incrementing to `v1.2.1`), and automatically creates and pushes the tag to GitHub.
    * Build & Infrastructure packaging: Builds the .NET application zip and packages the Terraform infrastructure configurations.
    * Staging Deployment: The Terraform configurations are applied and the zip package is deployed to the Staging environment.
    * Automated E2E Verification: Playwright integration/E2E regression tests are executed automatically against the active Staging URL to ensure functional integrity.

### Promotion to Production
* Once the release candidate is fully validated in Staging (incorporating E2E testing, accessibility audits, and stakeholder/UAT sign-offs):
    * Production Deployment: The release package is promoted and deployed to the Production environment (using the matching version tag to ensure exact artifact parity).
    * *Note: Continuous automated deployment to production and automated accessibility checking are currently built into the pipeline structure and can be fully promoted following manual sign-off.*
    * GitHub Release: A formal GitHub Release is generated for the successful deployment with the corresponding version tag.