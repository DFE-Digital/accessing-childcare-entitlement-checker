using AccessingChildcareEntitlementChecker.RulesEngine.Types;
using AccessingChildcareEntitlementChecker.Web.Mappers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using AgeRange = AccessingChildcareEntitlementChecker.Web.Models.AgeRange;
using BirthStatus = AccessingChildcareEntitlementChecker.Web.Models.BirthStatus;
using CountryOfResidence = AccessingChildcareEntitlementChecker.Web.Models.CountryOfResidence;
using RulesCountryOfResidence = AccessingChildcareEntitlementChecker.RulesEngine.Types.CountryOfResidence;
using RulesAgeRange = AccessingChildcareEntitlementChecker.RulesEngine.Types.AgeRange;
using RulesBirthStatus = AccessingChildcareEntitlementChecker.RulesEngine.Types.BirthStatus;

namespace AccessingChildcareEntitlementChecker.UnitTests.Mappers;

public class JourneyStateToEntitlementRequestMapperTests
{
    private static JourneyState CreateJourneyState()
    {
        var child = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = new DateOnly(2023, 1, 1),
            BornRelationship = Relationship.Parent,
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
            PartnerAge = AgeRange.TwentyOneOrOver,
            PartnerPaidWork = PartnerPaidWorkOption.Yes,
            PartnerBenefits =
            [
                 PartnerBenefitsOption.ContributionBasedEmploymentAndSupportAllowance
            ],

            Children =
            {
                [child.ChildId] = child
            }
        };
    }

    [Fact]
    public void Map_WhenJourneyStateIsPopulated_MapsEntitlementRequest()
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
        Assert.Equal(Nationality.BritishOrIrishCitizen, result.User.Nationality);
        Assert.True(result.User.IsInPaidWork);

        // Partner
        Assert.NotNull(result.Partner);
        Assert.Equal(RulesAgeRange.TwentyOneOrOver, result.Partner!.AgeRange);
        Assert.Contains(PersonBenefit.ContributionBasedEmploymentAndSupportAllowance, result.Partner.Benefits);
        Assert.True(result.Partner.IsInPaidWork);

        // Child
        var child = Assert.Single(result.Children);

        Assert.Equal("child-1", child.ChildId);
        Assert.Equal("Jack", child.Name);
        Assert.Equal(RulesBirthStatus.Born, child.BirthStatus);
        Assert.Equal(new DateOnly(2023, 1, 1), child.DateOfBirth);
        Assert.Equal(RelationshipToChild.Parent, child.RelationshipToChild);
        Assert.Contains(ChildRelatedBenefit.DisabilityLivingAllowance, child.ChildRelatedBenefits);
    }

    [Fact]
    public void Map_WhenHasPartnerIsFalse_ReturnsNullPartner()
    {
        var mapper = new JourneyStateToEntitlementRequestMapper();

        var journeyState = CreateJourneyState();
        journeyState.HasPartner = false;

        var result = mapper.Map(journeyState);

        Assert.Null(result.Partner);
    }
}