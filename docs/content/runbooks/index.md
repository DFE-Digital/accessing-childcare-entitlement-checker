---
title: Runbooks
layout: collection
sectionKey: Runbooks
pagination:
  data: data
  size: 5
data:
  - url: "/runbooks/001-rollback-deployment"
    data:
      title: Roll back a deployment
  - url: "/runbooks/002-deploy-emergency-fix"
    data:
      title: Deploy an emergency fix
  - url: "/runbooks/003-remove-secrets-git-history"
    data:
      title: Remove secrets from Git history
  - url: "/runbooks/004--rotate-secrets"
    data:
      title: Rotate secrets
  - url: "/runbooks/005-ddos-attack"
    data:
      title: Respond to DDoS attack
  - url: "/runbooks/006-regional-failover"
    data:
      title: Regional failover
  - url: "/runbooks/007-investigate-service-degradation"
    data:
      title: Investigate service degradation
includeInBreadcrumbs: true
---
Runbooks provide step-by-step guidance for responding to, mitigating, and recovering from specific operational events and security incidents.

These runbooks are built to:

- Minimise Mean Time to Recovery (MTTR) by providing clear, pre-tested instructions to engineers on-call.
- Reduce Cognitive Load under operational pressure or high-stress security incidents.
- Ensure Consistency and Safety across environments by standardising commands, credentials handling, and escalation steps.

When executing these runbooks, prioritise the following principles:

1. Safety First: Prioritise preserving service availability and user data. Never attempt manual, ad-hoc changes in production that bypass our pipeline or IAC definition unless documented as an emergency fallback step.
2. Document Actions: Keep an active log of commands run, timestamps of actions, and observed behaviours in your incident response channel (e.g., Slack or Teams).
3. Validate Early: After executing any recovery step, run our validation checklists (such as hitting the `/health` endpoint and performing end-to-end tests) to ensure the service is fully restored before declaring resolution.
