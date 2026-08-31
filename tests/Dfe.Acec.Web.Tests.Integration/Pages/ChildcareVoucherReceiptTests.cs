using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class ChildcareVoucherReceiptTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _url = $"/benefits/childcare-vouchers";

    [Theory]
    [InlineData(null, "/benefits/childcare-support")]
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
        doc
            .AssertRadioButtonCount(3)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, null)]
    [InlineData(null, ChildcareVoucherReceiptOption.EmployerArrangesWithProvider, null)]
    [InlineData(null, ChildcareVoucherReceiptOption.ThroughSalarySacrifice, null)]
    [InlineData(ReturnTo.CheckAnswers, ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, null)]
    [InlineData(ReturnTo.CheckAnswers, ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, true)]
    [InlineData(ReturnTo.CheckAnswers, ChildcareVoucherReceiptOption.EmployerArrangesWithProvider, null)]
    [InlineData(ReturnTo.CheckAnswers, ChildcareVoucherReceiptOption.EmployerArrangesWithProvider, true)]
    [InlineData(ReturnTo.CheckAnswers, ChildcareVoucherReceiptOption.ThroughSalarySacrifice, null)]
    [InlineData(ReturnTo.CheckAnswers, ChildcareVoucherReceiptOption.ThroughSalarySacrifice, true)]
    public async Task PostValidRedirects(string? returnTo, ChildcareVoucherReceiptOption childcareVoucherReceipt, bool? hasPartner)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
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
            new KeyValuePair<string, string>("ChildcareVoucherReceipt", childcareVoucherReceipt.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect("/partner");
    }

    [Theory]
    [InlineData(null, "/benefits/childcare-support")]
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
