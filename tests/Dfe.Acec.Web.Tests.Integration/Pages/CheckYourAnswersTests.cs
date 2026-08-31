using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class CheckYourAnswersTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _url = "/check-your-answers";

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

        var response = await client.GetAsync(_url, TestContext.Current.CancellationToken);
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

        var response = await client.GetAsync(_url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }
}
