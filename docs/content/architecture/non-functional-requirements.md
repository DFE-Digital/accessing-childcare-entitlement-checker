---
title: Non-Functional Requirements
layout: sub-navigation
sectionKey: Architecture
eleventyNavigation:
  parent: Architecture
  key: Non-Functional Requirements
order: 4
---

This document defines the operational characteristics and quality standards for the Accessing Childcare Entitlement Checker.

| ID | Category | Requirement | Target |
|:---:|:---:|:---|:---|
| NFR001 | Performance | Median page load time (p50) | < 1 second |
| NFR002 | Performance | 95th percentile page load time (p95) | < 2 seconds |
| NFR003 | Throughput | Concurrent users | Up to 500 simultaneous users (initial phase) |
| NFR004 | Availability | Composite SLA | 99.9% |
| NFR005 | Security | OWASP Top 10 | Zero high/critical vulnerabilities identified by ZAP |
| NFR006 | Accessibility | WCAG Compliance | Level AA (WCAG 2.2) |
| NFR007 | Maintainability | Unit Test Coverage | > 80% line coverage |
