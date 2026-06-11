---
title: Application Architecture
layout: sub-navigation
sectionKey: Architecture
order: 1
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Architecture
  key: Application Architecture
---
## Level 1: System Context
The highest level of abstraction, showing how the system interacts with users.

```mermaid
C4Context
    title [System Context] Childcare Entitlement Checker

    Person(parent, "Parent/Carer", "A person checking their eligibility for childcare support.")
    System(acec, "Childcare Entitlement Checker", "Allows users to answer questions and receive a summary of childcare schemes they might be eligible for.")

    Rel(parent, acec, "Checks eligibility using", "HTTPS")
```

## Level 2: Container Diagram
Shows the high-level technology building blocks.

```mermaid
C4Container
    title [Container] Childcare Entitlement Checker

    Person(parent, "Parent/Carer", "User checking eligibility.")
    
    System_Boundary(c1, "Childcare Entitlement Checker") {
        Container(web_app, "Web Application", "ASP.NET Core 10.0 MVC", "Provides the multi-step form and displays results.")
        Container(rules_engine, "Rules Engine", "C# Class Library", "Calculates eligibility based on household facts.")
    }

    Rel(parent, web_app, "Uses", "HTTPS")
    Rel(web_app, rules_engine, "Invokes", "In-process")
```

## Level 3: Component Diagram (Rules Engine)
Shows the internal structure of the Rules Engine and how it processes eligibility.

```mermaid
C4Component
    title [Component] Rules Engine

    Container(web_app, "Web Application", "ASP.NET Core 10.0 MVC", "Provides user input data.")

    Container_Boundary(re_boundary, "Rules Engine") {
        Component(engine, "EntitlementRulesEngine", "Service", "Orchestrates the evaluation process.")
        Component(context_builder, "DerivedContextBuilder", "Helper", "Transforms raw DTOs into household facts.")
        Component(evaluators, "ISchemeEvaluator", "Interface", "Contract for individual scheme logic (e.g., Tax-Free Childcare).")
        Component(schemes, "Scheme Evaluators", "Implementations", "Concrete logic for each childcare scheme.")
    }

    Rel(web_app, engine, "Calls ResolveEntitlements", "In-process")
    Rel(engine, context_builder, "Uses to build fact context")
    Rel(engine, evaluators, "Iterates through all implementations")
    Rel(evaluators, schemes, "Implemented by")
```

## Project Structure

The solution is divided into two primary functional projects:

1.  **Web**: Follows standard ASP.NET Core MVC patterns. Manages the stateful user journey across multiple pages.
2.  **RulesEngine**: A pure logic library containing no web-specific dependencies. It uses a "Fact-based" approach where raw user input is mapped to a `DerivedContext` before being evaluated by a suite of independent `ISchemeEvaluator` implementations.

