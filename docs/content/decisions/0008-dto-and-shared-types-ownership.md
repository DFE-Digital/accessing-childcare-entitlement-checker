---
title: Use separate RulesEngine models and mapping layer rather than shared contract assembly
layout: page
showPagination: true
order: 8
sectionKey: Decisions
eleventyNavigation:
  parent: Decisions
---

## Context and Problem Statement

The childcare entitlement checker introduces entitlement evaluation logic that could either live inside the MVC Web project or be separated into a dedicated `RulesEngine` project.

A separate `RulesEngine` project gives a clearer boundary around entitlement logic, but it raises a question about how data should cross that boundary.

The Web project already has journey/view models and enums used by forms. The RulesEngine also needs similar concepts to evaluate entitlement rules.

We therefore needed to decide whether to:

- share the same types across Web and RulesEngine to avoid mapping
- introduce a shared contract assembly
- duplicate the concepts and map between Web models and RulesEngine DTOs
- collapse the RulesEngine back into the Web project

The core question is whether sharing types across the boundary would couple the Web and RulesEngine closely enough that the separate project would no longer provide meaningful separation.

## Decision Drivers

* Keep entitlement logic isolated from MVC/presentation concerns.
* Reference Web enums/types directly from RulesEngine (rejected because it introduced unsupported circular project references between Web and RulesEngine).
* Maintain a single dependency direction between Web and RulesEngine.

## Considered Options

* Create a shared assembly containing shared DTOs/enums/contracts.
* Collapse RulesEngine into the Web project.
* Keep RulesEngine separate and duplicate/map shared concepts.

## Decision Outcome

Chosen option: "Keep RulesEngine separate and duplicate/map shared concepts".

The RulesEngine project will own:

- entitlement request/response DTOs
- rules-engine-specific enums/types
- derived context models
- scheme evaluators
- orchestration logic

The Web project will continue to own:

- MVC ViewModels
- presentation-layer enums/types
- validation attributes
- localisation metadata
- Razor rendering concerns

A mapping layer will map:

```text
ViewModel Enums / JourneyState → RulesEngine DTOs
```

No shared contract assembly will be introduced.

## Consequences

Good, because entitlement logic remains isolated from MVC/presentation concerns.

Good, because dependency direction remains.

Good, because the mapping layer provides an explicit boundary between MVC journey models and entitlement domain models.

Good because maintaining separate types allows the MVC application and RulesEngine to evolve independently during active development, with any model divergence resolved explicitly through the mapping layer.

Bad, because some enums/types will exist in both projects.

Bad, because updates to the UI or rules may require changes in multiple places and projects.

Bad, because mapping code is required between Web models and RulesEngine DTOs.