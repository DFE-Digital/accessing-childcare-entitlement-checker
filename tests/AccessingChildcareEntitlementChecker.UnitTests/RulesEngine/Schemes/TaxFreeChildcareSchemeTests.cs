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
        Assert.Equal(SchemeCode.TaxFreeChildcare, result.SchemeCode);
        Assert.True(result!.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenBothParentsWorking_ReturnsSchemeResult()
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
        Assert.True(result!.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_WhenNeitherParentWorking_ReturnsNull()
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
    public void Evaluate_WhenBothParentsWorkingAndHouseholdReceivesUniversalCredit_ReturnsNull()
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
        Assert.Equal(SchemeCode.TaxFreeChildcare, result!.SchemeCode);
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
            ChildRelatedBenefits =
            [
                ChildRelatedBenefit.DisabilityLivingAllowance
            ],
            AgeInYears = 16
        };

        var result = evaluator.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.TaxFreeChildcare, result.SchemeCode);
        Assert.True(result!.EligibleNow);
        Assert.False(result.EligibleInFuture);
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
        Assert.True(result!.EligibleNow);
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
    public void Evaluate_HouseholdHasNoAccessToPublicFunds_ReturnsSchemeResult()
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
    public void Evaluate_WhenBothParentsWorkingAndPartnerReceivesChildcareVouchers_ReturnsNull()
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
    public void Evaluate_SingleParentBelowIncome_LeaveChildIsIneligibleAndOtherChildIsTemporarilyEligible()
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
    public void Evaluate_UserLeaveChildAndPartnerOnLeaveForAnotherChild_ReturnsDifferentApplyAndEndParties()
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
    public void Evaluate_BothParentsOnLeaveForDifferentChildrenAndBelowIncome_OnlyOtherChildIsEligible()
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
    public void Evaluate_TemporaryLeaveExemptionIsAvailableButBenefitRouteQualifies_ReturnsNoSpecialEndParty()
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