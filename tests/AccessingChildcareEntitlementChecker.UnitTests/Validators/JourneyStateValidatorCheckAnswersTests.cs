using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Validators;
using FluentValidation;

namespace AccessingChildcareEntitlementChecker.UnitTests.Validators;

public class JourneyStateValidatorCheckAnswersTests
{
    private readonly JourneyStateValidator validator = new();

    private FluentValidation.Results.ValidationResult Validate(JourneyState journeyState)
    {
        return validator.Validate(
            journeyState,
            options => options.IncludeRuleSets(
                JourneyStateValidator.CheckAnswersRuleSet));
    }

    private static Child CreateBornChild()
    {
        var child = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = new DateOnly(2020, 1, 1),
            ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
        };

        return child;
    }

    private static JourneyState CreateValidUserOnlyJourneyState()
    {
        return new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            Children =
            {
                [CreateBornChild().ChildId] = CreateBornChild()
            },
            UserAge = AgeRange.TwentyOneOrOver,
            Nationality = NationalityOption.BritishOrIrishCitizen,
            PaidWork = PaidWorkOption.No,
            UniversalCredit = UniversalCreditOption.DoesNotReceive,
            Benefits = [BenefitsOption.None],
            ChildcareSupport = [ChildcareSupportOption.None],
            HasPartner = false
        };
    }

    private static JourneyState CreateValidUserAndPartnerOnlyJourneyState()
    {
        return new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            Children =
            {
                [CreateBornChild().ChildId] = CreateBornChild()
            },
            UserAge = AgeRange.TwentyOneOrOver,
            Nationality = NationalityOption.BritishOrIrishCitizen,
            PaidWork = PaidWorkOption.No,
            UniversalCredit = UniversalCreditOption.DoesNotReceive,
            Benefits = [BenefitsOption.None],
            ChildcareSupport = [ChildcareSupportOption.None],
            HasPartner = true,

            PartnerAge = AgeRange.TwentyOneOrOver,
            PartnerNationality = NationalityOption.BritishOrIrishCitizen,
            PartnerPaidWork = PartnerPaidWorkOption.No,
            PartnerBenefits = [PartnerBenefitsOption.None],
            PartnerChildcareSupport = [PartnerChildcareSupportOption.None],

        };
    }

    [Fact]
    public void CheckAnswers_WhenUserOnlyJourneyIsComplete_IsValid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();

        var result = Validate(journeyState);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CheckAnswers_WhenCountryOfResidenceIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.CountryOfResidence = null;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.CountryOfResidence));
    }

    [Fact]
    public void CheckAnswers_WhenUserAgeIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.UserAge = null;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.UserAge));
    }

    [Fact]
    public void CheckAnswers_WhenNationalityIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.Nationality = null;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.Nationality));
    }

    [Fact]
    public void CheckAnswers_WhenPaidWorkIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.PaidWork = null;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PaidWork));
    }

    [Fact]
    public void CheckAnswers_WhenUniversalCreditIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.UniversalCredit = null;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.UniversalCredit));
    }

    [Fact]
    public void CheckAnswers_WhenBenefitsAreMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.Benefits = [];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.Benefits));
    }

    [Fact]
    public void CheckAnswers_WhenChildcareSupportIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.ChildcareSupport = [];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.ChildcareSupport));
    }

    [Fact]
    public void CheckAnswers_WhenHasPartnerIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.HasPartner = null;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.HasPartner));
    }

    [Fact]
    public void CheckAnswers_WhenNationalityRequiresSettledStatusAndSettledStatusIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.SettledStatus));
    }

    [Fact]
    public void CheckAnswers_WhenPaidWorkRequiresWorkStatusAndWorkStatusIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.PaidWork = PaidWorkOption.Yes;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.WorkStatus));
    }

    [Fact]
    public void CheckAnswers_WhenSelfEmployedAndSelfEmployedDurationIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.PaidWork = PaidWorkOption.Yes;
        journeyState.WorkStatus = [WorkStatusOption.SelfEmployed];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.SelfEmployedDuration));
    }

    [Fact]
    public void CheckAnswers_WhenSelfEmployedDurationRequiresYearlyEarningsAndYearlyEarningsIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.PaidWork = PaidWorkOption.Yes;
        journeyState.WorkStatus = [WorkStatusOption.SelfEmployed];
        journeyState.SelfEmployedDuration = SelfEmployedDurationOption.NotLessThan12Months;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.YearlyEarnings));
    }

    [Fact]
    public void CheckAnswers_WhenPaidEmploymentAndWeeklyEarningsIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.PaidWork = PaidWorkOption.Yes;
        journeyState.WorkStatus = [WorkStatusOption.PaidEmployment];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.WeeklyEarnings));
    }

    [Fact]
    public void CheckAnswers_WhenApprenticeAndWeeklyEarningsIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.PaidWork = PaidWorkOption.Yes;
        journeyState.WorkStatus = [WorkStatusOption.Apprentice];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.WeeklyEarnings));
    }

    [Fact]
    public void CheckAnswers_WhenWeeklyEarningsAreAboveThresholdAndYearlyEarningsIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.PaidWork = PaidWorkOption.Yes;
        journeyState.WorkStatus = [WorkStatusOption.PaidEmployment];
        journeyState.WeeklyEarnings = WeeklyEarningsOption.AboveThreshold;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.YearlyEarnings));
    }

    [Fact]
    public void CheckAnswers_WhenChildcareSupportIncludesVouchersAndVoucherReceiptIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.ChildcareSupport = [ChildcareSupportOption.ChildcareVouchers];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.ChildcareVoucherReceipt));
    }

    [Fact]
    public void CheckAnswers_WhenUserAndPartnerJourneyIsComplete_IsValid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();

        var result = Validate(journeyState);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CheckAnswers_WhenPartnerAgeIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerAge = null;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerAge));
    }


    [Fact]
    public void CheckAnswers_WhenPartnerPaidWorkIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerPaidWork = null;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerPaidWork));
    }

    [Fact]
    public void CheckAnswers_WhenPartnerBenefitsAreMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerBenefits = [];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerBenefits));
    }

    [Fact]
    public void CheckAnswers_WhenPartnerChildcareSupportIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerChildcareSupport = [];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerChildcareSupport));
    }

    [Fact]
    public void CheckAnswers_WhenPartnerPaidWorkRequiresWorkStatusAndWorkStatusIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerWorkStatus));
    }

    [Fact]
    public void CheckAnswers_WhenPartnerSelfEmployedAndSelfEmployedDurationIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        journeyState.PartnerWorkStatus = [WorkStatusOption.SelfEmployed];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerSelfEmployedDuration));
    }

    [Fact]
    public void CheckAnswers_WhenPartnerSelfEmployedDurationRequiresYearlyEarningsAndYearlyEarningsIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        journeyState.PartnerWorkStatus = [WorkStatusOption.SelfEmployed];
        journeyState.PartnerSelfEmployedDuration = SelfEmployedDurationOption.NotLessThan12Months;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerYearlyEarnings));
    }

    [Fact]
    public void CheckAnswers_WhenPartnerPaidEmploymentAndWeeklyEarningsIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        journeyState.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerWeeklyEarnings));
    }

    [Fact]
    public void CheckAnswers_WhenPartnerIsApprenticeAndWeeklyEarningsIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        journeyState.PartnerWorkStatus = [WorkStatusOption.Apprentice];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerWeeklyEarnings));
    }

    [Fact]
    public void CheckAnswers_WhenPartnerWeeklyEarningsAreAboveThresholdAndYearlyEarningsIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        journeyState.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];
        journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.AboveThreshold;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerYearlyEarnings));
    }

    [Fact]
    public void CheckAnswers_WhenPartnerChildcareSupportIncludesVouchersAndVoucherReceiptIsMissing_IsInvalid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();
        journeyState.PartnerChildcareSupport = [PartnerChildcareSupportOption.ChildcareVouchers];

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.PartnerChildcareVoucherReceipt));
    }


}