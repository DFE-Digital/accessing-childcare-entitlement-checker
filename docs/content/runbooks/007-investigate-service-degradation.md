---
title: Investigate service degradation
layout: page
showPagination: true
order: 7
sectionKey: Runbooks
eleventyNavigation:
  parent: Runbooks
---

This runbook describes the investigation steps to follow when the service is experiencing high latency, excessive 5xx errors, or general degradation.

## Step 1: Inspect High-Level Metrics
Log into the Azure Portal and navigate to the target Application Insights instance. Inspect the following charts over the degradation timeframe:
- Server Response Time: Is latency spiking across all endpoints or specific routes?
- Server Requests: Are request volumes significantly higher than normal (indicating potential load issues or scrapers)?
- Failed Requests: Is there a corresponding spike in exceptions or 500-level codes?

## Step 2: Query Logs in Log Analytics
Navigate to the Log Analytics Workspace connected to the environment and run these core queries to locate the root cause.

### Query A: Retrieve Top Server Exceptions
Identify what exceptions are crashing threads or causing errors:
```kusto
AppExceptions
| summarize Count=count() by ProblemId, ExceptionType
| order by Count desc
| take 10
```

### Query B: Trace Slow HTTP Requests
Determine which URLs are taking the longest to resolve:
```kusto
AppRequests
| where Success == false or DurationMs > 2000
| project TimeGenerated, Name, Url, DurationMs, ResultCode
| order by DurationMs desc
| take 20
```

### Query C: App Service System Console Logs
Check for system-level errors or boot failures from the Linux host:
```kusto
AppServiceConsoleLogs
| where Message contains "Error" or Message contains "Exception"
| project TimeGenerated, Message
| order by TimeGenerated desc
| take 50
```

## Step 3: Identify Potential Causes

### Scenario A: Spiking Logic Exceptions in the Rules Engine
If logs point to exceptions within `AccessingChildcareEntitlementChecker.RulesEngine.Services`, a bug has slipped past the CI gates (e.g. unexpected date format input or null references in household facts).
- Resolution: Isolate the inputs causing the crash and execute Runbook: Deploy an emergency fix.

### Scenario B: Resource Starvation (CPU/Memory exhaustion)
If the App Service Plan is pinned at >90% CPU or Memory utilization:
- Resolution: Go to the App Service Plan in the Azure Portal and scale up the instance size or scale out the instance count (e.g., from 2 instances to 4 instances) to mitigate load temporarily.

### Scenario C: Front Door Handshake Failures
If App Insights shows no active traffic but users receive 502/503 errors at the edge, there may be an IP restriction mismatch or SSL failure.
- Resolution: Verify Front Door origin configs and confirm the App Service's IP restrictions are properly accepting `AzureFrontDoor.Backend`.
