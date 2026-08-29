using Dfe.Acec.Tests.Integration.Fixtures;
using Dfe.Acec.Tests.Integration.Helpers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Tests.Integration.Pages;

public class PartnerNationalityTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/nationality/nationality-partner";

    [Theory]
    [InlineData(null, "/age/partner-age")]
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
    [InlineData(null, NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, null, null, "/nationality/settled-status-partner")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, null, null, "/work-status/work-partner")]
    [InlineData(null, NationalityOption.CitizenOfADifferentCountry, null, null, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, null, null, "/nationality/settled-status-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, SettledStatusOption.Yes, null, "/nationality/settled-status-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.BritishOrIrishCitizen, null, null, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.BritishOrIrishCitizen, null, PartnerPaidWorkOption.Yes, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfADifferentCountry, null, null, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfADifferentCountry, null, PartnerPaidWorkOption.Yes, "/work-status/work-partner")]
    public async Task PostValidRedirects(string? returnTo, NationalityOption partnerNationality, SettledStatusOption? partnerSettledStatus, PartnerPaidWorkOption? partnerPaidWork, string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerNationality = partnerNationality,
            PartnerSettledStatus = partnerSettledStatus,
            PartnerPaidWork = partnerPaidWork,
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
            new KeyValuePair<string, string>("PartnerNationality", partnerNationality.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/age/partner-age")]
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
