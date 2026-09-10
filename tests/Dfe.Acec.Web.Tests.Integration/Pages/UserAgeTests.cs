using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class UserAgeTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/age/parent-age";

    [Theory]
    [InlineData(null, "/children/check-childs-details")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

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
    [InlineData(null, null, null, null, null, AgeRange.EighteenToTwenty)]
    [InlineData(ReturnTo.CheckAnswers, null, null, null, null, AgeRange.EighteenToTwenty)]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, null, WeeklyEarningsOption.AboveThreshold, AgeRange.EighteenToTwenty)]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, null, null, AgeRange.EighteenToTwenty)]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, null, WeeklyEarningsOption.AboveThreshold, AgeRange.TwentyOneOrOver)]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PaidWorkOption.Yes, null, AgeRange.EighteenToTwenty)]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PaidWorkOption.No, null, AgeRange.EighteenToTwenty)]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PaidWorkOption.Yes, WeeklyEarningsOption.AboveThreshold, AgeRange.EighteenToTwenty)]
    [InlineData(ReturnTo.CheckAnswers, AgeRange.EighteenToTwenty, NationalityOption.BritishOrIrishCitizen, PaidWorkOption.Yes, WeeklyEarningsOption.AboveThreshold, AgeRange.TwentyOneOrOver)]
    public async Task PostValidRedirects(
        string? returnTo,
        AgeRange? oldUserAge,
        NationalityOption? nationality,
        PaidWorkOption? paidWork,
        WeeklyEarningsOption? weeklyEarnings,
        AgeRange newUserAge)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            UserAge = oldUserAge,
            NationalityOptions = nationality == null ? [] : [nationality.Value],
            PaidWork = paidWork,
            WeeklyEarnings = weeklyEarnings,
        });

        using var client = host.CreateClient();

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
        postResponse.AssertRedirect("/nationality");
    }

    [Theory]
    [InlineData(null, "/children/check-childs-details")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task PostInvalidShowsValidationError(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

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
