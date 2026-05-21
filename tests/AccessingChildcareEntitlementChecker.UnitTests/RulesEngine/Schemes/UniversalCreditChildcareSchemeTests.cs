using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Schemes;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.UnitTests.RulesEngine.Schemes;

public class UniversalCreditChildcareSchemeTests
{
    [Fact]
    public void Evaluate_WhenChildIsEligibleNow_ReturnsSchemeResult()
    {
        var scheme = new UniversalCreditChildcareEvaluator();

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
                Benefits =
                [
                    PersonBenefit.UniversalCredit
                ]
            }
        };

        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            AgeInYears = 3
        };

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
        var scheme = new UniversalCreditChildcareEvaluator();

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
                Benefits =
                [
                    PersonBenefit.UniversalCredit
                ]
            }
        };

        var child = new ChildFacts
        {
            Name = "James",
            IsBorn = true,
            AgeInYears = 17
        };

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenHouseholdDoesNotReceiveUniversalCredit_ReturnsNull()
    {
        var scheme = new UniversalCreditChildcareEvaluator();

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
                    PersonBenefit.EmploymentAndSupportAllowance
                ]
            }
        };

        var child = new ChildFacts
        {
            Name = "Mia",
            IsBorn = true,
            AgeInYears = 3
        };

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenSingleParentNotInPaidWork_ReturnsNull()
    {
        var scheme = new UniversalCreditChildcareEvaluator();

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
                IsInPaidWork = false,
                Benefits =
                [
                    PersonBenefit.UniversalCredit
                ]
            }
        };

        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            AgeInYears = 3
        };

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenCoupleHasOneWorkingParentAndOneExemptParent_ReturnsSchemeResult()
    {
        var scheme = new UniversalCreditChildcareEvaluator();

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
                Benefits =
                [
                    PersonBenefit.UniversalCredit
                ]
            },

            Partner = new PersonFacts
            {
                IsInPaidWork = false,
                Benefits = [
                    PersonBenefit.IncapacityBenefit
                ]
            }
        };

        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            AgeInYears = 3
        };

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
        var scheme = new UniversalCreditChildcareEvaluator();

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
                Benefits =
                [
                    PersonBenefit.UniversalCredit
                ]
            },

            Partner = new PersonFacts
            {
                IsInPaidWork = false,
                Benefits = [
                    PersonBenefit.JobseekersAllowance
                ]
            }
        };

        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            AgeInYears = 3
        };

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenChildIsUnbornAndHouseholdMeetsRequirements_ReturnsFutureEligibility()
    {
        var scheme = new UniversalCreditChildcareEvaluator();

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
                Benefits =
                [
                    PersonBenefit.UniversalCredit
                ]
            }
        };

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
        Assert.Equal(
            new DateOnly(2025, 12, 1),
            result.UseFromDate);
    }
}