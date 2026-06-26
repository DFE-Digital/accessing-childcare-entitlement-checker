using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerChildcareVoucherReceiptTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(null, "/benefits/childcare-support-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get_Has_Input_And_BackLink(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"/benefits/childcare-vouchers-partner?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc
            .AssertRadioButtonCount(3)
            .AssertBackLink(backLinkUrl);
    }

    [Theory]
    [InlineData(null, ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, "/check-your-answers")]
    [InlineData(null, ChildcareVoucherReceiptOption.EmployerArrangesWithProvider, "/check-your-answers")]
    [InlineData(null, ChildcareVoucherReceiptOption.ThroughSalarySacrifice, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, ChildcareVoucherReceiptOption.EmployerArrangesWithProvider, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, ChildcareVoucherReceiptOption.ThroughSalarySacrifice, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, ChildcareVoucherReceiptOption partnerChildcareVoucherReceipt, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerChildcareVoucherReceipt = partnerChildcareVoucherReceipt,
        });
        var url = $"/benefits/childcare-vouchers-partner?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("PartnerChildcareVoucherReceipt", partnerChildcareVoucherReceipt.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/benefits/childcare-support-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"/benefits/childcare-vouchers-partner?returnTo={returnTo}";
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
