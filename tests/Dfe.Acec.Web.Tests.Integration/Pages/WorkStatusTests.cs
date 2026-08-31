using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class WorkStatusTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _url = "/work-status/work-status";

    [Theory]
    [InlineData(null, "/work-status/work")]
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
    [InlineData(null, WorkStatusOption.SelfEmployed, null, null, "/work-status/self-employed")]
    [InlineData(null, WorkStatusOption.PaidEmployment, null, null, "/earnings/wage")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.SelfEmployed, null, null, "/work-status/self-employed")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.LessThan12Months, null, "/work-status/self-employed")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, null, "/earnings/wage")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, "/earnings/wage")]
    public async Task PostValidRedirects(string? returnTo, WorkStatusOption workStatus, SelfEmployedDurationOption? selfEmployedDuration, WeeklyEarningsOption? weeklyEarnings, string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            WorkStatus = [workStatus],
            SelfEmployedDuration = selfEmployedDuration,
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
            new KeyValuePair<string, string>("WorkStatus", workStatus.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/work-status/work")]
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

    [Fact]
    public async Task PostSelectionRedirectsToSelfEmployed()
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

        var getResponse = await client.GetAsync(_url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, _url, cookie, token, [
                new("WorkStatus", "SelfEmployed"),
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect("/work-status/self-employed");
    }
}
