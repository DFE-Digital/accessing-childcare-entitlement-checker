using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerAgeTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/age/partner-age";

    [Theory]
    [InlineData(null, "/partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(3)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, null, null, null, null, AgeRange.EighteenToTwenty, "/nationality-partner")]
    [InlineData(ReturnTo.CheckAnswers, null, null, null, null, AgeRange.EighteenToTwenty, "/nationality-partner")]
    [InlineData(ReturnTo.CheckAnswers, null, NationalityOption.BritishOrIrishCitizen, null, null, AgeRange.EighteenToTwenty, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, null, null, AgeRange.EighteenToTwenty, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, null, WeeklyEarningsOption.AboveThreshold, AgeRange.TwentyOneOrOver, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PartnerPaidWorkOption.Yes, null, AgeRange.EighteenToTwenty, "/earnings/wage-partner")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PartnerPaidWorkOption.No, null, AgeRange.EighteenToTwenty, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PartnerPaidWorkOption.Yes, WeeklyEarningsOption.AboveThreshold, AgeRange.TwentyOneOrOver, "/earnings/wage-partner")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PartnerPaidWorkOption.Yes, WeeklyEarningsOption.AboveThreshold, AgeRange.EighteenToTwenty, "/check-your-answers")]
    public async Task Post_Valid_Redirects(
        string? returnTo,
        AgeRange? oldPartnerAge,
        NationalityOption? partnerNationality,
        PartnerPaidWorkOption? partnerPaidWorkOption,
        WeeklyEarningsOption? partnerWeeklyEarnings,
        AgeRange newPartnerAge,
        string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerAge = oldPartnerAge,
            PartnerNationality = partnerNationality,
            PartnerPaidWork = partnerPaidWorkOption,
            PartnerWeeklyEarnings = partnerWeeklyEarnings,
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
            new KeyValuePair<string, string>("PartnerAge", newPartnerAge.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

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
