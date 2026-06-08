using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Schemes;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;

namespace AccessingChildcareEntitlementChecker.UnitTests.RulesEngine.Schemes;

public class UniversalCreditChildcareSchemeTests
{
    private static readonly DateOnly Today = new(2025, 1, 1);

    private static UniversalCreditChildcareEvaluator CreateEvaluator()
    {
        return new UniversalCreditChildcareEvaluator();
    }
    private static DerivedContext CreateEligibleContext()
    {
        return new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                ReceivesUniversalCredit = true,
                HasAccessToPublicFunds = true,
                LivesInGreatBritain = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true
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

        var child = CreateBornChild(new DateOnly(2021, 11, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(
            SchemeCode.UniversalCreditChildcare,
            result!.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenChildIsOver16_ReturnsNull()
    {
        var scheme = CreateEvaluator();

        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2007, 10, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenHouseholdDoesNotReceiveUniversalCredit_ReturnsNull()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                ReceivesUniversalCredit = false,
                HasAccessToPublicFunds = true,
                LivesInGreatBritain = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                Benefits = [
                    PersonBenefit.ContributionBasedEmploymentAndSupportAllowance
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenSingleParentNotInPaidWork_ReturnsNull()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                ReceivesUniversalCredit = true,
                HasAccessToPublicFunds = true,
                LivesInGreatBritain = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = false
            }
        };

        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenCoupleHasOneWorkingParentAndOneExemptParent_ReturnsSchemeResult()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                ReceivesUniversalCredit = true,
                HasAccessToPublicFunds = true,
                LivesInGreatBritain = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
            },

            Partner = new PersonFacts
            {
                IsInPaidWork = false,
                Benefits = [
                    PersonBenefit.IncapacityBenefit
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(
            SchemeCode.UniversalCreditChildcare,
            result!.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenCoupleHasOneWorkingParentAndOneNonExemptParent_ReturnsNull()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = true,
                ReceivesUniversalCredit = true,
                HasAccessToPublicFunds = true,
                LivesInGreatBritain = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
            },

            Partner = new PersonFacts
            {
                IsInPaidWork = false,
                Benefits = [
                    PersonBenefit.ContributionBasedEmploymentAndSupportAllowance
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenChildIsUnbornAndHouseholdMeetsRequirements_ReturnsFutureEligibility()
    {
        var scheme = CreateEvaluator();

        var context = CreateEligibleContext();

        var child = new ChildFacts
        {
            Name = "Baby",
            IsBorn = false,
            DueDate = new DateOnly(2025, 12, 1)
        };

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.False(result!.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }
}