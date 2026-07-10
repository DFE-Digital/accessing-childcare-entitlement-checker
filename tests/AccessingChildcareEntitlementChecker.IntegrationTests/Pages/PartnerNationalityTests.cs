using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerNationalityTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/nationality/nationality-partner";

    [Theory]
    [InlineData(null, "/age/partner-age")]
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
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, null, null, "/nationality/settled-status-partner")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, null, null, "/work-status/work-partner")]
    [InlineData(null, NationalityOption.CitizenOfADifferentCountry, null, null, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, null, null, "/nationality/settled-status-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.Yes, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.BritishOrIrishCitizen, null, null, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.BritishOrIrishCitizen, null, PartnerPaidWorkOption.Yes, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfADifferentCountry, null, null, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfADifferentCountry, null, PartnerPaidWorkOption.Yes, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, NationalityOption partnerNationality, SettledStatusOption? partnerSettledStatus, PartnerPaidWorkOption? partnerPaidWork, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerNationality = partnerNationality,
            PartnerSettledStatus = partnerSettledStatus,
            PartnerPaidWork = partnerPaidWork,
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
            new KeyValuePair<string, string>("PartnerNationality", partnerNationality.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/age/partner-age")]
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
