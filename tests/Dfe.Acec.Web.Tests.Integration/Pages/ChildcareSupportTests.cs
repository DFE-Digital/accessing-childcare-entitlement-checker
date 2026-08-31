using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class ChildcareSupportTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _url = $"/benefits/childcare-support";

    [Theory]
    [InlineData(null, "/benefits/benefits")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

        var url = $"{_url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertCheckboxCount(3)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner()
            .AssertGroupHint("Select all that apply");
    }

    [Theory]
    [InlineData(null, ChildcareSupportOption.ChildcareVouchers, null, null, "/benefits/childcare-vouchers")]
    [InlineData(null, ChildcareSupportOption.ChildcareBursaryOrGrant, null, null, "/partner")]
    [InlineData(ReturnTo.CheckAnswers, ChildcareSupportOption.ChildcareVouchers, null, null, "/benefits/childcare-vouchers")]
    [InlineData(ReturnTo.CheckAnswers, ChildcareSupportOption.ChildcareVouchers, ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, null, "/benefits/childcare-vouchers")]
    [InlineData(ReturnTo.CheckAnswers, ChildcareSupportOption.ChildcareBursaryOrGrant, null, null, "/partner")]
    [InlineData(ReturnTo.CheckAnswers, ChildcareSupportOption.ChildcareBursaryOrGrant, null, true, "/partner")]
    public async Task PostValidRedirects(string? returnTo, ChildcareSupportOption childcareSupport, ChildcareVoucherReceiptOption? childcareVoucherReceipt, bool? hasPartner, string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            ChildcareSupport = [childcareSupport],
            ChildcareVoucherReceipt = childcareVoucherReceipt,
            HasPartner = hasPartner,
        });

        using var client = host.CreateClient();
        var url = $"{_url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("ChildcareSupport", childcareSupport.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/benefits/benefits")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task PostInvalidShowsValidationError(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

        var url = $"{_url}?returnTo={returnTo}";
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
