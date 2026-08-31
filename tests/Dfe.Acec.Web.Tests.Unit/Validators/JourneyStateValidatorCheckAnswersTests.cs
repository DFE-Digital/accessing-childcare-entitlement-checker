using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Validators;
using FluentValidation;

namespace Dfe.Acec.Web.Tests.Unit.Validators;

public class JourneyStateValidatorCheckAnswersTests
{
    private readonly JourneyStateValidator _validator = new();

    private FluentValidation.Results.ValidationResult Validate(JourneyState journeyState) => _validator.Validate(
            journeyState,
            options => options.IncludeRuleSets(
                JourneyStateValidator.CheckAnswersRuleSet));

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

    private static JourneyState CreateValidUserOnlyJourneyState() => new()
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

    private static JourneyState CreateValidUserAndPartnerOnlyJourneyState() => new()
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

    [Fact]
    public void CheckAnswersWhenUserOnlyJourneyIsCompleteIsValid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();

        var result = Validate(journeyState);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CheckAnswersWhenCountryOfResidenceIsMissingIsInvalid()
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
    public void CheckAnswersWhenUserAgeIsMissingIsInvalid()
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
    public void CheckAnswersWhenNationalityIsMissingIsInvalid()
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
    public void CheckAnswersWhenPaidWorkIsMissingIsInvalid()
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
    public void CheckAnswersWhenUniversalCreditIsMissingIsInvalid()
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
    public void CheckAnswersWhenBenefitsAreMissingIsInvalid()
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
    public void CheckAnswersWhenChildcareSupportIsMissingIsInvalid()
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
    public void CheckAnswersWhenHasPartnerIsMissingIsInvalid()
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
    public void CheckAnswersWhenNationalityRequiresSettledStatusAndSettledStatusIsMissingIsInvalid()
    {
        var journeyState = CreateValidUserOnlyJourneyState();
        journeyState.Nationality = NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland;

        var result = Validate(journeyState);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(JourneyState.SettledStatus));
    }

    [Fact]
    public void CheckAnswersWhenPaidWorkRequiresWorkStatusAndWorkStatusIsMissingIsInvalid()
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
    public void CheckAnswersWhenSelfEmployedAndSelfEmployedDurationIsMissingIsInvalid()
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
    public void CheckAnswersWhenSelfEmployedDurationRequiresYearlyEarningsAndYearlyEarningsIsMissingIsInvalid()
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
    public void CheckAnswersWhenPaidEmploymentAndWeeklyEarningsIsMissingIsInvalid()
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
    public void CheckAnswersWhenApprenticeAndWeeklyEarningsIsMissingIsInvalid()
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
    public void CheckAnswersWhenWeeklyEarningsAreAboveThresholdAndYearlyEarningsIsMissingIsInvalid()
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
    public void CheckAnswersWhenChildcareSupportIncludesVouchersAndVoucherReceiptIsMissingIsInvalid()
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
    public void CheckAnswersWhenUserAndPartnerJourneyIsCompleteIsValid()
    {
        var journeyState = CreateValidUserAndPartnerOnlyJourneyState();

        var result = Validate(journeyState);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CheckAnswersWhenPartnerAgeIsMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerPaidWorkIsMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerBenefitsAreMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerChildcareSupportIsMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerPaidWorkRequiresWorkStatusAndWorkStatusIsMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerSelfEmployedAndSelfEmployedDurationIsMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerSelfEmployedDurationRequiresYearlyEarningsAndYearlyEarningsIsMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerPaidEmploymentAndWeeklyEarningsIsMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerIsApprenticeAndWeeklyEarningsIsMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerWeeklyEarningsAreAboveThresholdAndYearlyEarningsIsMissingIsInvalid()
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
    public void CheckAnswersWhenPartnerChildcareSupportIncludesVouchersAndVoucherReceiptIsMissingIsInvalid()
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
