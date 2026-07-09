using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerSelfEmployedDurationTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(null, "/work-status/work-status-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"/work-status/self-employed-partner?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, SelfEmployedDurationOption.LessThan12Months, null, null, "/Partner/PartnerBenefits")]
    [InlineData(null, SelfEmployedDurationOption.NotLessThan12Months, null, null, "/earnings/wage-partner")]
    [InlineData(ReturnTo.CheckAnswers, SelfEmployedDurationOption.LessThan12Months, null, null, "/Partner/PartnerBenefits")]
    [InlineData(ReturnTo.CheckAnswers, SelfEmployedDurationOption.LessThan12Months, PartnerBenefitsOption.CarersAllowance, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, SelfEmployedDurationOption.NotLessThan12Months, null, null, "/earnings/wage-partner")]
    [InlineData(ReturnTo.CheckAnswers, SelfEmployedDurationOption.NotLessThan12Months, null, WeeklyEarningsOption.AboveThreshold, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, SelfEmployedDurationOption partnerSelfEmployedDuration, PartnerBenefitsOption? partnerBenefits, WeeklyEarningsOption? partnerWeeklyEarnings, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerSelfEmployedDuration = partnerSelfEmployedDuration,
            PartnerBenefits = partnerBenefits is null ? new() : [partnerBenefits.Value],
            PartnerWeeklyEarnings = partnerWeeklyEarnings,
        });
        var url = $"/work-status/self-employed-partner?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("PartnerSelfEmployedDuration", partnerSelfEmployedDuration.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/work-status/work-status-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"/work-status/self-employed-partner?returnTo={returnTo}";
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
