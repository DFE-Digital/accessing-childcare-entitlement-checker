using Dfe.Acec.RulesEngine.Derived;
using Dfe.Acec.RulesEngine.Helpers;
using Dfe.Acec.RulesEngine.Schemes;
using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Tests.Unit.Schemes;

public class FifteenHoursForDisadvantagedChildrenSchemeTests
{
    private static readonly DateOnly _today = new(2025, 1, 1);

    private static FifteenHoursForDisadvantagedChildrenEvaluator CreateEvaluator()
    {
        return new FifteenHoursForDisadvantagedChildrenEvaluator();
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
                Benefits =
                [
                    PersonBenefit.GuaranteedElementOfPensionCredit
                ]
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
                _today),

            AgeInMonths = AgeCalculations.CalculateAgeInMonths(
                dateOfBirth,
                _today)
        };
    }

    [Fact]
    public void EvaluateWhenChildIsBornAndEligibleNowReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = CreateBornChild(new DateOnly(2023, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.FifteenHoursForDisadvantagedChildren, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenChildIsBornAndEligibleInFutureReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = CreateBornChild(new DateOnly(2024, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.FifteenHoursForDisadvantagedChildren, result.SchemeCode);
        Assert.False(result.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenChildIsDueAndEligibleInFutureReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = false,
            DueDate = new DateOnly(2025, 10, 1)
        };

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.FifteenHoursForDisadvantagedChildren, result.SchemeCode);
        Assert.False(result.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenChildIsBornAndExceedsAgeThresholdReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            DueDate = new DateOnly(2020, 1, 1)
        };

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenChildIsNotEligibleDueToLocationReturnsNull()
    {
        var evaluator = CreateEvaluator();

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
                Benefits =
                [
                    PersonBenefit.GuaranteedElementOfPensionCredit
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateOneParentWorkingOtherParentReceivingQualifyingBenefitReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
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
                PaidWorkStatus = PaidWorkStatus.Yes
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.No,
                Benefits = [
                    PersonBenefit.GuaranteedElementOfPensionCredit
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.FifteenHoursForDisadvantagedChildren, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenChildRecievesEhcpAutomaticallyEligibleReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
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
                PaidWorkStatus = PaidWorkStatus.No
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.No
            }
        };

        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            DateOfBirth = new DateOnly(2023, 1, 1),
            AgeInYears = 2,
            ChildRelatedBenefits = [
                ChildRelatedBenefit.EducationHealthAndCarePlan
            ]
        };

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.FifteenHoursForDisadvantagedChildren, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenChildRecievesDlaAutomaticallyEligibleReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
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
                PaidWorkStatus = PaidWorkStatus.No
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.No
            }
        };

        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            DateOfBirth = new DateOnly(2023, 1, 1),
            AgeInYears = 2,
            ChildRelatedBenefits = [
                ChildRelatedBenefit.DisabilityLivingAllowance
            ]
        };

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.FifteenHoursForDisadvantagedChildren, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }
}
