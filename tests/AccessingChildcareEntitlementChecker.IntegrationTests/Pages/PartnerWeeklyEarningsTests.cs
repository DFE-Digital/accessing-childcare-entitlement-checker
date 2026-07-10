using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerWeeklyEarningsTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/earnings/wage-partner";

    [Theory]
    [InlineData(null, WorkStatusOption.PaidEmployment, "/work-status/work-status-partner")]
    [InlineData(null, WorkStatusOption.SelfEmployed, "/work-status/self-employed-partner")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, WorkStatusOption.PaidEmployment, "/children/check-childs-details")]
    public async Task Get(
        string? returnTo,
        WorkStatusOption partnerWorkStatus,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerAge = AgeRange.TwentyOneOrOver,
            PartnerWorkStatus = [partnerWorkStatus],
        });

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, WeeklyEarningsOption.AboveThreshold, null, null, "/earnings/adjusted-net-income-partner")]
    [InlineData(null, WeeklyEarningsOption.BelowThreshold, null, null, "/Partner/PartnerBenefits")]
    [InlineData(ReturnTo.CheckAnswers, WeeklyEarningsOption.AboveThreshold, null, null, "/earnings/adjusted-net-income-partner")]
    [InlineData(ReturnTo.CheckAnswers, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.AboveThreshold, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, WeeklyEarningsOption.BelowThreshold, null, null, "/Partner/PartnerBenefits")]
    [InlineData(ReturnTo.CheckAnswers, WeeklyEarningsOption.BelowThreshold, null, PartnerBenefitsOption.CarersAllowance, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, WeeklyEarningsOption partnerWeeklyEarnings, YearlyEarningsOption? partnerYearlyEarnings, PartnerBenefitsOption? partnerBenefits, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerAge = AgeRange.TwentyOneOrOver,
            PartnerWorkStatus = [WorkStatusOption.PaidEmployment],
            PartnerWeeklyEarnings = partnerWeeklyEarnings,
            PartnerYearlyEarnings = partnerYearlyEarnings,
            PartnerBenefits = partnerBenefits is null ? new() : [partnerBenefits.Value],
        });
        var url = $"{Url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("PartnerWeeklyEarnings", partnerWeeklyEarnings.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, WorkStatusOption.PaidEmployment, "/work-status/work-status-partner")]
    [InlineData(null, WorkStatusOption.SelfEmployed, "/work-status/self-employed-partner")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, WorkStatusOption.PaidEmployment, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(
        string? returnTo,
        WorkStatusOption partnerWorkStatus,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerAge = AgeRange.TwentyOneOrOver,
            PartnerWorkStatus = [partnerWorkStatus],
        });

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
