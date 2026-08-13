---
title: Adding a childcare scheme
layout: sub-navigation
sectionKey: Tutorials
order: 2
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Tutorials
  key: Adding a childcare scheme
---
Let's learn how to add a brand new business rule to the system! In this tutorial, we'll guide you step-by-step through adding a new scheme to our Rules Engine. 

By the end of this guide, you'll have created a new scheme, registered it, and written a test to prove it works.

## 1. Declare the scheme code

First things first, we need to give our new scheme a name. The system identifies all our different childcare schemes using a shared list (an `enum`).

Open up `src/AccessingChildcareEntitlementChecker.RulesEngine/Types/SchemeCode.cs` and add your new scheme to the bottom of the list:

```csharp
public enum SchemeCode
{
    // ... existing schemes
    NewEntitlementScheme
}
```

## 2. Create the evaluator strategy

Next, let's write the actual business logic! The Rules Engine uses the "Strategy Pattern", which means every scheme gets its own dedicated, isolated class.

Create a new file in the `src/AccessingChildcareEntitlementChecker.RulesEngine/Schemes` folder named `NewEntitlementSchemeEvaluator.cs`. We will implement the `ISchemeEvaluator` interface here.

```csharp
using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Schemes;

public class NewEntitlementSchemeEvaluator : ISchemeEvaluator
{
    private const int MinimumEligibleAge = 2;

    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        // 1. Check if they are eligible right now
        var eligibleNow = 
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            child.IsBorn &&
            child.AgeInYears >= MinimumEligibleAge;

        // 2. Check if they will be eligible in the future (e.g., an unborn child)
        var eligibleInFuture = 
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            !child.IsBorn;

        // 3. If they aren't eligible now or in the future, just return null!
        if (!eligibleNow && !eligibleInFuture)
        {
            return null;
        }

        // 4. Otherwise, return the successful result
        return new SchemeResultDto
        {
            SchemeCode = SchemeCode.NewEntitlementScheme,
            EligibleNow = eligibleNow,
            EligibleInFuture = eligibleInFuture
        };
    }
}
```

## 3. Register your new scheme

Now that we have written our scheme, we need to tell the application about it so it can run it! We do this by registering it in the Dependency Injection (DI) container.

Open `src/AccessingChildcareEntitlementChecker.RulesEngine/Extensions/ServiceCollectionExtensions.cs` and add your new evaluator to the list:

```csharp
public static IServiceCollection AddRulesEngine(this IServiceCollection services)
{
    services.AddScoped<EntitlementRulesEngine>();

    // Register Scheme Evaluators
    services.AddScoped<ISchemeEvaluator, UniversalCreditChildcareEvaluator>();
    services.AddScoped<ISchemeEvaluator, FifteenHoursUniversalEvaluator>();
    // ... other evaluators
    
    // Add your brand new scheme here!
    services.AddScoped<ISchemeEvaluator, NewEntitlementSchemeEvaluator>(); 

    return services;
}
```

## 4. Verify with a unit test

Finally, let's prove our new scheme works exactly as expected!

Create a new test file at `tests/AccessingChildcareEntitlementChecker.UnitTests/RulesEngine/Schemes/NewEntitlementSchemeTests.cs` and write a quick test to verify your logic.

*Friendly tip: To see how the system orchestrates these rules conceptually under the hood, check out the [Rules engine explanation](/explanation/rules-engine/).*
