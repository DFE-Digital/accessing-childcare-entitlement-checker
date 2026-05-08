# Use separate RulesEngine models and mapping layer rather than shared contract assembly

## Context and Problem Statement

The childcare entitlement checker introduces a separate `RulesEngine` project responsible for evaluating entitlement eligibility independently from the MVC web application.

A decision was required around ownership of shared enums and DTO types such as:

* `CountryOfResidence`
* `AgeRange`
* `WorkStatus`
* `Nationality`

Initially a separate shared assembly was considered so both the Web project and RulesEngine could consume the same contract types.

The Web application has evolved toward using these enums within presentation-layer concerns

* `DisplayAttribute`
* localisation resource keys
* enum-driven rendering helpers in Razor views

Example:

```csharp
public enum BirthStatus
{
    [Display(Name = "Option_Born")]
    Born,

    [Display(Name = "Option_Due")]
    Due,
}
```
Attempting to reference Web types directly from RulesEngine also introduced a circular dependency:

Web → RulesEngine

RulesEngine → Web


## Decision Drivers

* Keep entitlement logic isolated from MVC/presentation concerns.
* Avoid circular project references.
* Maintain a single dependency direction between Web and RulesEngine.
* Avoid unnecessary coupling between MVC/presentation concerns and entitlement logic.

## Considered Options

* Create a shared assembly containing shared DTOs/enums/contracts.
* Reference Web enums/types directly from RulesEngine.
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

Good, because avoids circular project references.

Good, because the mapping layer provides an explicit boundary between MVC journey models and entitlement domain models.

Bad, because some enums/types will exist in both projects.

Bad, because updates to the UI or rules may require changes in multiple places and projects.

Bad, because mapping code is required between Web models and RulesEngine DTOs.