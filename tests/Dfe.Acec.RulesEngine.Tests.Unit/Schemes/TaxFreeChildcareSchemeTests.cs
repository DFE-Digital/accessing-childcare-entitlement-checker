using Dfe.Acec.RulesEngine.Derived;
using Dfe.Acec.RulesEngine.Helpers;
using Dfe.Acec.RulesEngine.Schemes;
using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Tests.Unit.Schemes;

public class TaxFreeChildcareSchemeTests
{
    private static readonly DateOnly _today = new(2025, 1, 1);

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
                PaidWorkStatus = PaidWorkStatus.Yes,
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
                _today),

            AgeInMonths = AgeCalculations.CalculateAgeInMonths(
                dateOfBirth,
                _today)
        };
    }

    [Fact]
    public void EvaluateWhenChildIsEligibleNowReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.TaxFreeChildcare, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenBothParentsWorkingReturnsSchemeResult()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                EarnsAboveThreshold = true,
                ExceedsAdjustedNetIncomeLimit = false
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.Yes,
                EarnsAboveThreshold = true,
                ExceedsAdjustedNetIncomeLimit = false
            }
        };

        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenNeitherParentWorkingReturnsNull()
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
                PaidWorkStatus = PaidWorkStatus.No,
                Benefits =
                [
                    PersonBenefit.IncapacityBenefit
                ]
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.No,
                Benefits =
                [
                    PersonBenefit.IncapacityBenefit
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2020, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenBothParentsWorkingAndHouseholdReceivesUniversalCreditReturnsNull()
    {
        var scheme = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = true,
                HasAccessToPublicFunds = true,
                ReceivesUniversalCredit = true
            },

            User = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.Yes,
                EarnsAboveThreshold = true,
                ExceedsAdjustedNetIncomeLimit = false
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.Yes,
                EarnsAboveThreshold = true,
                ExceedsAdjustedNetIncomeLimit = false,
            }
        };

        var child = CreateBornChild(new DateOnly(2020, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenChildIsNotBornAndEligibleInFutureReturnsFutureEligibility()
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
        Assert.Equal(SchemeCode.TaxFreeChildcare, result.SchemeCode);
        Assert.False(result.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateDisabledChildEligibleNowUnderExtendedAgeRangeReturnsSchemeResult()
    {
        var evaluator = CreateEvaluator();
        var context = CreateEligibleContext();
        var child = new ChildFacts
        {
            Name = "Jack",
            IsBorn = true,
            ChildRelatedBenefits =
            [
                ChildRelatedBenefit.DisabilityLivingAllowance
            ],
            AgeInYears = 16
        };

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.TaxFreeChildcare, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateChildNotEligibleDueToAgeReturnsNull()
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
    public void EvaluateParentNotMeetingMinimumIncomeThresholdReturnsNull()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateParentExceedingAdjustedNetIncomeLimitThresholdReturnsNull()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = true,
                EarnsAboveThreshold = true
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenParentIsInSelfEmploymentGracePeriodReturnsSchemeResult()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = true
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.TaxFreeChildcare, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateOneParentWorkingOtherParentReceivingQualifyingBenefitReturnsSchemeResult()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true,
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.No,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                Benefits =
                [
                    PersonBenefit.IncapacityBenefit
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.TaxFreeChildcare, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateHouseholdHasNoAccessToPublicFundsReturnsSchemeResult()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
            }
        };
        var child = CreateBornChild(new DateOnly(2022, 1, 1));

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
        Assert.Null(result.ApplyAndStartAffectedByParentalLeave);
        Assert.Null(result.EligibilityEndsWithParentalLeaveFor);
    }

    [Fact]
    public void EvaluateWhenBothParentsWorkingAndPartnerReceivesChildcareVouchersReturnsNull()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                EarnsAboveThreshold = true,
                ExceedsAdjustedNetIncomeLimit = false
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.Yes,
                EarnsAboveThreshold = true,
                ExceedsAdjustedNetIncomeLimit = false,
                ChildcareSupport =
                [
                    ChildcareSupport.ChildcareVouchers
                ]
            }
        };

        var child = CreateBornChild(new DateOnly(2020, 1, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }


    [Fact]
    public void EvaluateHouseholdReceivesChildcareBursaryReturnsNull()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
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

    [Fact]
    public void EvaluateSingleParentBelowIncomeLeaveChildIsIneligibleAndOtherChildIsTemporarilyEligible()
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
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false,
                ExceedsAdjustedNetIncomeLimit = false
            }
        };

        var leaveChild = CreateBornChild(new DateOnly(2022, 1, 1));
        leaveChild.UserIsOnParentalLeaveForChild = true;

        var otherChild = CreateBornChild(new DateOnly(2020, 1, 1));
        otherChild.UserIsOnParentalLeaveForChild = false;

        var leaveChildResult = evaluator.Evaluate(context, leaveChild);

        var otherChildResult = evaluator.Evaluate(context, otherChild);

        Assert.Null(leaveChildResult);
        Assert.NotNull(otherChildResult);
        Assert.True(otherChildResult.EligibleNow);
        Assert.False(otherChildResult.EligibleInFuture);
        Assert.Null(otherChildResult.ApplyAndStartAffectedByParentalLeave);
        Assert.Equal(ParentalLeaveParty.User, otherChildResult.EligibilityEndsWithParentalLeaveFor);
    }

    [Fact]
    public void EvaluateUserLeaveChildAndPartnerOnLeaveForAnotherChildReturnsDifferentApplyAndEndParties()
    {
        var evaluator = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = true,
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                EarnsAboveThreshold = true,
                ExceedsAdjustedNetIncomeLimit = false
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false,
                ExceedsAdjustedNetIncomeLimit = false
            }
        };

        var userLeaveChild = CreateBornChild(new DateOnly(2022, 1, 1));

        userLeaveChild.UserIsOnParentalLeaveForChild = true;
        userLeaveChild.PartnerIsOnParentalLeaveForChild = false;

        var partnerLeaveChild = CreateBornChild(new DateOnly(2020, 1, 1));

        partnerLeaveChild.UserIsOnParentalLeaveForChild = false;
        partnerLeaveChild.PartnerIsOnParentalLeaveForChild = true;

        var userLeaveChildResult = evaluator.Evaluate(context, userLeaveChild);

        var partnerLeaveChildResult = evaluator.Evaluate(context, partnerLeaveChild);

        Assert.NotNull(userLeaveChildResult);
        Assert.True(userLeaveChildResult.EligibleNow);
        Assert.Equal(ParentalLeaveParty.User, userLeaveChildResult.ApplyAndStartAffectedByParentalLeave);
        Assert.Equal(ParentalLeaveParty.Partner, userLeaveChildResult.EligibilityEndsWithParentalLeaveFor);
        Assert.Null(partnerLeaveChildResult);
    }

    [Fact]
    public void EvaluateBothParentsOnLeaveForDifferentChildrenAndBelowIncomeOnlyOtherChildIsEligible()
    {
        var evaluator = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = true,
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false,
                ExceedsAdjustedNetIncomeLimit = false
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false,
                ExceedsAdjustedNetIncomeLimit = false
            }
        };

        var userLeaveChild = CreateBornChild(new DateOnly(2022, 1, 1));

        userLeaveChild.UserIsOnParentalLeaveForChild = true;
        userLeaveChild.PartnerIsOnParentalLeaveForChild = false;

        var partnerLeaveChild = CreateBornChild(new DateOnly(2020, 1, 1));

        partnerLeaveChild.UserIsOnParentalLeaveForChild = false;
        partnerLeaveChild.PartnerIsOnParentalLeaveForChild = true;

        var otherChild = CreateBornChild(new DateOnly(2018, 1, 1));

        otherChild.UserIsOnParentalLeaveForChild = false;
        otherChild.PartnerIsOnParentalLeaveForChild = false;

        var userLeaveChildResult = evaluator.Evaluate(context, userLeaveChild);

        var partnerLeaveChildResult = evaluator.Evaluate(context, partnerLeaveChild);

        var otherChildResult = evaluator.Evaluate(context, otherChild);

        Assert.Null(userLeaveChildResult);
        Assert.Null(partnerLeaveChildResult);
        Assert.NotNull(otherChildResult);
        Assert.True(otherChildResult.EligibleNow);
        Assert.False(otherChildResult.EligibleInFuture);
        Assert.Null(otherChildResult.ApplyAndStartAffectedByParentalLeave);
        Assert.Equal(ParentalLeaveParty.UserAndPartner, otherChildResult.EligibilityEndsWithParentalLeaveFor);
    }

    [Fact]
    public void EvaluateTemporaryLeaveExemptionIsAvailableButBenefitRouteQualifiesReturnsNoSpecialEndParty()
    {
        var evaluator = CreateEvaluator();

        var context = new DerivedContext
        {
            Household = new HouseholdFacts
            {
                HasPartner = true,
                HasAccessToPublicFunds = true
            },

            User = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false,
                ExceedsAdjustedNetIncomeLimit = false,
                Benefits =
                [
                    PersonBenefit.CarersAllowance
                ]
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.Yes,
                EarnsAboveThreshold = true,
                ExceedsAdjustedNetIncomeLimit = false
            }
        };

        var otherChild = CreateBornChild(new DateOnly(2022, 1, 1));

        otherChild.UserIsOnParentalLeaveForChild = false;
        otherChild.PartnerIsOnParentalLeaveForChild = false;

        var result = evaluator.Evaluate(context, otherChild);

        Assert.NotNull(result);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
        Assert.Null(result.ApplyAndStartAffectedByParentalLeave);
        Assert.Null(result.EligibilityEndsWithParentalLeaveFor);
    }

}
