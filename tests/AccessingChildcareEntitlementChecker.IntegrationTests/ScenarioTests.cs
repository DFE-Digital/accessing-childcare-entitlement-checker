using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.RulesEngine.Services;
using AccessingChildcareEntitlementChecker.Web.Mappers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AccessingChildcareEntitlementChecker.IntegrationTests
{
    /// <summary>
    /// Integration tests to allow stepping through scenarios in debugger.
    /// </summary>
    public class ScenarioTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
    {
        [Fact]
        public async Task AC_891()
        {
            var journeyState = new JourneyState
            {
                CountryOfResidence = CountryOfResidence.England,
                Children = new Dictionary<string, Child>
                {
                    { "1", new Child("1", "Isabel") { BirthStatus = BirthStatus.Born, BirthDate = new DateOnly(2024, 3, 1), ChildSupportOptions = [], } },
                    { "2", new Child("2", "Mary") { BirthStatus = BirthStatus.Born, BirthDate = new DateOnly(2026, 4, 1), ChildSupportOptions = [], } },
                },
                UserAge = AgeRange.TwentyOneOrOver,
                Nationality = NationalityOption.BritishOrIrishCitizen,
                PaidWork = PaidWorkOption.No,
                UniversalCredit = UniversalCreditOption.Receives,
                Benefits = [BenefitsOption.ContributionBasedEmploymentAndSupportAllowance],
                ChildcareSupport = [ChildcareSupportOption.None],
                HasPartner = true,
                PartnerAge = AgeRange.TwentyOneOrOver,
                PartnerNationality = NationalityOption.BritishOrIrishCitizen,
                PartnerPaidWork = PartnerPaidWorkOption.Yes,
                PartnerWorkStatus = [WorkStatusOption.PaidEmployment],
                PartnerWeeklyEarnings = WeeklyEarningsOption.AboveThreshold,
                PartnerYearlyEarnings = YearlyEarningsOption.BelowThreshold,
                PartnerBenefits = [PartnerBenefitsOption.None],
                PartnerChildcareSupport = [PartnerChildcareSupportOption.None],
            };

            var httpClient = factory.CreateClientWithJourneyState(journeyState);

            // probably better here to actually make an http call.
            using var scope = factory.Services.CreateScope();
            var journeyStateMapper = scope.ServiceProvider.GetRequiredService<JourneyStateToEntitlementRequestMapper>();
            var rulesEngine = scope.ServiceProvider.GetRequiredService<EntitlementRulesEngine>();

            var request = journeyStateMapper.Map(journeyState);
            var response = rulesEngine.Evaluate(request, DateOnly.FromDateTime(DateTime.Today));
            Assert.Equal(response.ChildResults.Count, 2);

        }
    }
}
