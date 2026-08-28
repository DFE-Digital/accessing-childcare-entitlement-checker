using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class CheckYourAnswersTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/check-your-answers";

    [Fact]
    public async Task Get_WhenFeatureFlagEnabled_SuppressesLocationRow()
    {
        using var host = factory.CreateClientWithJourneyStateAndFeatureFlags(new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            HasPartner = false,
        }, new()
        {
            { "FeatureManagement:HmrcIntegration", "true" }
        });

        using var client = host.CreateClient();

        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.DoesNotContain("Where do you live?", content);
    }

    [Theory]
    [InlineData(false, null, "/partner")]
    [InlineData(true, null, "/benefits/childcare-support-partner")]
    [InlineData(true, PartnerChildcareSupportOption.ChildcareVouchers, "/benefits/childcare-vouchers-partner")]
    public async Task Get(
        bool? hasPartner,
        PartnerChildcareSupportOption? partnerChildcareSupport,
        string backLinkUrl)
    {
        using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            HasPartner = hasPartner,
            PartnerChildcareSupport = partnerChildcareSupport.HasValue ? [partnerChildcareSupport.Value] : [],
        });

        using var client = host.CreateClient();

        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }
}
