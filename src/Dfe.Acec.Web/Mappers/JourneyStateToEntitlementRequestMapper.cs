using Dfe.Acec.RulesEngine.Dtos.Requests;
using Dfe.Acec.RulesEngine.Types;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using AgeRange = Dfe.Acec.RulesEngine.Types.AgeRange;
using BirthStatus = Dfe.Acec.RulesEngine.Types.BirthStatus;
using CountryOfResidence = Dfe.Acec.RulesEngine.Types.CountryOfResidence;

namespace Dfe.Acec.Web.Mappers;

public class JourneyStateToEntitlementRequestMapper
{

    public EntitlementRequest Map(JourneyState journeyState) => new()
    {
        Household = MapHousehold(journeyState),
        User = MapUser(journeyState),
        Partner = MapPartner(journeyState),
        Children = MapChildren(journeyState)
    };

    private static HouseholdDto MapHousehold(JourneyState journeyState) => new()
    {
        CountryOfResidence = MapCountryOfResidence(journeyState.CountryOfResidence),
        HasPartner = journeyState.HasPartner == true,
        ReceivesUniversalCredit = journeyState.UniversalCredit == UniversalCreditOption.Receives,
    };

    private static PersonDto MapUser(JourneyState journeyState) => new()
    {
        AgeRange = MapAgeRange(journeyState.UserAge),
        PaidWorkStatus = MapPaidWorkStatus(journeyState.PaidWork),
        WorkStatuses = [.. journeyState.WorkStatus.Select(MapWorkStatus)],
        SelfEmployedLessThan12Months = journeyState.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months,
        EarnsAboveThreshold = journeyState.WeeklyEarnings == WeeklyEarningsOption.AboveThreshold,
        ExceedsAdjustedNetIncomeLimit = journeyState.YearlyEarnings == YearlyEarningsOption.AboveThreshold,
        Benefits = [.. journeyState.Benefits.Select(MapPersonBenefit).OfType<PersonBenefit>()],
        ChildcareSupport = [.. journeyState.ChildcareSupport.Select(MapChildcareSupport).OfType<ChildcareSupport>()],
        Nationality = MapNationality(journeyState.Nationality),
        HasSettledOrPreSettledStatus = MapSettledStatus(journeyState.SettledStatus),
    };

    private static PersonDto? MapPartner(JourneyState journeyState)
    {
        if (journeyState.HasPartner != true)
        {
            return null;
        }

        return new PersonDto
        {
            AgeRange = MapAgeRange(journeyState.PartnerAge),
            PaidWorkStatus = MapPaidWorkStatus(journeyState.PartnerPaidWork),
            WorkStatuses = [.. journeyState.PartnerWorkStatus.Select(MapWorkStatus)],
            SelfEmployedLessThan12Months = journeyState.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months,
            EarnsAboveThreshold = journeyState.PartnerWeeklyEarnings == WeeklyEarningsOption.AboveThreshold,
            ExceedsAdjustedNetIncomeLimit = journeyState.PartnerYearlyEarnings == YearlyEarningsOption.AboveThreshold,
            Benefits = [.. journeyState.PartnerBenefits.Select(MapPersonBenefit).OfType<PersonBenefit>()],
            ChildcareSupport = [.. journeyState.PartnerChildcareSupport.Select(MapPartnerChildcareSupport).OfType<ChildcareSupport>()],
            Nationality = MapNationality(journeyState.PartnerNationality),
            HasSettledOrPreSettledStatus = MapSettledStatus(journeyState.PartnerSettledStatus),
        };
    }

    private static List<ChildDto> MapChildren(JourneyState journeyState)
    {
        var children = new List<ChildDto>();

        foreach (var child in journeyState.Children.Values)
        {
            children.Add(MapChild(child, journeyState));
        }

        return children;
    }

