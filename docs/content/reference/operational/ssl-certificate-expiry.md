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
- Automatic renewal: Azure Front Door renews managed certificates 45 days before expiry for hostnames with a CNAME record to the Front Door endpoint.
- Apex domain: The production `service.gov.uk` hostname is a zone apex with an alias record, not a CNAME record. Front Door cannot renew this certificate automatically. When the domain shows *Pending revalidation*, follow the response steps below. See [apex domains in Azure Front Door](https://learn.microsoft.com/en-us/azure/frontdoor/apex-domain).

## Detection

- Automated edge SSL monitoring alerts.
- Sudden drops in traffic coupled with 5xx/handshake failure logs.

## Response & recovery

If a managed certificate fails to auto-renew (typically due to DNS validation record removal):
1. Verification of CNAME/TXT validation records on the custom domain DNS to ensure they match required values.
2. Triggering of manual validation via the Azure Portal or Terraform to force certificate renewal.

For the production `service.gov.uk` apex domain:
1. In the Azure Portal, open the production Front Door profile and go to **Domains**.
2. Select **Pending revalidation** for the `service.gov.uk` domain, then select **Regenerate**.
3. Run the production deployment. Terraform reads the new token and updates the `_dnsauth` TXT record.

## Related runbooks

- [Investigate service degradation](/how-to/runbooks/investigate-service-degradation/)
