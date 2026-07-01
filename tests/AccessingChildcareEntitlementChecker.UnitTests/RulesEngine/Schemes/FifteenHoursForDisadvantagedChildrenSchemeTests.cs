using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Schemes;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.UnitTests.RulesEngine.Schemes;

public class FifteenHoursForDisadvantagedChildrenSchemeTests
{
    private static readonly DateOnly Today = new(2025, 1, 1);

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
                Today),

            AgeInMonths = AgeCalculations.CalculateAgeInMonths(
                dateOfBirth,
                Today)
        };
    }

    [Fact]
    public void Evaluate_WhenChildIsBornAndEligibleNow_ReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = CreateBornChild(new DateOnly(2023, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.FifteenHoursForDisadvantagedChildren, result.SchemeCode);
        Assert.True(result!.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenChildIsBornAndEligibleInFuture_ReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = CreateBornChild(new DateOnly(2024, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.FifteenHoursForDisadvantagedChildren, result.SchemeCode);
        Assert.False(result!.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenChildIsDueAndEligibleInFuture_ReturnsSchemeResult()
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
        Assert.False(result!.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenChildIsBornAndExceedsAgeThreshold_ReturnsNull()
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
    public void Evaluate_WhenChildIsNotEligibleDueToLocation_ReturnsNull()
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
    public void Evaluate_OneParentWorkingOtherParentReceivingQualifyingBenefit_ReturnsSchemeResult()
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
    public void Evaluate_WhenChildRecievesEHCPAutomaticallyEligible_ReturnsSchemeResult()
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
    public void Evaluate_WhenChildRecievesDLAAutomaticallyEligible_ReturnsSchemeResult()
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

    [Fact]
    public void Evaluate_WhenParentsHaveFosterChildAutomaticallyEligible_ReturnsSchemeResult()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = true
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
            RelationshipToChild = RelationshipToChild.FosterParent
        };

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.FifteenHoursForDisadvantagedChildren, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }
}