    private static ChildDto MapChild(Child child, JourneyState journeyState) => new()
    {
        ChildId = child.ChildId,
        Name = child.Name,
        BirthStatus = MapBirthStatus(child.BirthStatus),
        DateOfBirth = child.BirthDate,
        DueDate = child.DueDate,
        ChildRelatedBenefits = MapChildBenefits(child),
        UserIsOnParentalLeaveForChild = journeyState.ParentalLeaveChildrenIds.Contains(child.ChildId),
        PartnerIsOnParentalLeaveForChild = journeyState.PartnerParentalLeaveChildrenIds.Contains(child.ChildId),
    };

    private static CountryOfResidence?
        MapCountryOfResidence(Web.Models.CountryOfResidence? country) => country switch
        {
            Models.CountryOfResidence.England =>
                CountryOfResidence.England,

            Models.CountryOfResidence.Scotland =>
                CountryOfResidence.Scotland,

            Models.CountryOfResidence.Wales =>
                CountryOfResidence.Wales,

            Models.CountryOfResidence.NorthernIreland =>
                CountryOfResidence.NorthernIreland,

            null => null,

            _ => throw new ArgumentOutOfRangeException(
                nameof(country))
        };

    private static AgeRange? MapAgeRange(Web.Models.AgeRange? ageRange) => ageRange switch
    {
        Models.AgeRange.UnderEighteen =>
            AgeRange.UnderEighteen,

        Models.AgeRange.EighteenToTwenty =>
            AgeRange.EighteenToTwenty,

        Models.AgeRange.TwentyOneOrOver =>
            AgeRange.TwentyOneOrOver,

        null => null,

        _ => throw new ArgumentOutOfRangeException(
            nameof(ageRange))

    };


    private static PaidWorkStatus? MapPaidWorkStatus(PaidWorkOption? paidWorkOption) => paidWorkOption switch
    {
        PaidWorkOption.Yes => PaidWorkStatus.Yes,
        PaidWorkOption.No => PaidWorkStatus.No,
        PaidWorkOption.ParentalLeave => PaidWorkStatus.ParentalLeave,
        PaidWorkOption.SickLeave => PaidWorkStatus.SickLeave,
        null => null,
        _ => throw new ArgumentOutOfRangeException(nameof(paidWorkOption))

    };

    private static PaidWorkStatus? MapPaidWorkStatus(PartnerPaidWorkOption? partnerPaidWorkOption) => partnerPaidWorkOption switch
    {
        PartnerPaidWorkOption.Yes => PaidWorkStatus.Yes,
        PartnerPaidWorkOption.No => PaidWorkStatus.No,
        PartnerPaidWorkOption.ParentalLeave => PaidWorkStatus.ParentalLeave,
        PartnerPaidWorkOption.SickLeave => PaidWorkStatus.SickLeave,
        null => null,
        _ => throw new ArgumentOutOfRangeException(nameof(partnerPaidWorkOption))

    };

    private static WorkStatus MapWorkStatus(WorkStatusOption workStatus) => workStatus switch
    {
        WorkStatusOption.PaidEmployment =>
            WorkStatus.PaidEmployment,

        WorkStatusOption.SelfEmployed =>
            WorkStatus.SelfEmployed,

        WorkStatusOption.Apprentice =>
            WorkStatus.Apprentice,

        _ => throw new ArgumentOutOfRangeException(
            nameof(workStatus))
    };

    private static PersonBenefit? MapPersonBenefit(BenefitsOption benefit) => benefit switch
    {
        BenefitsOption.CarersAllowance =>
            PersonBenefit.CarersAllowance,

        BenefitsOption.ContributionBasedEmploymentAndSupportAllowance =>
            PersonBenefit.ContributionBasedEmploymentAndSupportAllowance,

        BenefitsOption.EmploymentAndSupportAllowance =>
            PersonBenefit.EmploymentAndSupportAllowance,

        BenefitsOption.GuaranteedElementOfPensionCredit =>
            PersonBenefit.GuaranteedElementOfPensionCredit,

        BenefitsOption.IncapacityBenefit =>
            PersonBenefit.IncapacityBenefit,

        BenefitsOption.LimitedCapabilityForWork =>
            PersonBenefit.LimitedCapabilityForWork,

        BenefitsOption.LimitedCapabilityForWorkRelatedActivity =>
            PersonBenefit.LimitedCapabilityForWorkRelatedActivity,

        BenefitsOption.SevereDisablementAllowance =>
            PersonBenefit.SevereDisablementAllowance,


        BenefitsOption.None =>
            null,

        _ => throw new ArgumentOutOfRangeException(
            nameof(benefit),
            benefit,
            null)
    };

