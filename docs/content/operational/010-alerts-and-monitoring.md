---
title: Alerts and Monitoring Runbook
layout: page
showPagination: true
order: 10
sectionKey: Operational
includeInBreadcrumbs: true
eleventyNavigation:
   parent: Operational
---

This runbook describes the Azure Monitor alerts configured for the Accessing Childcare Entitlement Checker service. It provides guidance on what each alert means, its threshold, and the recommended steps to investigate and resolve issues when an alert triggers.

## Configuration & Environment Targeting

Alerts are conditionally deployed in Azure using the following Terraform variables:
- `enable_alerts`: A boolean flag (default `false`) used to enable alerting in specific environments (e.g., Production).
- `alert_email_address`: The email address where all alert notifications are routed via the configured Action Group (`email_action_group`).

## Web Test Availability Alert

- Severity: 1 (Critical)
- Metric: `availabilityResults/availabilityPercentage` (Application Insights)
- Threshold: `< 100%` average over 5 minutes.
- Meaning: The synthetic ping test to the public-facing URL has failed. The application is likely completely down, unreachable, or returning non-200 responses.

### Next Steps & Action
1. Confirm Status: Attempt to access the public endpoint directly via a browser or `curl`.
2. Check Azure Front Door: Check the health and status of Azure Front Door to ensure the gateway isn't experiencing an outage.
3. Inspect App Service: Go to the Azure Portal and check if the App Service is running. Check memory/CPU spikes.
4. View Logs: Query Application Insights for `requests` where `success == false` or inspect `exceptions` to see what is causing the failures.

## High Response Time Alert

- Severity: 3 (Informational/Warning)
- Metric: `requests/duration` (Application Insights)
- Threshold: `> 2000 ms` (2 seconds) average over 5 minutes.
- Meaning: The average server response time is abnormally high. Users may experience slow page loads.

### Next Steps & Action
1. Analyse Application Performance: Use Application Insights -> Performance blade to identify the slowest operations.
2. Database & External Dependencies: Check if there are slow external dependency calls (e.g., database queries or third-party APIs).
3. Resource Exhaustion: Check the App Service Plan CPU and memory metrics. Resource exhaustion can lead to queued requests and slow response times.
4. App Service Profiler: If enabled, check the Application Insights Profiler traces to find the exact line of code causing bottlenecks.

## High Exception Rate Alert

- Severity: 2 (Error)
- Metric: `exceptions/count` (Application Insights)
- Threshold: `> 10` unhandled exceptions over 5 minutes.
- Meaning: A spike in unhandled exceptions occurring in the application code.

### Next Steps & Action
1. Inspect Exceptions: Go to Application Insights -> Failures.
2. Analyse Error Types: Filter by exceptions and look at the exception type, message, and call stack.
3. Correlation: Correlate exceptions with recent deployments or releases.
4. Database Connectivity: If exceptions are related to database access, verify that connection pools are not exhausted and database is fully operational.

## App Service 5xx Errors Alert

- Severity: 1 (Critical)
- Metric: `Http5xx` (App Service)
- Threshold: `> 10` errors over 5 minutes.
- Meaning: The App Service web server is returning 5xx server errors to clients.

### Next Steps & Action
1. Check Application Insights: View Exception and Request logs to find the cause of 500/503 errors.
2. Web Server Logs: Enable and check App Service logs (Diagnostic Logs) via Log Stream or Kudu Console if Application Insights isn't receiving data.
3. Process Crash: Check if the web application process (dotnet) has crashed or is recycling. Check Kudu Event Viewer or Crash Diagnoser.

## High CPU Usage Alert (App Service Plan)

- Severity: 2 (Error)
- Metric: `CpuPercentage` (App Service Plan)
- Threshold: `> 80%` average over 5 minutes.
- Meaning: The CPU utilisation on the App Service Plan is dangerously high, which can lead to application sluggishness or timeouts.

### Next Steps & Action
1. Identify the Culprit: If multiple web apps share the plan, determine which app is consuming the CPU.
2. Check App Service Metrics: Check Thread Count, Request Count, and CPU usage on individual instances.
3. Scale Out / Scale Up: Consider scaling out (adding more instances) or scaling up (upgrading the SKU tier) if the load is legitimate and sustained.
4. Garbage Collection / Infinite Loops: Check Application Insights performance traces for potential infinite loops or heavy garbage collection phases.

## High Memory Usage Alert (App Service Plan)

- Severity: 2 (Error)
- Metric: `MemoryPercentage` (App Service Plan)
- Threshold: `> 80%` average over 5 minutes.
- Meaning: Memory usage on the App Service Plan hosting the application has exceeded 80%.

### Next Steps & Action
1. Monitor Memory Trends: Check if the memory is growing steadily (suggesting a memory leak) or if it spiked suddenly.
2. Restart the Web App: A quick mitigation to recover memory is to restart the App Service slot.
3. Investigate Memory Leaks: If the leak is persistent, collect a memory dump via Azure App Service Diagnostics (Diagnostic Tools -> Collect Memory Dump) and analyze it.

## WAF Blocked Requests Alert

- Severity: 3 (Informational/Warning)
- Metric: `WebApplicationFirewallRequestCount` where Action = `Block` (Front Door Profile)
- Threshold: `> 50` blocks over 5 minutes.
- Meaning: The Azure Front Door Web Application Firewall (WAF) is actively blocking an elevated number of requests. This could indicate a scan, a security attack (e.g., SQL injection, XSS), or a false positive after an application release.

### Next Steps & Action
1. Check WAF Logs: Run a Kusto query in Log Analytics Workspace on the `AzureDiagnostic` table (or Front Door WAF logs) to inspect blocked requests:
   ```kusto
   AzureDiagnostics
   | where ResourceProvider == "MICROSOFT.CDN" and Category == "FrontDoorWebApplicationFirewallLog"
   | where action_s == "Block"
   | take 100
   ```
2. Assess Request Pattern: Determine if the blocked requests are coming from a single IP address (possible attack/scanner) or multiple normal users (possible false positive).
3. Manage Rule Exclusions: If legitimate application traffic is being blocked, configure WAF exclusions for the offending rules.
