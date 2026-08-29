using Dfe.Acec.Tests.Integration.Fixtures;
using Dfe.Acec.Tests.Integration.Helpers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Tests.Integration.Pages;

public class CheckYourAnswersTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/check-your-answers";

    [Fact]
    public async Task GetWhenFeatureFlagEnabledSuppressesLocationRow()
    {
        await using var host = factory.CreateClientWithJourneyStateAndFeatureFlags(new JourneyState
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
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
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
