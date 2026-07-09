using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class CheckYourAnswersTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(false, null, "/partner")]
    [InlineData(true, null, "/benefits/childcare-support-partner")]
    [InlineData(true, PartnerChildcareSupportOption.ChildcareVouchers, "/benefits/childcare-vouchers-partner")]
    public async Task Get(
        bool? hasPartner,
        PartnerChildcareSupportOption? partnerChildcareSupport,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            HasPartner = hasPartner,
            PartnerChildcareSupport = partnerChildcareSupport.HasValue ? [partnerChildcareSupport.Value] : [],
        });

        var url = $"/check-your-answers";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }
}
