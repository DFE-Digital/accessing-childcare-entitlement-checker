using Dfe.Acec.RulesEngine.Types;
using Dfe.Acec.Web.Mappers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using AgeRange = Dfe.Acec.Web.Models.AgeRange;
using BirthStatus = Dfe.Acec.Web.Models.BirthStatus;
using CountryOfResidence = Dfe.Acec.Web.Models.CountryOfResidence;
using RulesCountryOfResidence = Dfe.Acec.RulesEngine.Types.CountryOfResidence;
using RulesAgeRange = Dfe.Acec.RulesEngine.Types.AgeRange;
using RulesBirthStatus = Dfe.Acec.RulesEngine.Types.BirthStatus;

namespace Dfe.Acec.Web.Tests.Unit.Mappers;

public class JourneyStateToEntitlementRequestMapperTests
{
    private static JourneyState CreateJourneyState()
    {
        var child = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = new DateOnly(2023, 1, 1),
            ChildSupportOptions =
            [
                ChildSupport.DisabilityLivingAllowance
            ]
        };

        return new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            HasPartner = true,
            UniversalCredit = UniversalCreditOption.Receives,
            UserAge = AgeRange.TwentyOneOrOver,
            SettledStatus = SettledStatusOption.Yes,
            Benefits =
            [
                BenefitsOption.CarersAllowance
            ],
            Nationality = NationalityOption.BritishOrIrishCitizen,
            PaidWork = PaidWorkOption.Yes,
            WorkStatus = [
                WorkStatusOption.PaidEmployment,
                WorkStatusOption.SelfEmployed
            ],
            ChildcareSupport = [
                ChildcareSupportOption.ChildcareBursaryOrGrant
            ],
            PartnerAge = AgeRange.EighteenToTwenty,
            PartnerPaidWork = PartnerPaidWorkOption.No,
            PartnerBenefits =
            [
                 PartnerBenefitsOption.ContributionBasedEmploymentAndSupportAllowance
            ],
            PartnerChildcareSupport = [
                PartnerChildcareSupportOption.ChildcareVouchers
            ],

