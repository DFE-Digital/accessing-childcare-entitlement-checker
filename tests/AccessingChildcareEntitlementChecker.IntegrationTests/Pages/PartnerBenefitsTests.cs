using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerBenefitsTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/Partner/PartnerBenefits";

    [Theory]
    [InlineData(null, PartnerPaidWorkOption.No, null, null, null, null, "/work-status/work-partner")]
    [InlineData(null, PartnerPaidWorkOption.Yes, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.LessThan12Months, null, null, "/work-status/self-employed-partner")]
    [InlineData(null, PartnerPaidWorkOption.Yes, null, null, WeeklyEarningsOption.BelowThreshold, null, "/earnings/wage-partner")]
    [InlineData(null, PartnerPaidWorkOption.Yes, null, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.AboveThreshold, "/earnings/adjusted-net-income-partner")]
    [InlineData(null, PartnerPaidWorkOption.Yes, null, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/earnings/adjusted-net-income-partner")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.No, null, null, null, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, PartnerPaidWorkOption.No, null, null, null, null, "/children/check-childs-details")]
    public async Task Get(
        string? returnTo,
        PartnerPaidWorkOption? partnerPaidWork,
        WorkStatusOption? partnerWorkStatus,
        SelfEmployedDurationOption? partnerSelfEmployedDuration,
        WeeklyEarningsOption? partnerWeeklyEarnings,
        YearlyEarningsOption? partnerYearlyEarnings,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerPaidWork = partnerPaidWork,
            PartnerWorkStatus = partnerWorkStatus.HasValue ? [partnerWorkStatus.Value] : [],
            PartnerSelfEmployedDuration = partnerSelfEmployedDuration,
            PartnerWeeklyEarnings = partnerWeeklyEarnings,
            PartnerYearlyEarnings = partnerYearlyEarnings,
        });

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertCheckboxCount(9)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, PartnerBenefitsOption.CarersAllowance, null, "/benefits/childcare-support-partner")]
    [InlineData(ReturnTo.CheckAnswers, PartnerBenefitsOption.CarersAllowance, null, "/benefits/childcare-support-partner")]
    [InlineData(ReturnTo.CheckAnswers, PartnerBenefitsOption.CarersAllowance, PartnerChildcareSupportOption.ChildcareVouchers, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, PartnerBenefitsOption partnerBenefits, PartnerChildcareSupportOption? partnerChildcareSupport, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerBenefits = [partnerBenefits],
            PartnerChildcareSupport = partnerChildcareSupport is null ? new() : [partnerChildcareSupport.Value],
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
            new KeyValuePair<string, string>("PartnerBenefits", partnerBenefits.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, PartnerPaidWorkOption.No, null, null, null, null, "/work-status/work-partner")]
    [InlineData(null, PartnerPaidWorkOption.Yes, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.LessThan12Months, null, null, "/work-status/self-employed-partner")]
    [InlineData(null, PartnerPaidWorkOption.Yes, null, null, WeeklyEarningsOption.BelowThreshold, null, "/earnings/wage-partner")]
    [InlineData(null, PartnerPaidWorkOption.Yes, null, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.AboveThreshold, "/earnings/adjusted-net-income-partner")]
    [InlineData(null, PartnerPaidWorkOption.Yes, null, null, WeeklyEarningsOption.AboveThreshold, YearlyEarningsOption.BelowThreshold, "/earnings/adjusted-net-income-partner")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.No, null, null, null, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, PartnerPaidWorkOption.No, null, null, null, null, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(
        string? returnTo,
        PartnerPaidWorkOption? partnerPaidWork,
        WorkStatusOption? partnerWorkStatus,
        SelfEmployedDurationOption? partnerSelfEmployedDuration,
        WeeklyEarningsOption? partnerWeeklyEarnings,
        YearlyEarningsOption? partnerYearlyEarnings,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerPaidWork = partnerPaidWork,
            PartnerWorkStatus = partnerWorkStatus.HasValue ? [partnerWorkStatus.Value] : [],
            PartnerSelfEmployedDuration = partnerSelfEmployedDuration,
            PartnerWeeklyEarnings = partnerWeeklyEarnings,
            PartnerYearlyEarnings = partnerYearlyEarnings,
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
