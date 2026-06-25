using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PaidWorkTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(null, NationalityOption.CitizenOfADifferentCountry, "/nationality")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, "/nationality")]
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "/nationality/settled-status")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfADifferentCountry, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, NationalityOption.CitizenOfADifferentCountry, "/children/check-childs-details")]
    public async Task Get_Has_Input_And_BackLink(string? returnTo, NationalityOption? nationality, string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
        });

        var url = $"/work-status/work?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(3)
            .AssertBackLink(backLinkUrl);
    }

    [Theory]
    [InlineData(null, PaidWorkOption.Yes, null, null, "/work-status/work-status")]
    [InlineData(null, PaidWorkOption.No, null, null, "/benefits/universal-credit")]
    [InlineData(null, PaidWorkOption.OnLeave, null, null, "/leave/type-of-leave")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.Yes, null, null, "/work-status/work-status")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.Yes, WorkStatusOption.PaidEmployment, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.No, null, null, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.No, null, UniversalCreditOption.Receives, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.OnLeave, null, null, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, PaidWorkOption paidWork, WorkStatusOption? workStatus, UniversalCreditOption? universalCredit, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PaidWork = paidWork,
            WorkStatus = workStatus is null ? new() : [workStatus.Value],
            UniversalCredit = universalCredit,
        });
        var url = $"/work-status/work?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("PaidWork", paidWork.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, NationalityOption.CitizenOfADifferentCountry, "/nationality")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, "/nationality")]
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "/nationality/settled-status")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfADifferentCountry, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, NationalityOption.CitizenOfADifferentCountry, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, NationalityOption? nationality, string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
        });

        var url = $"/work-status/work?returnTo={returnTo}";
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

    [Fact]
    public async Task Post_OnLeave_Redirects_To_TypeOfLeave()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var url = "/work-status/work";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
                new KeyValuePair<string,string>("PaidWork", "OnLeave")
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect("/leave/type-of-leave");
    }
}