            Children =
            {
                [child.ChildId] = child
            }
        };
    }

    [Fact]
    public void MapWhenJourneyStateIsPopulatedMapsEntitlementRequest()
    {
        var mapper = new JourneyStateToEntitlementRequestMapper();

        var journeyState = CreateJourneyState();

        var result = mapper.Map(journeyState);

        Assert.NotNull(result);

        // Household
        Assert.NotNull(result.Household.CountryOfResidence);
        Assert.Equal(RulesCountryOfResidence.England, result.Household.CountryOfResidence!.Value);
        Assert.True(result.Household.HasPartner);
        Assert.True(result.Household.ReceivesUniversalCredit);

        // User
        Assert.Equal(RulesAgeRange.TwentyOneOrOver, result.User.AgeRange);
        Assert.True(result.User.HasSettledOrPreSettledStatus);
        Assert.Contains(PersonBenefit.CarersAllowance, result.User.Benefits);
        Assert.Contains(WorkStatus.PaidEmployment, result.User.WorkStatuses);
        Assert.Contains(WorkStatus.SelfEmployed, result.User.WorkStatuses);
        Assert.Equal(Nationality.BritishOrIrishCitizen, result.User.Nationality);
        Assert.Equal(PaidWorkStatus.Yes, result.User.PaidWorkStatus);
        Assert.Contains(ChildcareSupport.ChildcareBursaryOrGrant, result.User.ChildcareSupport);

        // Partner
        Assert.NotNull(result.Partner);
        Assert.Equal(RulesAgeRange.EighteenToTwenty, result.Partner!.AgeRange);
        Assert.Contains(PersonBenefit.ContributionBasedEmploymentAndSupportAllowance, result.Partner.Benefits);
        Assert.Equal(PaidWorkStatus.No, result.Partner.PaidWorkStatus);
        Assert.Contains(ChildcareSupport.ChildcareVouchers, result.Partner.ChildcareSupport);

        // Child
        var child = Assert.Single(result.Children);

        Assert.Equal("child-1", child.ChildId);
        Assert.Equal("Jack", child.Name);
        Assert.Equal(RulesBirthStatus.Born, child.BirthStatus);
        Assert.Equal(new DateOnly(2023, 1, 1), child.DateOfBirth);
        Assert.Contains(ChildRelatedBenefit.DisabilityLivingAllowance, child.ChildRelatedBenefits);
    }

    [Fact]
    public void MapWhenAlternativeValuesProvidedMapsEntitlementRequest()
    {
        var mapper = new JourneyStateToEntitlementRequestMapper();

        var child = new Child("child-1", "Mia")
        {
            BirthStatus = BirthStatus.Due,
            DueDate = new DateOnly(2026, 1, 1),
            ChildSupportOptions =
            [
                ChildSupport.ArmedForcesIndependencePayment,
                ChildSupport.CertificateOfVisualImpairment,
                ChildSupport.EducationHealthAndCarePlan,
                ChildSupport.PersonalIndependencePayment
            ]
        };

        var journeyState = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.Wales,

            UserAge = AgeRange.UnderEighteen,
            PaidWork = PaidWorkOption.No,
            SettledStatus = SettledStatusOption.StillWaiting,
            Nationality = NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland,

            WorkStatus =
            [
                WorkStatusOption.Apprentice
            ],

            Benefits =
            [
                BenefitsOption.EmploymentAndSupportAllowance,
                BenefitsOption.GuaranteedElementOfPensionCredit,
                BenefitsOption.IncapacityBenefit,
                BenefitsOption.LimitedCapabilityForWork,
                BenefitsOption.LimitedCapabilityForWorkRelatedActivity,
                BenefitsOption.SevereDisablementAllowance,
                BenefitsOption.None
            ],

            HasPartner = false,

            Children =
            {
                [child.ChildId] = child
            }
        };

        var result = mapper.Map(journeyState);

        Assert.Equal(RulesCountryOfResidence.Wales, result.Household.CountryOfResidence);
        Assert.Equal(RulesAgeRange.UnderEighteen, result.User.AgeRange);
        Assert.Equal(PaidWorkStatus.No, result.User.PaidWorkStatus);
        Assert.True(result.User.HasSettledOrPreSettledStatus);

        Assert.Null(result.Partner);

        var mappedChild = Assert.Single(result.Children);

        Assert.Equal(RulesBirthStatus.Due, mappedChild.BirthStatus);

        Assert.Contains(ChildRelatedBenefit.ArmedForcesIndependencePayment, mappedChild.ChildRelatedBenefits);
        Assert.Contains(ChildRelatedBenefit.CertificateOfVisualImpairment, mappedChild.ChildRelatedBenefits);
        Assert.Contains(ChildRelatedBenefit.EducationHealthAndCarePlan, mappedChild.ChildRelatedBenefits);
        Assert.Contains(ChildRelatedBenefit.PersonalIndependencePayment, mappedChild.ChildRelatedBenefits);
    }

    [Fact]
    public void MapWhenHasPartnerIsFalseReturnsNullPartner()
    {
        var mapper = new JourneyStateToEntitlementRequestMapper();

        var journeyState = CreateJourneyState();
        journeyState.HasPartner = false;

        var result = mapper.Map(journeyState);

        Assert.Null(result.Partner);
    }

    [Fact]
    public void MapWhenOptionalValuesAreNullMapsNulls()
    {
        var mapper = new JourneyStateToEntitlementRequestMapper();

        var result = mapper.Map(new JourneyState());

        Assert.Null(result.Household.CountryOfResidence);
        Assert.Null(result.User.AgeRange);
        Assert.Null(result.User.PaidWorkStatus);
        Assert.Null(result.User.Nationality);
        Assert.Null(result.User.HasSettledOrPreSettledStatus);
        Assert.Empty(result.Children);
        Assert.Null(result.Partner);
    }
}
