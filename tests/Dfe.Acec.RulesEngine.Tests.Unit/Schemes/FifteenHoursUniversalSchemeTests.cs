using Dfe.Acec.RulesEngine.Derived;
using Dfe.Acec.RulesEngine.Helpers;
using Dfe.Acec.RulesEngine.Schemes;
using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Tests.Unit.Schemes;

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
    public void EvaluateWhenChildIsEligibleNowReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext();
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
        Assert.Equal(
            SchemeCode.FifteenHoursUniversal,
            result.SchemeCode);
    }

    [Fact]
    public void EvaluateWhenChildIsEligibleInFutureReturnsFutureEligibility()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext();
        var child = CreateBornChild(new DateOnly(2023, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.False(result.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenChildIsOverFourReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext();
        var child = CreateBornChild(new DateOnly(2020, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenLocationIsNotEnglandReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = CreateContext(CountryOfResidence.Wales);
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenChildIsEligibleInFutureSetsUseFromDateToNextTerm()
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