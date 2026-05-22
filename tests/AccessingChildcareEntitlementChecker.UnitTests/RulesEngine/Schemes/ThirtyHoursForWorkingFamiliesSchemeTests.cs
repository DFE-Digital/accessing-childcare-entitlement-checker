using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Schemes;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.UnitTests.RulesEngine.Schemes;

public class ThirtyHoursForWorkingFamiliesSchemeTests
{
    private static readonly DateOnly Today = new(2025, 1, 1);
    private static ThirtyHoursForWorkingFamiliesEvaluator CreateEvaluator()
    {
        return new ThirtyHoursForWorkingFamiliesEvaluator();
    }

    private static DerivedContext CreateEligibleContext()
    {
        return new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true,
                CountryOfResidence = CountryOfResidence.England
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
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
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(
            SchemeCode.ThirtyHoursForWorkingFamilies,
            result!.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenChildIsBornAndEligibleInFuture_ReturnsFutureEligibility()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2024, 10, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(
            SchemeCode.ThirtyHoursForWorkingFamilies,
            result!.SchemeCode);
        Assert.False(result.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenChildIsNotBornAndEligibleInFuture_ReturnsFutureEligibility()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = false,
        };

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(
            SchemeCode.ThirtyHoursForWorkingFamilies,
            result!.SchemeCode);
        Assert.False(result.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenChildIsNotEligibleDueToAge_ReturnsNull()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2019, 8, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenChildIsNotEligibleDueToLocation_ReturnsNull()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true,
                CountryOfResidence = CountryOfResidence.Wales
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenParentDoesNotMeetMinimumIncomeThreshold_ReturnsNull()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true,
                CountryOfResidence = CountryOfResidence.England
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenParentExceedsMaximumIncomeThreshold_ReturnsNull()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true,
                CountryOfResidence = CountryOfResidence.England
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = true,
                EarnsAboveThreshold = true
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenParentIsInSelfEmploymentGracePeriod_ReturnsSchemeResult()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true,
                CountryOfResidence = CountryOfResidence.England
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = true
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(
            SchemeCode.ThirtyHoursForWorkingFamilies,
            result!.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_OneParentWorkingOtherParentReceivingQualifyingBenefit_ReturnsSchemeResult()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = true,
                HasAccessToPublicFunds = true,
                CountryOfResidence = CountryOfResidence.England
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
            },
            Partner = new PersonFacts
            {
                IsInPaidWork = false,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                Benefits =
                [
                    PersonBenefit.IncapacityBenefit
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 9, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(
            SchemeCode.ThirtyHoursForWorkingFamilies,
            result!.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenChildEligibleInFuture_SetsApplyFromDate()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2024, 10, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Equal(
            child.DateOfBirth!.Value.AddDays(23 * 7),
            result!.ApplyFromDate);
    }

    [Fact]
    public void Evaluate_WhenChildEligibleInFuture_SetsUseFromDate()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2024, 10, 1));

        var nineMonthsOldDate =
            child.DateOfBirth!.Value.AddMonths(9);

        var expectedUseFromDate =
            TermDateCalculator.GetNextTermStartDate(
                nineMonthsOldDate);

        var result = scheme.Evaluate(context, child);

        Assert.Equal(
            expectedUseFromDate,
            result!.UseFromDate);
    }
}