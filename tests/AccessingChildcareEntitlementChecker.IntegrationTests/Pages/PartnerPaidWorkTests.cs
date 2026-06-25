using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerPaidWorkTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(null, null, null, null, "/nationality/nationality-partner")]
    [InlineData(null, null, null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "/nationality/settled-status-partner")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, null, null, "/age/partner-age")]
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.Yes, null, "/age/partner-age")]
    [InlineData(ReturnTo.CheckAnswers, null, null, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, null, null, null, "/children/check-childs-details")]
    public async Task Get_Has_Input_And_BackLink(
        string? returnTo,
        NationalityOption? nationality,
        SettledStatusOption? settledStatus,
        NationalityOption? partnerNationality,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
            SettledStatus = settledStatus,
            PartnerNationality = partnerNationality,
        });

        var url = $"/work-status/work-partner?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(3)
            .AssertBackLink(backLinkUrl);
    }

    [Theory]
    [InlineData(null, null, null, null, "/nationality/nationality-partner")]
    [InlineData(null, null, null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "/nationality/settled-status-partner")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, null, null, "/age/partner-age")]
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.Yes, null, "/age/partner-age")]
    [InlineData(ReturnTo.CheckAnswers, null, null, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, null, null, null, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(
        string? returnTo,
        NationalityOption? nationality,
        SettledStatusOption? settledStatus,
        NationalityOption? partnerNationality,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
            SettledStatus = settledStatus,
            PartnerNationality = partnerNationality,
        });

        var url = $"/work-status/work-partner?returnTo={returnTo}";
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
