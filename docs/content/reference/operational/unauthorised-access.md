---
title: Unauthorised access
layout: sub-navigation
order: 7
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
An attacker or compromised identity gains privileged administrative access to hosting environments or the source code repository.

## Impact

- Infrastructure configuration tampering.
- Insertion of malicious code or backdoors into the rules engine.
- Alteration of public UI routing or configurations.

## Prevention

- Non-Production Basic Authentication: Non-production environments (Dev, Test, Staging) are locked behind HTTP Basic Authentication and managed via GitHub Actions secrets to prevent public or unauthorised access to pre-release features.
- Identity Security (Entra ID): Administrative access to the Azure Portal is restricted via Entra ID utilising Multi-Factor Authentication (MFA), Privileged Identity Management (PIM) for just-in-time access, and Single Sign-On (SSO).
- Passwordless Deployment (OIDC): GitHub Actions utilises OpenID Connect (OIDC) federated credentials rather than long-lived secrets to log into Azure, preventing credential theft from repositories.
- Least Privilege Access: Cloud roles are scoped narrowly using role-based access control (RBAC).

## Detection

- Audit Logs: Log Analytics Workspace captures Microsoft Entra ID login logs, App Service HTTP logs, and Terraform deployment actions.
- GitHub Alerting: Warnings when new OAuth apps or SSH keys are authorised.

## Response

- Revocation of compromised active tokens and session states.
- Reversion of unauthorised infrastructure configurations by executing `terraform apply` to overwrite drifted changes.
- Execution of a security audit on code commits and deployment history.

## Related runbooks

- [Rotate secrets](/how-to/runbooks/rotate-secrets/)
- [Remove secrets from Git history](/how-to/runbooks/remove-secrets-git-history/)
