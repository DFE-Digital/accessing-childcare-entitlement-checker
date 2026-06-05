---
title: OWASP ZAP Security Scanning Guide
layout: sub-navigation
sectionKey: Developers
eleventyNavigation:
  parent: Developers
  key: ZAP Scanning
order: 5
---

This project uses OWASP ZAP (Zaproxy) to perform automated Dynamic Application Security Testing (DAST) against the development environment. The scan is designed to run automatically in CI/CD to identify common web vulnerabilities (XSS, SQLi, missing security headers, etc.) before they reach production.

## Key Components

| File                                  | Purpose                                                           |
|:--------------------------------------|:------------------------------------------------------------------|
| `.github/workflows/workflow-zap-scan.yml` | The GitHub Actions workflow orchestrating the scan.               |
| `zap/automation-plan.yaml`            | The ZAP Automation Framework plan (defines scan jobs).            |
| `zap/auth-header.js`                  | JavaScript hook to inject Basic Auth and bypass WAF rules.        |
| `zap/findings.json`                   | A stable snapshot used to prevent unnecessary Pull Request noise. |

## The Workflow

### Triggers
- Scheduled: Every Monday at 2:00 AM UTC.
- Manual: Can be triggered via the "Actions" tab in GitHub.

### Execution Steps
1. Dynamic Target Resolution: The workflow queries Azure CLI to find the current Front Door hostname (handling both custom domains and default endpoints).
2. Basic Auth Injection: It builds a `Basic Auth` header using the `DEVELOPMENT_BASIC_AUTH_PASSWORD` secret.
3. ZAP Scan: Runs the ZAP Docker container using the Automation Plan.
4. SARIF Post-processing: Converts absolute `https` URLs in the report into relative paths so GitHub Code Scanning can process them.
5. Change Detection: Compares the results against `zap/findings.json`.

## Security & Infrastructure

### Bypassing WAF Geo-Blocking
The development environment is protected by an Azure WAF that blocks traffic from outside the UK. Since GitHub runners operate globally, the scan uses a custom User-Agent:
- User-Agent: `OWASP-ZAP-Automation`

The WAF is configured with an exception rule to allow this specific User-Agent regardless of the source IP.

### Authentication
The scan authenticates using Basic Auth. The `zap/auth-header.js` script runs on every request sent by ZAP to inject the `Authorization` header.

## Managing Findings

### GitHub Security Tab
The primary "Source of Truth" for security alerts is the Security -> Code scanning tab in the GitHub repository. All ZAP findings are uploaded there as SARIF alerts.

### Reducing PR Noise (`findings.json`)
To prevent a Pull Request from being created on every single run (which happens due to timestamps or minor path variations), we use `zap/findings.json`.

A Pull Request is only generated if:
- A new type of vulnerability is found.
- An existing vulnerability is fixed.
- The count of occurrences for a specific rule changes.

### Reviewing a Security PR
If you receive an automated PR titled `security: OWASP ZAP security findings updated`:
1. Check `docs/testing/zap-report.md` for the human-readable summary.
2. Visit the Security tab in GitHub to see the detailed breakdown of the alerts.
3. Merge the PR to update the baseline snapshot once the findings have been acknowledged or addressed.

## Troubleshooting

### "Target URL not found"
Ensure the Azure Service Principal has permissions to list Front Door resources in the `development` environment.

### "404 Error in Spider"
This usually indicates the WAF has blocked the scan and redirected it to a non-existent "Service Unavailable" page. Verify that the User-Agent in `zap/auth-header.js` matches the exception rule in `infra/terraform/fromtdoor.tf`.

### "SARIF URI Scheme Mismatch"
This happens if the `Post-process SARIF` step fails. GitHub expects relative paths or `file://` schemes; absolute `https://` URLs will be rejected.
