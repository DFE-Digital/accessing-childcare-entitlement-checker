using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Services;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.UnitTests.RulesEngine.Services;

public class EntitlementRulesEngineTests
{
    [Fact]
    public void Evaluate_WhenSchemeReturnsResult_IncludesSchemeInResponse()
    {
        var evaluators = new List<ISchemeEvaluator>
        {
            new FakeEligibleSchemeEvaluator()
        };

        var engine = new EntitlementRulesEngine(evaluators);

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
                Benefits = []
            },

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
        var result = engine.Evaluate(request, today);

        Assert.Single(result.ChildResults);
        Assert.Single(result.ChildResults[0].Schemes);
        Assert.Equal(
            SchemeCode.UniversalCreditChildcare,
            result.ChildResults[0].Schemes[0].SchemeCode);
    }

    [Fact]
    public void Evaluate_WhenSchemeReturnsNull_ExcludesSchemeFromResponse()
    {
        var evaluators = new List<ISchemeEvaluator>
        {
            new FakeIneligibleSchemeEvaluator()
        };

        var engine = new EntitlementRulesEngine(evaluators);

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
                Benefits = []
            },

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
        var result = engine.Evaluate(request, today);

        Assert.Single(result.ChildResults);
        Assert.Empty(result.ChildResults[0].Schemes);
    }

    [Fact]
    public void Evaluate_WhenRequestContainsMultipleChildren_EvaluatesEachChild()
    {
        var evaluators = new List<ISchemeEvaluator>
        {
            new FakeEligibleSchemeEvaluator()
        };

        var engine = new EntitlementRulesEngine(evaluators);

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
                Benefits = []
            },

            Children =
            [
                new ChildDto
                {
                    Name = "Jack",
                    BirthStatus = BirthStatus.Born,
                    DateOfBirth = new DateOnly(2022, 1, 1)
                },

                new ChildDto
                {
                    Name = "Sophie",
                    BirthStatus = BirthStatus.Born,
                    DateOfBirth = new DateOnly(2021, 1, 1)
                }
            ]
        };

        var today = new DateOnly(2025, 1, 1);
        var result = engine.Evaluate(request, today);

        Assert.Equal(2, result.ChildResults.Count);
        Assert.Equal("Jack", result.ChildResults[0].ChildName);
        Assert.Equal("Sophie", result.ChildResults[1].ChildName);
        Assert.Single(result.ChildResults[0].Schemes);
        Assert.Single(result.ChildResults[1].Schemes);
    }

    private class FakeEligibleSchemeEvaluator : ISchemeEvaluator
    {
        public SchemeResultDto? Evaluate(
            DerivedContext context,
            ChildFacts child)
        {
            return new SchemeResultDto
            {
                SchemeCode = SchemeCode.UniversalCreditChildcare,
                EligibleNow = true,
                EligibleInFuture = false
            };
        }
    }

    private class FakeIneligibleSchemeEvaluator : ISchemeEvaluator
    {
        public SchemeResultDto? Evaluate(
            DerivedContext context,
            ChildFacts child)
        {
            return null;
        }
    }
}