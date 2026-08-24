---
title: Disclosure of secrets
layout: sub-navigation
order: 8
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
Developers might accidentally commit sensitive credentials, passwords, or deployment keys to public source repositories or expose them via application logs.

## Impact

- Unauthorised third parties gain administrative access to the Azure Subscription.
- Attackers bypass pre-production Basic Authentication gates.

## Prevention

- Passwordless Azure access: Deployment workflows utilise Azure OIDC. The system stores only short-lived federated credentials. Workflows save no subscription client secrets inside GitHub Secrets.
- Environment variable configuration: The team injects secrets, such as pre-production Basic Auth passwords, as App Service environment variables. They do not save these secrets in the repository's `appsettings.json`.
- Pre-commit scans: Utilisation of local scanners (such as GitGuardian or git-secrets) to detect credentials prior to commit.

## Detection

- GitHub secret scanning: Automated scans execute on every push and pull request.
- External reports: Alerts from security researchers or automated scans.

## Response

- Immediate response: Invalidate the leaked credential immediately. Delete the OAuth/Federated credential trust.
- History purge: Execute a Git rewriting command to purge the credential from repository histories.

## Recovery

To recover, generate a fresh replacement credential. Inject it into the appropriate environment variables. Finally, verify access logs to ensure no unauthorised requests occurred during exposure.

## Related runbooks

- [Rotate secrets](/how-to/runbooks/rotate-secrets/)
- [Remove secrets from Git history](/how-to/runbooks/remove-secrets-git-history/)
