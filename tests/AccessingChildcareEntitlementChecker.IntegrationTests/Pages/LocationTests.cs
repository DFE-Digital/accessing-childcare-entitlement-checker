using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class LocationTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(null, "/")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get_Has_Input_And_BackLink(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"/where-do-you-live?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(4)
            .AssertBackLink(backLinkUrl);
    }

    [Theory]
    [InlineData(null, false, "/children/add-child-details")]
    [InlineData(ReturnTo.CheckAnswers, false, "/children/add-child-details")]
    [InlineData(ReturnTo.CheckAnswers, true, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, bool hasChild, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = hasChild
                ? new Dictionary<string, Child> { { "child1", new Child("child1", "Child 1") } }
                : new Dictionary<string, Child>()
        });

        var url = $"/where-do-you-live?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
                new KeyValuePair<string,string>("Country", "England")
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();
        var url = $"/where-do-you-live?returnTo={returnTo}";
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
