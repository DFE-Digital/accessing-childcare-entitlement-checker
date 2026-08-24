---
title: SSL certificate expiry
layout: sub-navigation
order: 6
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
The SSL/TLS certificates expire, preventing browsers from securely loading the web tool.

## Impact

Browsers display a security warning ("Your connection is not private"), preventing users from accessing the eligibility checker.

## Prevention

- Front Door Managed Certificates: Custom domains associated with Azure Front Door are configured with managed TLS.
- Zero-Touch Renewal: Azure Front Door automatically manages domain validation, renewal, and installation of SSL certificates (using authorities such as DigiCert or Let's Encrypt) 45 days before expiry. No manual key vault rotations or human procedures are required.

## Detection

- Automated edge SSL monitoring alerts.
- Sudden drops in traffic coupled with 5xx/handshake failure logs.

## Response & recovery

If a managed certificate fails to auto-renew (typically due to DNS validation record removal):
1. Verification of CNAME/TXT validation records on the custom domain DNS to ensure they match required values.
2. Triggering of manual validation via the Azure Portal or Terraform to force certificate renewal.

## Related runbooks

- [Investigate service degradation](/how-to/runbooks/investigate-service-degradation/)
