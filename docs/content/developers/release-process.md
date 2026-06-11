---
title: Release Process
layout: sub-navigation
sectionKey: Developers
eleventyNavigation:
  parent: Developers
  key: Release Process
  order: 3
---

This document describes the operational release process for the Accessing Childcare Entitlement Checker. It acts as the operational and governance counterpart to the [Branching Strategy](branching-strategy.md) git workflow, ensuring that changes are safely promoted, validated, and communicated to stakeholders.

---

## Overall Release Steps

The release process is broken down into five distinct phases, moving code from integration to live operation.

```text
  Phase 1: Planning   ──>   Phase 2: Staging     ──>   Phase 3: Sign-off    ──>   Phase 4: Production  ──>  Phase 5: Post-Release
  & Preparation             & Auto-Validation          & Approvals               Deployment                & Cleanup
```

### Phase 1: Planning and Preparation

1. **Identify the Scope**: Review the merged pull requests on the `main` branch since the last release. Group features and bug fixes into a logical release version.
2. **Version Assignment**: Determine the next version number using Semantic Versioning (`vX.Y.Z` or `vX.Y`).
3. **Create the Release Branch**:
   - Create a release branch named `release/vX.Y` from the latest stable commit on `main`.
   - Command:
     ```bash
     git checkout main
     git pull
     git checkout -b release/vX.Y
     git push -u origin release/vX.Y
     ```
4. **Compile Draft Release Notes**: Document the changes, including linked Jira/GitHub issue numbers, user-facing impact, and any technical notes or configuration changes.

### Phase 2: Staging and Automated Validation

1. **Trigger Staging Deployment**: Pushing to the `release/vX.Y` branch triggers the GitHub CI/CD workflow, which builds release artifacts and deploys them automatically to the **Staging** environment.
2. **Automated Verification**:
   - Once deployed, the automated End-to-End (E2E) test suite runs using Playwright against Staging.
   - Automated accessibility checks are executed against Staging to ensure compliance with digital standards.
   - Verify that the automated pipeline completes successfully with zero critical or high-severity failures.

### Phase 3: Manual Testing and Approvals

While automated tests provide safety, manual validation ensures the service meets user and operational requirements:

