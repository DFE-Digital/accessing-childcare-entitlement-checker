---
title: Application Architecture
eleventyNavigation:
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

## Project Structure

The solution is divided into two primary functional projects:

1.  Web: Follows standard ASP.NET Core MVC patterns. Manages the stateful user journey across multiple pages.
2.  RulesEngine: A pure logic library containing no web-specific dependencies.
