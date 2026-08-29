using Dfe.Acec.Tests.Integration.Fixtures;
using Dfe.Acec.Tests.Integration.Helpers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Tests.Integration.Pages;

public class SettledStatusTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/nationality/settled-status";

    [Theory]
    [InlineData(null, "/nationality")]
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
        doc
            .AssertRadioButtonCount(3)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, SettledStatusOption.Yes, null)]
    [InlineData(null, SettledStatusOption.No, null)]
    [InlineData(null, SettledStatusOption.StillWaiting, null)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.Yes, null)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.Yes, PaidWorkOption.Yes)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.No, null)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.No, PaidWorkOption.Yes)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.StillWaiting, null)]
    [InlineData(ReturnTo.CheckAnswers, SettledStatusOption.StillWaiting, PaidWorkOption.Yes)]
    public async Task PostValidRedirects(string? returnTo, SettledStatusOption settledStatus, PaidWorkOption? paidWork)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            SettledStatus = settledStatus,
            PaidWork = paidWork,
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
            new KeyValuePair<string, string>("SettledStatus", settledStatus.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect("/work-status/work");
    }

    [Theory]
    [InlineData(null, "/nationality")]
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
