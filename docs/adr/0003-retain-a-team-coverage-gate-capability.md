# Retain a team coverage gate capability

## Context and Problem Statement

We've added SonarQube to the repo to check code quality.

However test/code coverage percentages are set centrally via SonarCloud. We don't have control over these.

Note: This ADR does not intend to fix a particular standard; only make sure we retain the capability to set our own.

## Decision Drivers

* We might want to exceed the standard set centrally (if any) via SonarCloud
* We want to be able to gate our PRs with our own standard.

## Considered Options

* Adhere only to org-wide standard
* Discuss changing the org-wide standard with the rest of the org

## Decision Outcome

We chose to retain our ability to set a team coverage standard; provided it exceeds the org wide standard.

## Pros and Cons of the Options

### Adhere only to org-wide standard

* Good, because it's consistent
* Bad, because as a greenfield project we may want to exceed the standard

### Discuss changing the org-wide standard with the rest of the org

* Good, because it would be consistent
* Bad, because our standard might not apply or be desirable for every team
* Bad, because it would likely be time-consuming