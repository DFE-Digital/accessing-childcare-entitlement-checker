---
title: Unauthorised access
layout: page
showPagination: true
order: 7
sectionKey: Operational
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
An attacker or compromised identity gains privileged administrative access to our hosting environments or source code repository.

## Impact

- Infrastructure configuration tampering.
- Insertion of malicious code or backdoors into the rules engine.
- Alteration of public UI routing or configurations.

## Prevention

- Non-Production Basic Authentication: All non-production environments (Dev, Test, Staging) are locked behind HTTP Basic Authentication and managed via GitHub Actions secrets to prevent public or unauthorised access to pre-release features.
- Identity Security (Entra ID): Administrative access to the Azure Portal is locked down via Entra ID utilising Multi-Factor Authentication (MFA), Privileged Identity Management (PIM) for just-in-time access, and Single Sign-On (SSO).
- Passwordless Deployment (OIDC): GitHub Actions utilises OpenID Connect (OIDC) federated credentials rather than long-lived secrets to log into Azure, preventing credential theft from our repositories.
- Least Privilege Access: Cloud roles are scoped narrowly using role-based access control (RBAC).

## Detection

- Audit Logs: Log Analytics Workspace captures Microsoft Entra ID login logs, App Service HTTP logs, and Terraform deployment actions.
- GitHub Alerting: Warnings when new OAuth apps or SSH keys are authorised.

## Response

- Revoke the compromised user's active tokens and session states.
- Revert unauthorised infrastructure configurations by running `terraform apply` to overwrite drifted changes.
- Conduct a security audit on code commits and deployment history.

## Related runbooks

- [Rotate secrets](/runbooks/004-rotate-secrets/)
- [Remove secrets from Git history](/runbooks/003-remove-secrets-git-history/)