    private static PersonBenefit? MapPersonBenefit(
        PartnerBenefitsOption benefit) => MapPersonBenefit(
            Enum.Parse<BenefitsOption>(
                benefit.ToString()));

    private static ChildcareSupport? MapChildcareSupport(ChildcareSupportOption childcareSupport) => childcareSupport switch
    {
        ChildcareSupportOption.ChildcareBursaryOrGrant =>
            ChildcareSupport.ChildcareBursaryOrGrant,

        ChildcareSupportOption.ChildcareVouchers =>
            ChildcareSupport.ChildcareVouchers,

        ChildcareSupportOption.None =>
            null,

        _ => throw new ArgumentOutOfRangeException(
            nameof(childcareSupport),
            childcareSupport,
            null)

    };

    private static ChildcareSupport? MapPartnerChildcareSupport(PartnerChildcareSupportOption partnerChildcareSupport) => partnerChildcareSupport switch
    {
        PartnerChildcareSupportOption.ChildcareBursaryOrGrant =>
            ChildcareSupport.ChildcareBursaryOrGrant,

        PartnerChildcareSupportOption.ChildcareVouchers =>
            ChildcareSupport.ChildcareVouchers,

        PartnerChildcareSupportOption.None =>
            null,

        _ => throw new ArgumentOutOfRangeException(
            nameof(partnerChildcareSupport),
            partnerChildcareSupport,
            null)
    };

    private static Nationality? MapNationality(NationalityOption? nationality) => nationality switch
    {
        NationalityOption.BritishOrIrishCitizen =>
            Nationality.BritishOrIrishCitizen,

        NationalityOption.CitizenOfADifferentCountry =>
            Nationality.Other,

        NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland =>
            Nationality.EuropeanUnionEuropeanEconomicAreaOrSwissCitizen,

        null => null,

        _ => throw new ArgumentOutOfRangeException(
            nameof(nationality),
            nationality,
            null)
    };

    private static bool? MapSettledStatus(
        SettledStatusOption? settledStatus) => settledStatus switch
        {
            SettledStatusOption.Yes or SettledStatusOption.StillWaiting => true,

            SettledStatusOption.No => false,

            null => null,

            _ => throw new ArgumentOutOfRangeException(
                nameof(settledStatus))
        };

    private static BirthStatus? MapBirthStatus(Models.BirthStatus? birthStatus) => birthStatus switch
    {
        Models.BirthStatus.Born =>
            BirthStatus.Born,

        Models.BirthStatus.Due =>
            BirthStatus.Due,

        null => null,

        _ => throw new ArgumentOutOfRangeException(
            nameof(birthStatus))
    };

    private static List<ChildRelatedBenefit> MapChildBenefits(
        Child child) => [.. child.ChildSupportOptions
            .Select(MapChildBenefit)
            .OfType<ChildRelatedBenefit>()];

    private static ChildRelatedBenefit? MapChildBenefit(ChildSupport childSupport) => childSupport switch
    {
        ChildSupport.ArmedForcesIndependencePayment =>
            ChildRelatedBenefit.ArmedForcesIndependencePayment,

        ChildSupport.CertificateOfVisualImpairment =>
            ChildRelatedBenefit.CertificateOfVisualImpairment,

        ChildSupport.DisabilityLivingAllowance =>
            ChildRelatedBenefit.DisabilityLivingAllowance,

        ChildSupport.EducationHealthAndCarePlan =>
            ChildRelatedBenefit.EducationHealthAndCarePlan,

        ChildSupport.PersonalIndependencePayment =>
            ChildRelatedBenefit.PersonalIndependencePayment,

        ChildSupport.NoneOfTheseApply =>
            null,

        _ => throw new ArgumentOutOfRangeException(
            nameof(childSupport))
    };
}
