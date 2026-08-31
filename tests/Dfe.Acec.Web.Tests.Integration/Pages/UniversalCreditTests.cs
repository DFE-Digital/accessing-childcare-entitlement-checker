using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class UniversalCreditTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _url = "/benefits/universal-credit";

    /// <remarks>
    /// N.b. Skips type of leave - design is pending.
    /// </remarks>
    [Theory]
    [InlineData(null, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/earnings/adjusted-net-income")]
    [InlineData(null, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.BelowThreshold, null, "/earnings/wage")]
    [InlineData(null, PaidWorkOption.Yes, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.LessThan12Months, null, null, "/work-status/self-employed")]
    [InlineData(null, PaidWorkOption.No, null, null, null, null, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/children/check-childs-details")]
    public async Task Get(
        string? returnTo,
        PaidWorkOption? paidWork,
        WorkStatusOption? workStatus,
        SelfEmployedDurationOption? selfEmployedDuration,
        WeeklyEarningsOption? weeklyEarnings,
        YearlyEarningsOption? yearlyEarnings,
        string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            PaidWork = paidWork,
            WorkStatus = workStatus.HasValue ? [workStatus.Value] : [],
            SelfEmployedDuration = selfEmployedDuration,
            WeeklyEarnings = weeklyEarnings,
            YearlyEarnings = yearlyEarnings,
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
    [InlineData(null, UniversalCreditOption.Receives, null)]
    [InlineData(null, UniversalCreditOption.DoesNotReceive, null)]
    [InlineData(ReturnTo.CheckAnswers, UniversalCreditOption.Receives, null)]
    [InlineData(ReturnTo.CheckAnswers, UniversalCreditOption.Receives, BenefitsOption.CarersAllowance)]
    [InlineData(ReturnTo.CheckAnswers, UniversalCreditOption.DoesNotReceive, null)]
    [InlineData(ReturnTo.CheckAnswers, UniversalCreditOption.DoesNotReceive, BenefitsOption.CarersAllowance)]
    public async Task PostValidRedirects(string? returnTo, UniversalCreditOption universalCredit, BenefitsOption? benefits)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            UniversalCredit = universalCredit,
            Benefits = benefits is null ? new() : [benefits.Value],
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
            new KeyValuePair<string, string>("UniversalCredit", universalCredit.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect("/benefits/benefits");
    }

    [Theory]
    [InlineData(null, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/earnings/adjusted-net-income")]
    [InlineData(null, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.BelowThreshold, null, "/earnings/wage")]
    [InlineData(null, PaidWorkOption.Yes, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.LessThan12Months, null, null, "/work-status/self-employed")]
    [InlineData(null, PaidWorkOption.No, null, null, null, null, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/children/check-childs-details")]
    public async Task PostInvalidShowsValidationError(
        string? returnTo,
        PaidWorkOption? paidWork,
        WorkStatusOption? workStatus,
        SelfEmployedDurationOption? selfEmployedDuration,
        WeeklyEarningsOption? weeklyEarnings,
        YearlyEarningsOption? yearlyEarnings,
        string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            PaidWork = paidWork,
            WorkStatus = workStatus.HasValue ? [workStatus.Value] : [],
            SelfEmployedDuration = selfEmployedDuration,
            WeeklyEarnings = weeklyEarnings,
            YearlyEarnings = yearlyEarnings,
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

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [], TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertValidationError()
            .AssertBackLink(backLinkUrl);
    }
}
