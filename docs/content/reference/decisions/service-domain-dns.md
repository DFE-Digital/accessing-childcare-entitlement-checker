---
title: Host the service.gov.uk domain in an Azure DNS zone
layout: sub-navigation
order: 13
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions
---
## Context

The service moves from an `education.gov.uk` hostname to `check-if-you-are-eligible-for-childcare-funding.service.gov.uk`. The production Front Door profile already has a custom domain for this hostname. The domain needs a DNS zone, an apex record that points to Front Door, and a TXT record for domain validation.

Each environment uses a different Azure subscription. The zone is in the production subscription.

Other DfE services, for example [publish-teacher-training](https://github.com/DFE-Digital/publish-teacher-training), use the shared [DfE Terraform modules](https://github.com/DFE-Digital/terraform-modules) for their domains.

## Options considered

| Option | Benefits | Costs |
| :--- | :--- | :--- |
| Production only, zone from the DfE `dns/zones` module | Small change. All DNS resources are in one subscription. No new identities or permissions. | Non-production environments keep their current hostnames. |
| All environments write records in a shared zone | Non-production gets `service.gov.uk` hostnames | Cross-subscription permissions, custom roles and a zone lookup in each deployment |
| DfE `domains/environment_domains` module | Same pattern as other DfE services | Needs a separate domains Front Door profile in front of each environment. This changes all of our Front Door infrastructure. |

## Decision

Only production uses the `service.gov.uk` domain. Development, test and staging keep their current hostnames.

| Component | Owner |
| :--- | :--- |
| Domains resource group | `infra/terraform-domains/` |
| DNS zone, CAA, SPF and DMARC records | `infra/terraform-domains/`, through the DfE `dns/zones` module |
| Front Door custom domain | `infra/terraform/`, production only |
| Apex alias record (`@`) and validation record (`_dnsauth`) | `infra/terraform/`, production only |

The `workflow-domains.yml` workflow applies `infra/terraform-domains/` with the production identity. The domains state uses the `domains.tfstate` key in the production state storage account.

The production deployment reads the zone with a data source. It creates the A alias record and the TXT record with the same pattern as the DfE `domains/environment_domains` module. The `service_domain_resource_group_name` variable turns on these records. Only `production.tfvars` sets this variable.

### Certificate renewal

The production hostname is a zone apex. Front Door cannot renew an apex certificate automatically. When the domain shows *Pending revalidation*, an operator must regenerate the token and run the production deployment. See [SSL certificate expiry](/reference/operational/ssl-certificate-expiry/).

### Deployment order

1. Merge the change. The domains workflow creates the zone and shows its name servers in the job summary.
2. Ask GDS to delegate the domain to those name servers.
3. Run the production deployment. It creates the apex and validation records.

If the zone does not exist, the production plan fails at the zone data source. Do step 1 first.

## Consequences

* All DNS resources are in the production subscription. No cross-subscription access is necessary.
* Non-production environments do not get `service.gov.uk` hostnames. A later change can add them.
* The apex certificate needs manual revalidation.
* The education hostname and the `service.gov.uk` hostname serve the application at the same time. A later change removes the education hostname.
