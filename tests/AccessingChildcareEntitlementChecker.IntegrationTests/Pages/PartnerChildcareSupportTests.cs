using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerChildcareSupportTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/benefits/childcare-support-partner";

    [Theory]
    [InlineData(null, "/Partner/PartnerBenefits")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertCheckboxCount(3)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, PartnerChildcareSupportOption.ChildcareVouchers, null, "/benefits/childcare-vouchers-partner")]
    [InlineData(null, PartnerChildcareSupportOption.ChildcareBursaryOrGrant, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PartnerChildcareSupportOption.ChildcareVouchers, null, "/benefits/childcare-vouchers-partner")]
    [InlineData(ReturnTo.CheckAnswers, PartnerChildcareSupportOption.ChildcareVouchers, ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PartnerChildcareSupportOption.ChildcareBursaryOrGrant, null, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, PartnerChildcareSupportOption partnerChildcareSupport, ChildcareVoucherReceiptOption? partnerChildcareVoucherReceipt, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerChildcareSupport = [partnerChildcareSupport],
            PartnerChildcareVoucherReceipt = partnerChildcareVoucherReceipt,
        });
        var url = $"{Url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("PartnerChildcareSupport", partnerChildcareSupport.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/Partner/PartnerBenefits")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [], TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertValidationError()
            .AssertBackLink(backLinkUrl);
    }
}
