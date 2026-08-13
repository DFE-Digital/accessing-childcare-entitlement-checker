---
title: Application configuration
layout: sub-navigation
sectionKey: Reference
order: 7
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Reference
  key: Application configuration
---
This reference details the environment variables and configuration keys used by the Accessing Childcare Entitlement Checker application.

These keys are parsed during application initialisation to manage feature toggles, telemetry integration, and operational environments.

| Configuration key | Data type | Required? | Description |
| :--- | :--- | :--- | :--- |
| `APPLICATIONINSIGHTS_CONNECTION_STRING` | String | Optional | Connection string for Azure Application Insights monitoring |
| `RedisConnection` | String | Optional | Connection string for the distributed Redis cache; defaults to local in-memory cache if missing |
| `DevelopmentBasicAuthPassword` | String | Optional | Shared basic auth password for dev/test environment locking; omitted in Production |
| `ASPNETCORE_ENVIRONMENT` | String | Required | ASP.NET Core hosting environment, e.g. Development, Production |
| `GoogleTagManager__ContainerId` | String | Optional | Container ID for Google Tag Manager tracking |
| `FeatureManagement__HmrcIntegration` | Boolean | Optional | Toggles HMRC rules integration |
