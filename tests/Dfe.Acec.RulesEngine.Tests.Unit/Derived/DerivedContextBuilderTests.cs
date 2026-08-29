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
                Nationality = Nationality.BritishOrIrishCitizen,
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
}