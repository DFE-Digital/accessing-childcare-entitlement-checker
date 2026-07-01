using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using AgeRange = AccessingChildcareEntitlementChecker.RulesEngine.Types.AgeRange;
using CountryOfResidence = AccessingChildcareEntitlementChecker.RulesEngine.Types.CountryOfResidence;
using BirthStatus = AccessingChildcareEntitlementChecker.RulesEngine.Types.BirthStatus;

namespace AccessingChildcareEntitlementChecker.Web.Mappers;

public class JourneyStateToEntitlementRequestMapper
{

    public EntitlementRequest Map(JourneyState journeyState)
    {
        return new EntitlementRequest
        {
            Household = MapHousehold(journeyState),
            User = MapUser(journeyState),
            Partner = MapPartner(journeyState),
            Children = MapChildren(journeyState)
        };
    }

    private static HouseholdDto MapHousehold(JourneyState journeyState)
    {
        return new HouseholdDto
        {
            CountryOfResidence = MapCountryOfResidence(journeyState.CountryOfResidence),
            HasPartner = journeyState.HasPartner == true,
            ReceivesUniversalCredit = journeyState.UniversalCredit == UniversalCreditOption.Receives,
        };
    }

    private static PersonDto MapUser(JourneyState journeyState)
    {
        return new PersonDto
        {
            AgeRange = MapAgeRange(journeyState.UserAge),
            IsInPaidWork = MapPaidWork(journeyState.PaidWork),
            WorkStatuses = journeyState.WorkStatus.Select(MapWorkStatus).ToList(),
            SelfEmployedLessThan12Months = journeyState.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months,
            EarnsAboveThreshold = journeyState.WeeklyEarnings == WeeklyEarningsOption.AboveThreshold,
            ExceedsAdjustedNetIncomeLimit = journeyState.YearlyEarnings == YearlyEarningsOption.AboveThreshold,
            Benefits = journeyState.Benefits.Select(MapPersonBenefit).OfType<PersonBenefit>().ToList(),
            ChildcareSupport = journeyState.ChildcareSupport.Select(MapChildcareSupport).OfType<ChildcareSupport>().ToList(),
            Nationality = MapNationality(journeyState.Nationality),
            HasSettledOrPreSettledStatus = MapSettledStatus(journeyState.SettledStatus),
        };
    }

    private static PersonDto? MapPartner(JourneyState journeyState)
    {
        if (journeyState.HasPartner != true)
        {
            return null;
        }

        return new PersonDto
        {
            AgeRange = MapAgeRange(journeyState.PartnerAge),
            IsInPaidWork = MapPaidWork(journeyState.PartnerPaidWork),
            WorkStatuses = journeyState.PartnerWorkStatus.Select(MapWorkStatus).ToList(),
            SelfEmployedLessThan12Months = journeyState.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months,
            EarnsAboveThreshold = journeyState.PartnerWeeklyEarnings == WeeklyEarningsOption.AboveThreshold,
            ExceedsAdjustedNetIncomeLimit = journeyState.PartnerYearlyEarnings == YearlyEarningsOption.AboveThreshold,
            Benefits = journeyState.PartnerBenefits.Select(MapPersonBenefit).OfType<PersonBenefit>().ToList(),
            ChildcareSupport = journeyState.PartnerChildcareSupport.Select(MapPartnerChildcareSupport).OfType<ChildcareSupport>().ToList(),
            Nationality = MapNationality(journeyState.PartnerNationality),
            HasSettledOrPreSettledStatus = MapSettledStatus(journeyState.PartnerSettledStatus),
        };
    }

    private static List<ChildDto> MapChildren(
        JourneyState journeyState)
    {
        var children = new List<ChildDto>();

        foreach (var child in journeyState.Children.Values)
        {
            children.Add(MapChild(child));
        }

        return children;
    }

    private static ChildDto MapChild(Child child)
    {
        return new ChildDto()
        {
            ChildId = child.ChildId,
            Name = child.Name,
            BirthStatus = MapBirthStatus(child.BirthStatus),
            DateOfBirth = child.BirthDate,
            DueDate = child.DueDate,
            ChildRelatedBenefits = MapChildBenefits(child)
        };
    }

    private static CountryOfResidence?
        MapCountryOfResidence(Web.Models.CountryOfResidence? country)
    {
        return country switch
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
    }

    private static AgeRange? MapAgeRange(Web.Models.AgeRange? ageRange)
    {
        return ageRange switch
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
    }

    private static bool? MapPaidWork(PaidWorkOption? paidWork)
    {
        return paidWork switch
        {
            PaidWorkOption.Yes => true,
            PaidWorkOption.No => false,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(paidWork))
        };
    }

    private static bool? MapPaidWork(PartnerPaidWorkOption? paidWork)
    {
        return paidWork switch
        {
            PartnerPaidWorkOption.Yes => true,
            PartnerPaidWorkOption.No => false,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(paidWork))
        };
    }


    private static WorkStatus MapWorkStatus(WorkStatusOption workStatus)
    {
        return workStatus switch
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
    }

    private static PersonBenefit? MapPersonBenefit(BenefitsOption benefit)
    {
        return benefit switch
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
    }

    private static PersonBenefit? MapPersonBenefit(
        PartnerBenefitsOption benefit)
    {
        return MapPersonBenefit(
            Enum.Parse<BenefitsOption>(
                benefit.ToString()));
    }

    private static ChildcareSupport? MapChildcareSupport(ChildcareSupportOption childcareSupport)
    {
        return childcareSupport switch
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
    }

    private static ChildcareSupport? MapPartnerChildcareSupport(PartnerChildcareSupportOption partnerChildcareSupport)
    {
        return partnerChildcareSupport switch
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
    }

    private static Nationality? MapNationality(NationalityOption? nationality)
    {
        return nationality switch
        {
            NationalityOption.BritishOrIrishCitizen =>
                Nationality.BritishOrIrishCitizen,

            NationalityOption.CitizenOfADifferentCountry =>
                Nationality.Other,

            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland =>
                Nationality.EuropeanUnionEuropeanEconomicAreaOrSwissCitizen,

            null => null,

            _ => throw new ArgumentOutOfRangeException(
                nameof(nationality),
                nationality,
                null)
        };
    }

    private static bool? MapSettledStatus(
        SettledStatusOption? settledStatus)
    {
        return settledStatus switch
        {
            SettledStatusOption.Yes or SettledStatusOption.StillWaiting => true,

            SettledStatusOption.No => false,

            null => null,

            _ => throw new ArgumentOutOfRangeException(
                nameof(settledStatus))
        };
    }

    private static BirthStatus? MapBirthStatus(
        Models.BirthStatus? birthStatus)
    {
        return birthStatus switch
        {
            Models.BirthStatus.Born =>
                BirthStatus.Born,

            Models.BirthStatus.Due =>
                BirthStatus.Due,

            null => null,

            _ => throw new ArgumentOutOfRangeException(
                nameof(birthStatus))
        };
    }

    private static List<ChildRelatedBenefit> MapChildBenefits(
        Child child)
    {
        return child.ChildSupportOptions
            .Select(MapChildBenefit)
            .OfType<ChildRelatedBenefit>()
            .ToList();
    }

    private static ChildRelatedBenefit? MapChildBenefit(
        ChildSupport childSupport)
    {
        return childSupport switch
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
}
