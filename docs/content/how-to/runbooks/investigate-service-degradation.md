---
title: Investigate service degradation
layout: sub-navigation
order: 7
sectionKey: How-to guides
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
  key: Investigate service degradation
---
Follow this runbook to investigate and diagnose the root cause when the service experiences high latency, excessive 5xx errors, or general degradation.

## Step 1: Inspect high-level metrics in Application Insights

1. Log into the Azure Portal and navigate to the target Application Insights instance.
2. Inspect the following charts over the degradation timeframe:
   * **Server Response Time**: Identify if latency is spiking across all endpoints or specific routes.
   * **Server Requests**: Verify if request volumes are significantly higher than normal (indicating potential load issues or scrapers).
   * **Failed Requests**: Check for a corresponding spike in exceptions or 500-level codes.

## Step 2: Query logs in Log Analytics

Navigate to the Log Analytics Workspace connected to the environment and run these core queries to pinpoint the issue:

### Query A: Retrieve top server exceptions
Identify which exceptions are crashing threads or causing errors:
```kusto
AppExceptions
| summarize Count=count() by ProblemId, ExceptionType
| order by Count desc
| take 10
```

### Query B: Trace slow HTTP requests
Determine which URLs are taking the longest to resolve:
```kusto
AppRequests
| where Success == false or DurationMs > 2000
| project TimeGenerated, Name, Url, DurationMs, ResultCode
| order by DurationMs desc
| take 20
```

### Query C: Read App Service system console logs
Check for system-level errors or boot failures from the Linux host:
```kusto
AppServiceConsoleLogs
| where Message contains "Error" or Message contains "Exception"
| project TimeGenerated, Message
| order by TimeGenerated desc
| take 50
```

## Step 3: Identify and mitigate the root cause

### Scenario A: Mitigate spiking logic exceptions in the rules engine
1. If logs point to exceptions within `Dfe.Acec.RulesEngine.Services`, isolate the specific bad input (e.g., unexpected date format or null references in household facts) causing the crash.
2. Implement a code fix and execute **Runbook: Deploy an emergency fix**.

### Scenario B: Mitigate resource starvation (CPU/Memory exhaustion)
1. Check if the App Service Plan is pinned at >90% CPU or Memory utilisation.
2. Navigate to the App Service Plan in the Azure Portal and scale up the instance size or scale out the instance count (e.g., from 2 instances to 4 instances) to temporarily mitigate load.

### Scenario C: Mitigate Front Door handshake failures
1. If App Insights shows no active traffic but users receive 502/503 errors at the edge, check for an IP restriction mismatch or SSL failure.
2. Verify Front Door origin configurations and confirm that the App Service's IP restrictions are properly accepting `AzureFrontDoor.Backend` traffic.
