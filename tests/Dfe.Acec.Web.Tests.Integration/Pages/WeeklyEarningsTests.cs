using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class WeeklyEarningsTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _url = "/earnings/wage";

    [Theory]
    [InlineData(null, WorkStatusOption.PaidEmployment, null, "/work-status/work-status")]
    [InlineData(null, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.NotLessThan12Months, "/work-status/self-employed")]
    [InlineData(null, WorkStatusOption.Apprentice, null, "/work-status/work-status")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, WorkStatusOption.PaidEmployment, null, "/children/check-childs-details")]
    public async Task Get(
        string? returnTo,
        WorkStatusOption workStatus,
        SelfEmployedDurationOption? selfEmployedDuration,
        string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            UserAge = AgeRange.UnderEighteen,
            WorkStatus = [workStatus],
            SelfEmployedDuration = selfEmployedDuration,
        });

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
    [InlineData(null, WeeklyEarningsOption.AboveThreshold, null, null, "/earnings/adjusted-net-income")]
    [InlineData(null, WeeklyEarningsOption.BelowThreshold, null, null, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, WeeklyEarningsOption.AboveThreshold, null, null, "/earnings/adjusted-net-income")]
    [InlineData(ReturnTo.CheckAnswers, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.AboveThreshold, null, "/earnings/adjusted-net-income")]
    [InlineData(ReturnTo.CheckAnswers, WeeklyEarningsOption.BelowThreshold, null, null, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, WeeklyEarningsOption.BelowThreshold, null, UniversalCreditOption.Receives, "/benefits/universal-credit")]
    public async Task PostValidRedirects(string? returnTo, WeeklyEarningsOption weeklyEarnings, YearlyEarningsOption? yearlyEarnings, UniversalCreditOption? universalCredit, string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            UserAge = AgeRange.UnderEighteen,
            WorkStatus = [WorkStatusOption.PaidEmployment],
            WeeklyEarnings = weeklyEarnings,
            YearlyEarnings = yearlyEarnings,
            UniversalCredit = universalCredit,
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
            new KeyValuePair<string, string>("WeeklyEarnings", weeklyEarnings.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, WorkStatusOption.PaidEmployment, null, "/work-status/work-status")]
    [InlineData(null, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.NotLessThan12Months, "/work-status/self-employed")]
    [InlineData(null, WorkStatusOption.Apprentice, null, "/work-status/work-status")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, WorkStatusOption.PaidEmployment, null, "/children/check-childs-details")]
    public async Task PostInvalidShowsValidationError(
        string? returnTo,
        WorkStatusOption workStatus,
        SelfEmployedDurationOption? selfEmployedDuration,
        string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            UserAge = AgeRange.UnderEighteen,
            WorkStatus = [workStatus],
            SelfEmployedDuration = selfEmployedDuration,
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

        var postResponse = await HttpClientHelpers.PostFormAsync(
            client,
            url,
            cookie,
            token,
            [],
            TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertValidationError()
            .AssertBackLink(backLinkUrl);
    }
}
