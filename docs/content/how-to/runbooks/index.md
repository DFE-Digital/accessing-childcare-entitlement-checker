---
title: Runbooks
layout: sub-navigation
sectionKey: How-to guides
order: 1
includeInBreadcrumbs: true
eleventyNavigation:
  parent: How-to guides
  key: Runbooks
---
Use these runbooks to respond to, mitigate, and recover from specific operational events and security incidents.

Execute these procedures to:

- Minimise Mean Time to Recovery (MTTR) using clear, pre-tested instructions.
- Reduce cognitive load under operational pressure or high-stress security incidents.
- Ensure consistency and safety across environments by running standardised commands, credential-handling methods, and escalation steps.

## Prioritise key response principles

1. **Put Safety First**: Prioritise preserving service availability and user data. Never perform manual, ad-hoc changes in production that bypass our pipeline or IAC definition unless documented as an emergency fallback step.
2. **Log and Document Actions**: Keep an active log of executed commands, action timestamps, and observed behaviors in your incident response channel (e.g., Slack or Teams).
3. **Validate Early and Often**: Run validation checklists (such as checking the `/health` endpoint and executing end-to-end tests) after any recovery step to verify full service restoration before declaring resolution.
