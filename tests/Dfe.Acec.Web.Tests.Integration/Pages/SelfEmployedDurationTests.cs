using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class SelfEmployedDurationTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _url = "/work-status/self-employed";

    [Theory]
    [InlineData(null, "/work-status/work-status")]
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
        doc.AssertRadioButtonCount(2)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, SelfEmployedDurationOption.LessThan12Months, null, null, "/benefits/universal-credit")]
    [InlineData(null, SelfEmployedDurationOption.NotLessThan12Months, null, null, "/earnings/wage")]
    [InlineData(ReturnTo.CheckAnswers, SelfEmployedDurationOption.LessThan12Months, null, null, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, SelfEmployedDurationOption.LessThan12Months, UniversalCreditOption.Receives, null, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, SelfEmployedDurationOption.NotLessThan12Months, null, null, "/earnings/wage")]
    [InlineData(ReturnTo.CheckAnswers, SelfEmployedDurationOption.NotLessThan12Months, null, WeeklyEarningsOption.AboveThreshold, "/earnings/wage")]
    public async Task PostValidRedirects(string? returnTo, SelfEmployedDurationOption selfEmployedDuration, UniversalCreditOption? universalCredit, WeeklyEarningsOption? weeklyEarnings, string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            SelfEmployedDuration = selfEmployedDuration,
            UniversalCredit = universalCredit,
            WeeklyEarnings = weeklyEarnings,
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
            new KeyValuePair<string, string>("SelfEmployedDuration", selfEmployedDuration.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/work-status/work-status")]
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
