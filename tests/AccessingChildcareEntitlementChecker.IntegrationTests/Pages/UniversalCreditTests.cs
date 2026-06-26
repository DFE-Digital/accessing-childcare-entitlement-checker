using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class UniversalCreditTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{

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
    public async Task Get_Has_Input_And_BackLink(
        string? returnTo,
        PaidWorkOption? paidWork,
        WorkStatusOption? workStatus,
        SelfEmployedDurationOption? selfEmployedDuration,
        WeeklyEarningsOption? weeklyEarnings,
        YearlyEarningsOption? yearlyEarnings,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PaidWork = paidWork,
            WorkStatus = workStatus.HasValue ? [workStatus.Value] : [],
            SelfEmployedDuration = selfEmployedDuration,
            WeeklyEarnings = weeklyEarnings,
            YearlyEarnings = yearlyEarnings,
        });

        var url = $"/benefits/universal-credit?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(2)
            .AssertBackLink(backLinkUrl);
    }

    [Theory]
    [InlineData(null, UniversalCreditOption.Receives, null, "/benefits/benefits")]
    [InlineData(null, UniversalCreditOption.DoesNotReceive, null, "/benefits/benefits")]
    [InlineData(ReturnTo.CheckAnswers, UniversalCreditOption.Receives, null, "/benefits/benefits")]
    [InlineData(ReturnTo.CheckAnswers, UniversalCreditOption.Receives, BenefitsOption.CarersAllowance, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, UniversalCreditOption.DoesNotReceive, null, "/benefits/benefits")]
    [InlineData(ReturnTo.CheckAnswers, UniversalCreditOption.DoesNotReceive, BenefitsOption.CarersAllowance, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, UniversalCreditOption universalCredit, BenefitsOption? benefits, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            UniversalCredit = universalCredit,
            Benefits = benefits is null ? new() : [benefits.Value],
        });
        var url = $"/benefits/universal-credit?returnTo={returnTo}";
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

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/earnings/adjusted-net-income")]
    [InlineData(null, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.BelowThreshold, null, "/earnings/wage")]
    [InlineData(null, PaidWorkOption.Yes, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.LessThan12Months, null, null, "/work-status/self-employed")]
    [InlineData(null, PaidWorkOption.No, null, null, null, null, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(
        string? returnTo,
        PaidWorkOption? paidWork,
        WorkStatusOption? workStatus,
        SelfEmployedDurationOption? selfEmployedDuration,
        WeeklyEarningsOption? weeklyEarnings,
        YearlyEarningsOption? yearlyEarnings,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PaidWork = paidWork,
            WorkStatus = workStatus.HasValue ? [workStatus.Value] : [],
            SelfEmployedDuration = selfEmployedDuration,
            WeeklyEarnings = weeklyEarnings,
            YearlyEarnings = yearlyEarnings,
        });

        var url = $"/benefits/universal-credit?returnTo={returnTo}";
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
