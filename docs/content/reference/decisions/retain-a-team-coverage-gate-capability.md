---
title: Retain a team coverage gate capability
layout: sub-navigation
order: 3
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions

---
## Context and problem statement

The team integrates SonarQube into the repository to check code quality.

However, SonarCloud centrally configures test and code coverage thresholds. This configuration offers no local control over coverage parameters.

Note: This decision does not establish a specific standard. It ensures that the team retains the capability to set custom standards.

## Decision drivers

* The team may require coverage standards that exceed the central standards of SonarCloud.
* The team must gate pull requests (PRs) using team-defined coverage standards.

## Considered options

* Adhering solely to the organisation-wide standard.
* Negotiating a revised organisation-wide standard with other teams.

## Decision outcome

The team selected the option to retain the capability to define a team coverage standard. The standard must meet or exceed the organisation-wide minimum.

## Pros and cons of the options

### Adhering solely to the organisation-wide standard

* **Positive:** Consistency across projects.
* **Negative:** Limits the ability to apply stricter quality gates on greenfield code.

### Negotiating a revised organisation-wide standard with other teams

* **Positive:** Maintains consistency across all projects.
* **Negative:** Stricter standards may not suit other projects.
* **Negative:** Negotiation processes potentially take much time.
