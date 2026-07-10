using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class UserAgeTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/age/parent-age";

    [Theory]
    [InlineData(null, "/children/check-childs-details")]
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
    [InlineData(null, null, null, null, null, AgeRange.EighteenToTwenty, "/nationality")]
    [InlineData(ReturnTo.CheckAnswers, null, null, null, null, AgeRange.EighteenToTwenty, "/nationality")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, null, WeeklyEarningsOption.AboveThreshold, AgeRange.EighteenToTwenty, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, null, null, AgeRange.EighteenToTwenty, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, null, WeeklyEarningsOption.AboveThreshold, AgeRange.TwentyOneOrOver, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PaidWorkOption.Yes, null, AgeRange.EighteenToTwenty, "/earnings/wage")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PaidWorkOption.No, null, AgeRange.EighteenToTwenty, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PaidWorkOption.Yes, WeeklyEarningsOption.AboveThreshold, AgeRange.EighteenToTwenty, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PaidWorkOption.Yes, WeeklyEarningsOption.AboveThreshold, AgeRange.TwentyOneOrOver, "/earnings/wage")]
    public async Task Post_Valid_Redirects(
        string? returnTo,
        AgeRange? oldUserAge,
        NationalityOption? nationality,
        PaidWorkOption? paidWork,
        WeeklyEarningsOption? weeklyEarnings,
        AgeRange newUserAge,
        string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            UserAge = oldUserAge,
            Nationality = nationality,
            PaidWork = paidWork,
            WeeklyEarnings = weeklyEarnings,
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
                new KeyValuePair<string,string>("UserAge", newUserAge.ToString())
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/children/check-childs-details")]
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