1. **User Acceptance Testing (UAT)**: The Product Owner and business testers review the new features on Staging to ensure they meet the defined acceptance criteria.
2. **Exploratory & Regression Testing**: Conduct targetted manual testing of critical paths (such as the eligibility calculator flow) to ensure no regressions have been introduced.
3. **Security Check**: Check that any weekly OWASP ZAP security scan reports have been reviewed, and that no new high/medium alerts are unresolved.
4. **Sign-Off Acquisition**: Collect and log formal approvals from key roles (see [Approvals](#approvals) below).

### Phase 4: Production Deployment

1. **Schedule the Release Window**: Ensure the deployment is scheduled during an approved operational window (preferably low-traffic periods) and does not clash with critical policy change dates.
2. **Deploy to Production**:
   - Trigger the deployment workflow in GitHub Actions, targeting the stabilized `release/vX.Y` branch.
   - Monitor the deployment progress, logs, and system metrics closely during the rollout.
3. **Smoke Testing**: Once the deployment completes, the delivery and engineering team must perform a quick, non-destructive smoke test of the live service to confirm core functionality (such as loading the landing page and verifying basic site elements).

### Phase 5: Post-Release and Cleanup

1. **Tag the Release**: Tag the specific release commit on the release branch to mark the production deployment.
   - Commands:
     ```bash
     git checkout release/vX.Y
     git pull
     git tag -a vX.Y.Z -m "Release vX.Y.Z"
     git push origin vX.Y.Z
     ```
2. **Reconciliation (Cherry-Pick / Merge Back)**:
   - If any bug fixes or configuration changes were made directly on the `release/vX.Y` branch during the stabilization phase, they **must** be cherry-picked or merged back into `main` via PR to prevent codebase drift.
3. **Close Release Activities**: Update the corresponding Jira release page, close GitHub Milestones, and archive the release notes.

---

## Approvals

To ensure safety and quality, a release must pass three gates before it can be deployed to the live production environment. Each role is responsible for a specific aspect of system health.

### 1. Technical Sign-Off
* **Owner**: Lead Engineer / Technical Lead
* **Verification Scope**:
  - All automated unit, component, and E2E checks passed.
  - No critical/high static analysis warnings or dependency alerts (Dependabot) are outstanding.
  - Architectural patterns have been followed and documented where necessary.
  - Active security scans (OWASP ZAP) show no high-risk vulnerabilities.

### 2. Product Sign-Off
* **Owner**: Product Owner / Product Manager
* **Verification Scope**:
  - Features meet user expectations and functional specifications.
  - UX design conforms to GDS and DfE standards.
  - User Acceptance Testing (UAT) is complete and signed off by business stakeholders.
  - Release-specific content, guidance text, or legal references are accurate.

### 3. Operations & Delivery Sign-Off
* **Owner**: Delivery Manager / Service Owner
* **Verification Scope**:
  - The deployment is scheduled for an approved window.
  - Communication channels are prepped and stakeholders are aware of potential service updates.
  - Runbooks and operational documentation are up-to-date.
  - Support/helpdesk teams have been informed of upcoming user-facing changes.

### Sign-Off Matrix

| Role | Gate | Prerequisite for |
| :--- | :--- | :--- |
| **Technical Lead** | Technical Sign-Off | Transition to UAT & Prod Deployment |
| **Product Owner** | Product Sign-Off | Prod Deployment |
| **Delivery Manager** | Release Schedule Sign-Off | Prod Deployment |

---

## Communication Plans

Clear communication ensures that internal teams, external stakeholders, and users are aligned and prepared for releases.

### 1. Pre-Release Notification (T-24 Hours)
* **Goal**: Inform internal stakeholders, client support teams, and delivery partners of the upcoming deployment.
* **Channels**: Dedicated Slack/Teams channels (e.g., `#announcements`, `#dev-team`) and email.
* **Content Template**:
  ```text
  📢 RELEASE NOTICE: Accessing Childcare Entitlement Checker vX.Y.Z
  
  We are planning to deploy version vX.Y.Z to Production on [Date] at [Time] (UTC).
  
  ⏱ Expected Window: [Start Time] to [End Time] (Approx. [Duration] mins)
  ⚠️ Service Impact: [No downtime expected / Brief intermittent access may occur]
  
  What's New in this Release:
  • [Feature Title] - Brief description (Jira-XX)
  • [Feature Title] - Brief description (Jira-YY)
  • [Fix Title] - Description of critical bug resolution (Jira-ZZ)
  
  Key Stakeholders:
  • Tech Lead: [Name]
  • Product Owner: [Name]
  • Delivery Lead: [Name]
  
  If you have questions or concerns, please contact the team in this channel.
  ```

### 2. Deployment Start (T-0)
* **Goal**: Notify technical and operations teams that the deployment is underway.
* **Channels**: Operational Slack/Teams channels (e.g., `#ops`, `#dev-team`).
* **Content**:
  ```text
  🚀 DEPLOYMENT IN PROGRESS: Deploying vX.Y.Z to Production.
  Artifact source: release/vX.Y (Commit [Hash])
  Monitoring dashboards: [Link to Azure Dashboards/Application Insights]
  ```

### 3. Release Confirmation (Post-Deployment)
* **Goal**: Confirm successful rollout and provide a complete summary of the deployed capabilities.
* **Channels**: `#announcements` Slack/Teams channel, plus a summary email to business sponsors.
* **Content Template**:
  ```text
  ✅ RELEASE SUCCESSFUL: Accessing Childcare Entitlement Checker vX.Y.Z is Live!
  
  The deployment completed successfully at [Time]. All automated smoke tests and live verification checks have passed.
  
  🔗 Live Service URL: https://[service-url].gov.uk
  📄 Full Release Notes & Changelog: [Link to Docs/GitHub Release page]
  
  Thank you to the engineering, product, and delivery teams for making this release possible!
  ```

---

## Emergency / Hotfix Releases

When a critical production defect is identified (e.g., service outage, security vulnerability, or critical policy miscalculation), the release process is streamlined for speed while preserving safety:

1. **Authorization**: An emergency meeting is convened with the Tech Lead and Product Owner to agree on the fix scope and authorize an emergency hotfix.
2. **Branching**: A fix is developed on a `hotfix/*` branch off the active release branch as described in the [Hotfix Flow](branching-strategy.md#hotfix-flow).
3. **Promotion**:
   - The fix is merged into the active `release/vX.Y` branch.
   - It is automatically deployed to Staging and validated via Playwright automated tests.
4. **Approval Shortcut**: The technical sign-off and product sign-off can be granted concurrently on the PR itself to fast-track deployment.
5. **Production Push**: The updated release branch is deployed immediately to Production.
6. **Reconciliation**: Immediately after the production deployment, the hotfix is cherry-picked back to `main` to ensure the master branch is not drifted.
