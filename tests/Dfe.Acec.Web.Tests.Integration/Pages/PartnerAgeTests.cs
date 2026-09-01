using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class PartnerAgeTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/age/partner-age";

    [Theory]
    [InlineData(null, "/partner")]
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
    [InlineData(NationalityOption.BritishOrIrishCitizen, null, "/work-status/work-partner")]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, null, "/nationality-partner")]
    [InlineData(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, SettledStatusOption.Yes, "/work-status/work-partner")]
    [InlineData(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, SettledStatusOption.StillWaiting, "/nationality-partner")]
    [InlineData(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, SettledStatusOption.No, "/nationality-partner")]
    public async Task PostValidRedirects(
        NationalityOption userNationality,
        SettledStatusOption? userSettledStatusOption,
        string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = userNationality,
            SettledStatus = userSettledStatusOption,
        });

        using var client = host.CreateClient();

        var getResponse = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, Url, cookie, token, [
            new KeyValuePair<string, string>("PartnerAge", nameof(AgeRange.EighteenToTwenty))
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/partner")]
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
