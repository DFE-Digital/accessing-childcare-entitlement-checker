using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Schemes;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;

namespace AccessingChildcareEntitlementChecker.UnitTests.RulesEngine.Schemes;

public class FifteenHoursUniversalSchemeTests
{
    private static readonly DateOnly Today = new(2025, 1, 1);

    private static FifteenHoursUniversalEvaluator CreateEvaluator()
    {
        return new FifteenHoursUniversalEvaluator();
    }

    private static DerivedContext CreateContext(
        CountryOfResidence country = CountryOfResidence.England)
    {
        return new DerivedContext
        {
            Household = new HouseholdFacts
            {
                CountryOfResidence = country
            }
        };
    }
    private static ChildFacts CreateBornChild(
        DateOnly dateOfBirth)
    {
        return new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            DateOfBirth = dateOfBirth,
            AgeInYears = AgeCalculations.CalculateAgeInYears(
                dateOfBirth,
                Today),

            AgeInMonths = AgeCalculations.CalculateAgeInMonths(
                dateOfBirth,
                Today)
        };
    }

    [Fact]
    public void Evaluate_WhenChildIsEligibleNow_ReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext();
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.True(result!.EligibleNow);
        Assert.False(result.EligibleInFuture);
        Assert.Equal(
            SchemeCode.FifteenHoursUniversal,
            result.SchemeCode);
    }

    [Fact]
    public void Evaluate_WhenChildIsEligibleInFuture_ReturnsFutureEligibility()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext();
        var child = CreateBornChild(new DateOnly(2023, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.False(result!.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenChildIsOverFour_ReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext();
        var child = CreateBornChild(new DateOnly(2020, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenLocationIsNotEngland_ReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext(CountryOfResidence.Wales);
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenChildIsEligibleInFuture_SetsApplyFromDate()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext();
        var child = CreateBornChild(new DateOnly(2023, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(
            child.DateOfBirth?.AddYears(3),
            result!.ApplyFromDate);
    }

    [Fact]
    public void Evaluate_WhenChildIsEligibleInFuture_SetsUseFromDateToNextTerm()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext();
        var child = CreateBornChild(new DateOnly(2023, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Equal(
            new DateOnly(2026, 4, 1),
            result!.UseFromDate);
    }
}