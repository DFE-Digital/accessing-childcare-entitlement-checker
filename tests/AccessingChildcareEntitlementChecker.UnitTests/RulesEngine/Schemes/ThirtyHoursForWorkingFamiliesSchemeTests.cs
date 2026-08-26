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
                PaidWorkStatus = PaidWorkStatus.Yes,
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
            AgeInYears = AgeCalculations.CalculateAgeInYears(dateOfBirth, Today),

            AgeInMonths = AgeCalculations.CalculateAgeInMonths(dateOfBirth, Today)
        };
    }


    [Fact]
    public void EvaluateWhenChildIsEligibleNowReturnsSchemeResult()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenChildIsBornAndEligibleInFutureReturnsFutureEligibility()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2024, 10, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result.SchemeCode);
        Assert.False(result.EligibleNow);
        Assert.True(result.EligibleInFuture);
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
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result.SchemeCode);
        Assert.False(result.EligibleNow);
        Assert.True(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenChildIsNotEligibleDueToAgeReturnsNull()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2019, 8, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenChildIsNotEligibleDueToLocationReturnsNull()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenParentDoesNotMeetMinimumIncomeThresholdReturnsNull()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenParentExceedsMaximumIncomeThresholdReturnsNull()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = true,
                EarnsAboveThreshold = true
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Null(result);
    }

    [Fact]
    public void EvaluateWhenParentIsInSelfEmploymentGracePeriodReturnsSchemeResult()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = true
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result.SchemeCode);
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
                HasAccessToPublicFunds = true,
                CountryOfResidence = CountryOfResidence.England
            },

            User = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
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

        var child = CreateBornChild(new DateOnly(2023, 9, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateBothParentsWorkingReturnsSchemeResult()
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
            },
            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 9, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void EvaluateWhenChildEligibleInFutureSetsApplyFromDate()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2024, 10, 1));

        var result = scheme.Evaluate(context, child);

        Assert.Equal(child.DateOfBirth!.Value.AddDays(23 * 7), result!.ApplyFromDate);
    }

    [Fact]
    public void EvaluateWhenChildEligibleInFutureSetsUseFromDate()
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

        Assert.Equal(expectedUseFromDate, result!.UseFromDate);
    }

    [Fact]
    public void EvaluateSingleParentBelowIncomeLeaveChildIsIneligibleAndOtherChildIsTemporarilyEligible()
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
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false
            }
        };

        var leaveChild = CreateBornChild(new DateOnly(2023, 12, 1));

        leaveChild.UserIsOnParentalLeaveForChild = true;

        var otherChild = CreateBornChild(new DateOnly(2023, 9, 1));

        otherChild.UserIsOnParentalLeaveForChild = false;

        var leaveChildResult = scheme.Evaluate(context, leaveChild);

        var otherChildResult = scheme.Evaluate(context, otherChild);

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
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false
            }
        };

        var userLeaveChild = CreateBornChild(new DateOnly(2023, 12, 1));

        userLeaveChild.UserIsOnParentalLeaveForChild = true;
        userLeaveChild.PartnerIsOnParentalLeaveForChild = false;

        var partnerLeaveChild = CreateBornChild(new DateOnly(2023, 9, 1));

        partnerLeaveChild.UserIsOnParentalLeaveForChild = false;
        partnerLeaveChild.PartnerIsOnParentalLeaveForChild = true;

        var userLeaveChildResult = scheme.Evaluate(context, userLeaveChild);

        var partnerLeaveChildResult = scheme.Evaluate(context, partnerLeaveChild);

        Assert.NotNull(userLeaveChildResult);
        Assert.True(userLeaveChildResult.EligibleNow);
        Assert.Equal(ParentalLeaveParty.User, userLeaveChildResult.ApplyAndStartAffectedByParentalLeave);
        Assert.Equal(ParentalLeaveParty.Partner, userLeaveChildResult.EligibilityEndsWithParentalLeaveFor);
        Assert.Null(partnerLeaveChildResult);
    }

    [Fact]
    public void EvaluateBothParentsOnLeaveForDifferentChildrenAndBelowIncomeOnlyOtherChildIsEligible()
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
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false
            }
        };

        var userLeaveChild = CreateBornChild(new DateOnly(2023, 12, 1));

        userLeaveChild.UserIsOnParentalLeaveForChild = true;
        userLeaveChild.PartnerIsOnParentalLeaveForChild = false;

        var partnerLeaveChild = CreateBornChild(new DateOnly(2023, 9, 1));

        partnerLeaveChild.UserIsOnParentalLeaveForChild = false;
        partnerLeaveChild.PartnerIsOnParentalLeaveForChild = true;

        var otherChild = CreateBornChild(new DateOnly(2022, 12, 1));

        otherChild.UserIsOnParentalLeaveForChild = false;
        otherChild.PartnerIsOnParentalLeaveForChild = false;

        var userLeaveChildResult = scheme.Evaluate(context, userLeaveChild);

        var partnerLeaveChildResult = scheme.Evaluate(context, partnerLeaveChild);

        var otherChildResult = scheme.Evaluate(context, otherChild);

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
                PaidWorkStatus = PaidWorkStatus.ParentalLeave,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = false,
                Benefits =
                [
                    PersonBenefit.CarersAllowance
                ]
            },

            Partner = new PersonFacts
            {
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = true
            }
        };

        var otherChild = CreateBornChild(new DateOnly(2023, 12, 1));

        otherChild.UserIsOnParentalLeaveForChild = false;
        otherChild.PartnerIsOnParentalLeaveForChild = false;

        var result = scheme.Evaluate(context, otherChild);

        Assert.NotNull(result);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
        Assert.Null(result.ApplyAndStartAffectedByParentalLeave);
        Assert.Null(result.EligibilityEndsWithParentalLeaveFor);
    }
}