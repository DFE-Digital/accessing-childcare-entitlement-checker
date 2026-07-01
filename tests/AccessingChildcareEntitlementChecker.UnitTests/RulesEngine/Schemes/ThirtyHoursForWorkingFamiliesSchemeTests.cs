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
    public void Evaluate_WhenChildIsEligibleNow_ReturnsSchemeResult()
    {
        var scheme = CreateEvaluator();
        var context = CreateEligibleContext();

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result!.SchemeCode);
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
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result!.SchemeCode);
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
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result!.SchemeCode);
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
                PaidWorkStatus = PaidWorkStatus.Yes,
                ExceedsAdjustedNetIncomeLimit = false,
                EarnsAboveThreshold = false,
                SelfEmployedLessThan12Months = true
            }
        };

        var child = CreateBornChild(new DateOnly(2023, 12, 1));

        var result = scheme.Evaluate(context, child);

        Assert.NotNull(result);
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result!.SchemeCode);
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
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result!.SchemeCode);
        Assert.True(result.EligibleNow);
        Assert.False(result.EligibleInFuture);
    }

    [Fact]
    public void Evaluate_BothParentsWorking_ReturnsSchemeResult()
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
        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, result!.SchemeCode);
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

        Assert.Equal(child.DateOfBirth!.Value.AddDays(23 * 7), result!.ApplyFromDate);
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

        Assert.Equal(expectedUseFromDate, result!.UseFromDate);
    }

    [Fact]
    public void Evaluate_SingleParentBelowIncome_LeaveChildIsIneligibleAndOtherChildIsTemporarilyEligible()
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
        Assert.True(otherChildResult!.EligibleNow);
        Assert.False(otherChildResult.EligibleInFuture);
        Assert.Null(otherChildResult.ApplyAndStartAffectedByParentalLeave);
        Assert.Equal(ParentalLeaveParty.User, otherChildResult.EligibilityEndsWithParentalLeaveFor);
    }

    [Fact]
    public void Evaluate_UserLeaveChildAndPartnerOnLeaveForAnotherChild_ReturnsDifferentApplyAndEndParties()
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
        Assert.True(userLeaveChildResult!.EligibleNow);
        Assert.Equal(ParentalLeaveParty.User, userLeaveChildResult.ApplyAndStartAffectedByParentalLeave);
        Assert.Equal(ParentalLeaveParty.Partner, userLeaveChildResult.EligibilityEndsWithParentalLeaveFor);
        Assert.Null(partnerLeaveChildResult);
    }

    [Fact]
    public void Evaluate_BothParentsOnLeaveForDifferentChildrenAndBelowIncome_OnlyOtherChildIsEligible()
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
        Assert.True(otherChildResult!.EligibleNow);
        Assert.False(otherChildResult.EligibleInFuture);
        Assert.Null(otherChildResult.ApplyAndStartAffectedByParentalLeave);
        Assert.Equal(ParentalLeaveParty.UserAndPartner, otherChildResult.EligibilityEndsWithParentalLeaveFor);
    }

    [Fact]
    public void Evaluate_TemporaryLeaveExemptionIsAvailableButBenefitRouteQualifies_ReturnsNoSpecialEndParty()
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
        Assert.True(result!.EligibleNow);
        Assert.False(result.EligibleInFuture);
        Assert.Null(result.ApplyAndStartAffectedByParentalLeave);
        Assert.Null(result.EligibilityEndsWithParentalLeaveFor);
    }
}