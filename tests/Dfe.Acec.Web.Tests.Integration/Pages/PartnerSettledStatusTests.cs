using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class PartnerSettledStatusTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/nationality/settled-status-partner";

    [Theory]
    [InlineData(null, "/nationality/nationality-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, SettledStatusOption.Yes, null)]
    [InlineData(null, SettledStatusOption.No, null)]
    [InlineData(null, SettledStatusOption.StillWaiting, null)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.Yes, null)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.Yes, PartnerPaidWorkOption.Yes)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.No, null)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.No, PartnerPaidWorkOption.Yes)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.StillWaiting, null)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.StillWaiting, PartnerPaidWorkOption.Yes)]
    public async Task PostValidRedirects(string? returnTo, SettledStatusOption partnerSettledStatus, PartnerPaidWorkOption? partnerPaidWork)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerSettledStatus = partnerSettledStatus,
            PartnerPaidWork = partnerPaidWork,
        });

        using var client = host.CreateClient();
        var url = $"{Url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("PartnerSettledStatus", partnerSettledStatus.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect("/work-status/work-partner");
    }

    [Theory]
    [InlineData(null, "/nationality/nationality-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task PostInvalidShowsValidationError(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

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
