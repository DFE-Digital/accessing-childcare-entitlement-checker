using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Schemes;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.UnitTests.RulesEngine.Schemes;

public class TaxFreeChildcareSchemeTests
{
    private static readonly DateOnly Today = new(2025, 1, 1);

    private static TaxFreeChildcareEvaluator CreateEvaluator()
    {
        return new TaxFreeChildcareEvaluator();
    }
    private static DerivedContext CreateEligibleContext()
    {
        return new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
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
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.True(result!.EligibleNow);
        Assert.False(result.EligibleInFuture);
        Assert.Equal(
            SchemeCode.TaxFreeChildcare,
            result.SchemeCode);
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
            SchemeCode.TaxFreeChildcare,
            result!.SchemeCode);
        Assert.False(result.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_DisabledChildEligibleNowUnderExtendedAgeRange_ReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            ChildRelatedBenefits = [
                ChildRelatedBenefit.DisabilityLivingAllowance
            ],
            AgeInYears = 16
        };

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.True(result!.EligibleNow);
        Assert.False(result.EligibleInFuture);
        Assert.Equal(
            SchemeCode.TaxFreeChildcare,
            result.SchemeCode);
    }

    [Fact]
    public void Evaluate_ChildNotEligibleDueToAge_ReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            AgeInYears = 15
        };

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_ParentNotMeetingMinimumIncomeThreshold_ReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_ParentExceedingAdjustedNetIncomeLimitThreshold_ReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = true,
                EarnsAboveThreshold = true
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_WhenParentIsInSelfEmploymentGracePeriod_ReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = true
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.True(result!.EligibleNow);
        Assert.False(result.EligibleInFuture);
        Assert.Equal(
            SchemeCode.TaxFreeChildcare,
            result.SchemeCode);
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
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true,
            },

            Partner = new PersonFacts
            {
                IsInPaidWork = false,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                Benefits =
                [
                    PersonBenefit.JobseekersAllowance
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(
            SchemeCode.TaxFreeChildcare,
            result!.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_HouseholdHasNoAccessToPublicFunds_ReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = false
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_HouseholdReceivesChildcareVouchers_ReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true,
                ChildcareSupport =
                [
                    ChildcareSupport.ChildcareVouchers
                ]
            },

            Partner = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true,
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_HouseholdReceivesChildcareBursary_ReturnsNull()
    {
        var evaluator = CreateEvaluator();
        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = false,
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                IsInPaidWork = true,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true,
                ChildcareSupport =
                [
                    ChildcareSupport.ChildcareBursaryOrGrant
                ]
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

}