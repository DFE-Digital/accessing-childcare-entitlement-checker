---
title: Disclosure of secrets
layout: page
showPagination: true
order: 8
sectionKey: Operational
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
Sensitive credentials, passwords, or deployment keys are accidentally committed to public source repositories or exposed via application logs.

## Impact

- Unauthorized third parties gain administrative access to our Azure Subscription.
- Attackers bypass pre-production Basic Authentication gates.

## Prevention

- Passwordless Azure Access: Deployment workflows use Azure OIDC. We only store short-lived federated credentials. No subscription client secrets are saved inside GitHub Secrets!
- Environment Variable Configuration: Secrets like the pre-production Basic Auth password are injected as App Service environment variables rather than being saved in the repository's `appsettings.json`.
- Pre-commit scans: Encouraging developer use of local scanners (such as GitGuardian or git-secrets) to detect credentials before committing.

## Detection

- GitHub Secret Scanning: Automatically running on every push and pull request.
- External reports: Alerts from security researchers or automated scans.

## Response

- Immediately invalidate the leaked credential or delete the OAuth/Federated credential trust.
- Run a Git rewriting command to purge the credential from repository histories.

## Recovery

Create a fresh replacement credential, inject it into the appropriate environment variables, and re-verify access logs to ensure no malicious requests were served during the exposure window.

## Related runbooks

- [Rotate secrets](/runbooks/004-rotate-secrets/)
- [Remove secrets from Git history](/runbooks/003-remove-secrets-git-history/)
