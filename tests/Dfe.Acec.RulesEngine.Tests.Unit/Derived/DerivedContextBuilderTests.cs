using Dfe.Acec.RulesEngine.Derived;
using Dfe.Acec.RulesEngine.Dtos.Requests;
using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Tests.Unit.Derived;

public class DerivedContextBuilderTests
{
    [Fact]
    public void BuildWhenUserIsBritishSetsHasAccessToPublicFundsTrue()
    {
        var request = new EntitlementRequest
        {
            Household = new HouseholdDto
            {
                HasPartner = false,
                CountryOfResidence = CountryOfResidence.England
            },

            User = new PersonDto
            {
                Nationalities = [Nationality.BritishOrIrishCitizen],
                HasSettledOrPreSettledStatus = false,
                Benefits = [],
                PaidWorkStatus = PaidWorkStatus.Yes

            }
        };

        var today = new DateOnly(2025, 1, 1);
        var result = DerivedContextBuilder.Build(request, today);

        Assert.True(result.Household.HasAccessToPublicFunds);
    }

    [Fact]
    public void BuildWhenChildHasDateOfBirthCalculatesAgeInYears()
    {
        var request = new EntitlementRequest
        {
            Children =
            [
                new ChildDto
                {
                    Name = "Jack",
                    BirthStatus = BirthStatus.Born,
                    DateOfBirth = new DateOnly(2022, 1, 1)
                }
            ]
        };

        var today = new DateOnly(2025, 1, 1);
        var result = DerivedContextBuilder.Build(request, today);

        Assert.Equal(3, result.Children[0].AgeInYears);
    }

    [Fact]
    public void BuildWhenCountryIsWalesSetsLivesInGreatBritainTrue()
    {
        var request = new EntitlementRequest
        {
            Household = new HouseholdDto
            {
                HasPartner = false,
                CountryOfResidence = CountryOfResidence.Wales
            }
        };

        var today = new DateOnly(2025, 1, 1);
        var result = DerivedContextBuilder.Build(request, today);

        Assert.True(result.Household.LivesInGreatBritain);
    }

    [Theory]
    [InlineData(PaidWorkStatus.SickLeave, PaidWorkStatus.Yes, true)]
    [InlineData(PaidWorkStatus.Yes, PaidWorkStatus.SickLeave, true)]
    [InlineData(PaidWorkStatus.SickLeave, PaidWorkStatus.SickLeave, true)]
    [InlineData(PaidWorkStatus.Yes, PaidWorkStatus.Yes, false)]
    [InlineData(PaidWorkStatus.SickLeave, null, true)]
    [InlineData(PaidWorkStatus.Yes, null, false)]
    public void BuildSetsUserOrPartnerIsOnSickLeave(PaidWorkStatus userPaidWorkStatus, PaidWorkStatus? partnerPaidWorkStatus, bool expected)
    {
        var request = new EntitlementRequest
        {
            Household = new HouseholdDto
            {
                HasPartner = partnerPaidWorkStatus.HasValue,
                CountryOfResidence = CountryOfResidence.England
            },
            User = new PersonDto
            {
                PaidWorkStatus = userPaidWorkStatus
            },
            Partner = partnerPaidWorkStatus.HasValue
                ? new PersonDto
                {
                    PaidWorkStatus = partnerPaidWorkStatus.Value
                }
                : null
        };

        var today = new DateOnly(2025, 1, 1);
        var result = DerivedContextBuilder.Build(request, today);

        Assert.Equal(expected, result.Household.UserOrPartnerIsOnSickLeave);
    }
}
