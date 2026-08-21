---
title: Use separate rulesengine models and mapping layer rather than shared contract assembly
layout: sub-navigation
order: 8
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions
---
## Context and problem statement

The childcare entitlement checker features entitlement evaluation logic that can either reside within the MVC Web project or be separated into a dedicated `RulesEngine` project.

A separate `RulesEngine` project provides a clear architectural boundary around entitlement logic, but it raises questions regarding how data crosses that boundary.

The Web project contains journey/view models and enums utilized by forms. The RulesEngine requires similar concepts to evaluate entitlement rules.

The architectural choices evaluated were:

* Share the same types across the Web and RulesEngine projects to avoid mapping.
* Introduce a shared contract assembly.
* Duplicate the concepts and map between Web models and RulesEngine DTOs.
* Collapse the RulesEngine back into the Web project.

The primary consideration is whether sharing types across the boundary couples the Web and RulesEngine assemblies too tightly, undermining the value of the separation.

## Decision drivers

* Keep entitlement logic isolated from MVC and presentation concerns.
* Maintain a single dependency direction from the Web project to the RulesEngine project (direct references from RulesEngine to Web enums/types were rejected to avoid circular dependencies).

## Considered options

* Create a shared assembly containing shared DTOs, enums, and contracts.
* Collapse the RulesEngine project into the Web project.
* Keep the RulesEngine project separate and map duplicated concepts.

## Decision outcome

Chosen option: **Keep RulesEngine separate and duplicate/map shared concepts**.

The RulesEngine project owns:

* Entitlement request and response DTOs
* Rules-engine-specific enums and types
* Derived context models
* Scheme evaluators
* Orchestration logic

The Web project owns:

* MVC ViewModels
* Presentation-layer enums and types
* Validation attributes
* Localisation metadata
* Razor rendering concerns

A mapping layer handles transition:

```text
ViewModel Enums / JourneyState → RulesEngine DTOs
```

No shared contract assembly is introduced.

## Consequences

* **Positive:** Entitlement logic remains isolated from MVC and presentation concerns.
* **Positive:** Unidirectional dependency structure is preserved.
* **Positive:** The mapping layer provides an explicit boundary between MVC journey models and entitlement domain models.
* **Positive:** Maintaining separate types allows the MVC application and RulesEngine to evolve independently during active development, with any model divergence resolved explicitly through the mapping layer.
* **Negative:** Some enums and types exist in both projects.
* **Negative:** Updates to the UI or rules may require changes in multiple projects.
* **Negative:** Mapping code is required to bridge Web models and RulesEngine DTOs.
