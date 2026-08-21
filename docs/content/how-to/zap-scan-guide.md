---
title: OWASP ZAP security scanning guide
layout: sub-navigation
sectionKey: How-to guides
order: 5
includeInBreadcrumbs: true
eleventyNavigation:
  parent: How-to guides
  key: ZAP scanning

---
Use OWASP ZAP (Zaproxy) to perform automated Dynamic Application Security Testing (DAST) against the development environment and identify common web vulnerabilities (XSS, SQLi, missing security headers, etc.) before they reach production.

## Identify key scanning components

| File                                      | Purpose                                                           |
|:------------------------------------------|:------------------------------------------------------------------|
| `.github/workflows/workflow-zap-scan.yml` | Orchestrate the scan via this GitHub Actions workflow.            |
| `zap/automation-plan.yaml`                | Define scan jobs using this ZAP Automation Framework plan.        |
| `zap/auth-header.js`                      | Inject Basic Auth and bypass WAF rules using this JavaScript hook.|
| `zap/findings.json`                       | Prevent unnecessary Pull Request noise with this stable snapshot. |

## Execute the scan workflow

### Trigger the scan
* **Scheduled**: Run automatically every Monday at 2:00 AM UTC.
* **Manual**: Trigger the scan manually via the **Actions** tab in GitHub.

### Run the execution steps
1. **Resolve Dynamic Targets**: Query the Azure CLI to find the current Front Door hostname (handling both custom domains and default endpoints).
2. **Inject Basic Auth**: Build a `Basic Auth` header using the `DEVELOPMENT_BASIC_AUTH_PASSWORD` secret.
3. **Execute ZAP Scan**: Run the ZAP Docker container using the Automation Plan.
4. **Post-process SARIF**: Convert absolute `https` URLs in the report into relative paths so GitHub Code Scanning can process them.
5. **Detect Changes**: Compare the results against `zap/findings.json`.

## Configure security and infrastructure exceptions

### Bypass WAF geo-blocking
The development environment is protected by an Azure WAF that blocks traffic from outside the UK. Since GitHub runners operate globally, bypass geo-blocking by configuring a custom User-Agent:
- User-Agent: `OWASP-ZAP-Automation`

Configure an exception rule in the WAF to allow this specific User-Agent regardless of the source IP.

### Inject authentication headers
Authenticate the scan using Basic Auth. Run the `zap/auth-header.js` script on every request sent by ZAP to inject the `Authorization` header.

## Manage and resolve findings

### Review findings in the GitHub Security tab
Treat the **Security -> Code scanning** tab in the GitHub repository as the primary "Source of Truth" for security alerts. View and resolve all uploaded ZAP SARIF findings there.

### Minimise PR noise with findings.json
Avoid generating a Pull Request on every single run due to timestamps or minor path variations. Generate a Pull Request only when:
- The scan finds a new type of vulnerability.
- A previously recorded vulnerability is fixed.
- The occurrence count for a specific rule changes.

### Review an automated security PR
When you receive an automated PR titled `security: OWASP ZAP security findings updated`, perform these steps:
1. Open `docs/reference/testing/zap-report.md` to view the human-readable summary.
2. Navigate to the **Security** tab in GitHub to inspect the detailed breakdown of the alerts.
3. Merge the PR to update the baseline snapshot once you have acknowledged or addressed the findings.

## Troubleshoot scanning issues

### Resolve "Target URL not found"
Ensure the Azure Service Principal has permission to list Front Door resources in the `development` environment.

### Resolve "404 Error in spider"
If the WAF blocks the scan and redirects it to a non-existent "Service Unavailable" page, verify that the User-Agent in `zap/auth-header.js` matches the exception rule configured in `infra/terraform/frontdoor.tf`.

### Resolve "Sarif URI scheme mismatch"
Ensure the `Post-process SARIF` step completes successfully. GitHub expects relative paths or `file://` schemes; absolute `https://` URLs will be rejected.
