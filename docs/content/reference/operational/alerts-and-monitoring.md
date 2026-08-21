---
title: Alerts and Monitoring Runbook
layout: sub-navigation
order: 10
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
   parent: Operational
---

This reference outlines the Azure Monitor alerts for the Accessing Childcare Entitlement Checker service. It details thresholds, meanings, and recommended troubleshooting procedures.

## Configuration & Environment Targeting

The team conditionally deploys alerts in Azure using the following Terraform variables:
- `enable_alerts`: A boolean flag (default `false`) to enable alerting in specific environments, such as Production.
- `alert_email_address`: The email address where the system routes all alert notifications using the `email_action_group` Action Group.

## Web Test Availability Alert

- Severity: 1 (Critical)
- Metric: `availabilityResults/availabilityPercentage` (Application Insights)
- Threshold: `< 100%` average over 5 minutes.
- Meaning: The synthetic ping test to the public-facing URL has failed. The application is likely completely down, unreachable, or returning non-200 responses.

### Investigation Procedures
1. Verify the endpoint status by accessing the public endpoint directly via a browser or `curl`.
2. Check the health and status of Azure Front Door to ensure gateway operation.
3. Verify the App Service status via the Azure Portal. Check for CPU or memory spikes.
4. Run Application Insights log queries to identify failed requests (`success == false`) or unhandled exceptions.

## High Response Time Alert

- Severity: 3 (Informational/Warning)
- Metric: `requests/duration` (Application Insights)
- Threshold: `> 2000 ms` (2 seconds) average over 5 minutes.
- Meaning: The average server response time is abnormally high. Users may experience slow page loads.

### Investigation Procedures
1. Analyse performance using the Application Insights Performance blade to identify slow operations.
2. Check external dependencies and database queries for potential latency.
3. Review CPU and memory metrics of the App Service Plan to identify potential resource exhaustion.
4. Inspect Application Insights Profiler traces, if enabled, to locate execution bottlenecks.

## High Exception Rate Alert

- Severity: 2 (Error)
- Metric: `exceptions/count` (Application Insights)
- Threshold: `> 10` unhandled exceptions over 5 minutes.
- Meaning: A spike in unhandled exceptions occurring in the application code.

### Investigation Procedures
1. Inspect failure logs under the Application Insights Failures menu.
2. Analyse error types by filtering exceptions. Examine the exception type, message, and call stack.
3. Correlate exceptions with recent deployments or releases.
4. Verify connectivity and pool utilisation if exceptions relate to database or external service access.

## App Service 5xx Errors Alert

- Severity: 1 (Critical)
- Metric: `Http5xx` (App Service)
- Threshold: `> 10` errors over 5 minutes.
- Meaning: The App Service web server is returning 5xx server errors to clients.

### Investigation Procedures
1. Review Application Insights request and exception logs to determine the cause of 5xx errors.
2. Inspect App Service web server logs (Diagnostic Logs) via the Log Stream or Kudu Console if Application Insights lacks telemetry.
3. Verify whether the dotnet process has crashed or is recycling via the Kudu Event Viewer or Crash Diagnoser.

## High CPU Usage Alert (App Service Plan)

- Severity: 2 (Error)
- Metric: `CpuPercentage` (App Service Plan)
- Threshold: `> 80%` average over 5 minutes.
- Meaning: The CPU utilisation on the App Service Plan is dangerously high, which can lead to application sluggishness or timeouts.

### Investigation Procedures
1. Determine CPU usage allocation among apps sharing the App Service Plan if the environment hosts multiple applications.
2. Review App Service metrics, including thread count, request count, and individual instance CPU usage.
3. Scale out by adding instances or scale up by upgrading the SKU tier to accommodate legitimate sustained load.
4. Check performance traces in Application Insights to identify potential infinite loops or heavy garbage collection activity.

## High Memory Usage Alert (App Service Plan)

- Severity: 2 (Error)
- Metric: `MemoryPercentage` (App Service Plan)
- Threshold: `> 80%` average over 5 minutes.
- Meaning: Memory usage on the App Service Plan hosting the application has exceeded 80%.

### Investigation Procedures
1. Monitor memory trends to determine if consumption grows steadily, indicating a memory leak, or spikes suddenly.
2. Restart the App Service as a quick mitigation step to recover memory.
3. Collect memory dumps via Azure App Service Diagnostics (Diagnostic Tools -> Collect Memory Dump). Analyse them to identify persistent leaks.

## WAF Blocked Requests Alert

- Severity: 3 (Informational/Warning)
- Metric: `WebApplicationFirewallRequestCount` where Action = `Block` (Front Door Profile)
- Threshold: `> 50` blocks over 5 minutes.
- Meaning: The Azure Front Door Web Application Firewall (WAF) actively blocks an elevated number of requests. This indicates a potential scan, security attack, or a false positive.

### Investigation Procedures
1. Check WAF logs by executing a Kusto query on the `AzureDiagnostics` table in the Log Analytics Workspace:
   ```kusto
   AzureDiagnostics
   | where ResourceProvider == "MICROSOFT.CDN" and Category == "FrontDoorWebApplicationFirewallLog"
   | where action_s == "Block"
   | take 100
   ```
2. Assess request patterns. Check if blocked requests originate from a single IP address (indicative of a scan) or multiple users (suggesting a false positive).
3. Configure rule exclusions if WAF rules block legitimate application traffic.
