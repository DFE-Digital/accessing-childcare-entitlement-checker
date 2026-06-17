---
title: SSL certificate expiry
layout: page
showPagination: true
order: 6
sectionKey: Operational
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Operational
---
The SSL/TLS certificates expire, preventing users' browsers from securely loading the web tool.

## Impact

Browsers show a red warning page ("Your connection is not private"), preventing parents/carers from accessing the eligibility checker.

## Prevention

- Front Door Managed Certificates: Custom domains associated with Azure Front Door are configured with managed TLS.
- Zero-Touch Renewal: Azure Front Door automatically manages the domain validation, renewal, and installation of the SSL certificates (using authorities such as DigiCert or Let's Encrypt) 45 days before expiry. No manual key vault rotations or human procedures are required.

## Detection

- Automated edge SSL monitoring alerts.
- Sudden drops in traffic coupled with 5xx/handshake failure logs.

## Response & recovery

If a managed certificate fails to auto-renew (usually due to DNS validation record removal):
1. Verify the CNAME/TXT validation records on the custom domain DNS match the required values.
2. Trigger manual validation via the Azure Portal or Terraform to force the renewal.

## Related runbooks

- [Investigate service degradation](/runbooks/007-investigate-service-degradation/)
