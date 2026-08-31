using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class NationalityTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _url = "/nationality";

    [Theory]
    [InlineData(null, "/age/parent-age")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

        var url = $"{_url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(3)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, null, null, "/work-status/work")]
    [InlineData(null, NationalityOption.CitizenOfADifferentCountry, null, null, "/work-status/work")]
    [InlineData(null, NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, null, null, "/nationality/settled-status")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.BritishOrIrishCitizen, null, PaidWorkOption.Yes, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.BritishOrIrishCitizen, null, null, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, null, null, "/nationality/settled-status")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, SettledStatusOption.Yes, PaidWorkOption.Yes, "/nationality/settled-status")]
    public async Task PostValidRedirects(
        string? returnTo,
        NationalityOption nationality,
        SettledStatusOption? settledStatus,
        PaidWorkOption? paidWork,
        string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
            SettledStatus = settledStatus,
            PaidWork = paidWork
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
                new KeyValuePair<string,string>("Nationality", nationality.ToString())
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect(continueUrl);
    }

    [Fact]
    public async Task PostEuRedirectsToSettledStatus()
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

        var getResponse = await client.GetAsync(_url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, _url, cookie, token, [
                new KeyValuePair<string,string>("Nationality", "CitizenOfAnEuCountryEeaCountryOrSwitzerland")
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect("/nationality/settled-status");
    }

    [Theory]
    [InlineData(null, "/age/parent-age")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task PostInvalidShowsValidationError(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

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
