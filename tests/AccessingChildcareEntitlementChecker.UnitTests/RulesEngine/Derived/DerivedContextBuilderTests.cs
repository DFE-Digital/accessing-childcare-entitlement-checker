using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.UnitTests.RulesEngine.Derived;

public class DerivedContextBuilderTests
{
    [Fact]
    public void Build_WhenUserIsBritish_SetsHasAccessToPublicFundsTrue()
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
                IsInPaidWork = false
            }
        };

        var today = new DateOnly(2025, 1, 1);
        var result = DerivedContextBuilder.Build(request, today);

        Assert.True(result.Household.HasAccessToPublicFunds);
    }

    [Fact]
    public void Build_WhenChildHasDateOfBirth_CalculatesAgeInYears()
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
    public void Build_WhenCountryIsWales_SetsLivesInGreatBritainTrue()
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