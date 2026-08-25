using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
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
        using var host = factory.CreateClientWithJourneyState(new JourneyState());

        var client = host.CreateClient();

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
    [InlineData(NationalityOption.BritishOrIrishCitizen, null, "/work-status/work-partner")]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, null, "/nationality-partner")]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.Yes, "/work-status/work-partner")]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.StillWaiting, "/nationality-partner")]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.No, "/nationality-partner")]
    public async Task Post_Valid_Redirects(
        NationalityOption userNationality,
        SettledStatusOption? userSettledStatusOption,
        string continueUrl)
    {
        using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = userNationality,
            SettledStatus = userSettledStatusOption,
        });

        var client = host.CreateClient();

        var getResponse = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, Url, cookie, token, [
            new KeyValuePair<string, string>("PartnerAge", AgeRange.EighteenToTwenty.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, string backLinkUrl)
    {
        using var host = factory.CreateClientWithJourneyState(new JourneyState());

        var client = host.CreateClient();

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
