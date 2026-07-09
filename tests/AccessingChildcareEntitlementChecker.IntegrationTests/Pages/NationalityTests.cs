using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class NationalityTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(null, "/age/parent-age")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"/nationality?returnTo={returnTo}";
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
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, null, null, "/nationality/settled-status")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.BritishOrIrishCitizen, null, PaidWorkOption.Yes, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.BritishOrIrishCitizen, null, null, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, null, null, "/nationality/settled-status")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.Yes, PaidWorkOption.Yes, "/check-your-answers")]
    public async Task Post_Valid_Redirects(
        string? returnTo,
        NationalityOption nationality,
        SettledStatusOption? settledStatus,
        PaidWorkOption? paidWork,
        string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
            SettledStatus = settledStatus,
            PaidWork = paidWork
        });

        var url = $"/nationality?returnTo={returnTo}";
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
    public async Task Post_EU_Redirects_To_SettledStatus()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var url = "/nationality";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
                new KeyValuePair<string,string>("Nationality", "CitizenOfAnEUCountryEEACountryOrSwitzerland")
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect("/nationality/settled-status");
    }

    [Theory]
    [InlineData(null, "/age/parent-age")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"/nationality?returnTo={returnTo}";
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
