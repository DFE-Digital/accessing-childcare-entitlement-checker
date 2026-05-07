# Use app-level HTTP Basic auth to limit access to dev deployment

## Context and Problem Statement

We need to deploy the application to Azure in the dev environment, but want to limit access so that:

* End-users don't stumble across the dev deployment and mistake it for the real thing.
* The dev environment doesn't get indexed by search engines and steal rank from the production deployment.

## Decision Drivers

* Ease and speed of development.
* Ease of management.
* Ease of use for the wider team.

It's important to note that this isn't primarily a security solution - the security risk is low. It's a usability and hygiene issue.

## Considered Options

* Restricting access by IP
* Using app-level authentication with DfE Entra to restrict access to users with `...education.gov.uk` accounts
* Using app-level HTTP Basic authentication with a single password
* Using some other app-level authentication scheme

## Decision Outcome

Chosen option: "Using app-level HTTP Basic authentication with a single password".

* Simplest implementation.
* Fulfills requirement of blocking access primarily for usability/hygiene.
* Does not require coordination with other teams.
  * No need to get hold of DfE VPN egress IP CIDRs.
  * No need to a registration to DfE Entra/ADFS tenant.
* Simplest management - just need to distribute single password to team.

### Consequences

* Good, because easy to implement.
* Good, because easy for team to use.
* Good, because no need to coordinate with other teams.
* Good, because fulfills the requirement to limit access to dev site.
* Bad, because may require manual password rotation strategy
