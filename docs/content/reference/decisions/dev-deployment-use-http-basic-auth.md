---
title: Use app-level http basic auth to limit access to dev deployment
layout: sub-navigation
order: 5
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions

---
## Context and problem statement

Deployment of the application to the Azure development environment requires access restrictions to ensure:

* Public users cannot access the development deployment and mistake it for the production service.
* Search engines do not index the development environment, avoiding impact on production search engine rankings.

Note: This is a usability and hygiene measure rather than a robust security solution, as the security risk is low.

## Decision drivers

* Ease and speed of development.
* Low management overhead.
* Ease of access for development and stakeholder teams.

## Considered options

* Restricting access by IP.
* Using application-level authentication with DfE Entra to restrict access to users with `@education.gov.uk` accounts.
* Using application-level HTTP Basic authentication with a single shared credential.
* Using other application-level authentication schemes.

## Decision outcome

Chosen option: **Using application-level HTTP Basic authentication with a single shared credential**.

* Simplifies implementation.
* Satisfies the access restriction requirement.
* Eliminates coordination with external teams (such as obtaining DfE VPN egress IP CIDRs or registering with the DfE Entra/ADFS tenant).
* Low management overhead, requiring only the distribution of a single shared password.

### Consequences

* **Positive:** Low implementation effort.
* **Positive:** Simple user experience for development and stakeholder teams.
* **Positive:** No external dependencies or coordination required.
* **Positive:** Restricts access to the development environment as required.
* **Negative:** May require a manual credential rotation strategy.
