using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class CookiesTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/cookies";

    [Fact]
    public async Task Get()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertHeading("Cookies");
    }

    [Fact]
    public async Task Get_With_Malformed_Parameter_Returns_Bad_Request()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync($"{Url}?hasSetCookies=banana", TestContext.Current.CancellationToken);
        response.AssertBadRequest();
    }

    [Theory]
    [InlineData(true, "enabled")]
    [InlineData(false, "disabled")]
    public async Task Post_Sets_Cookie(bool analyticsCookiesEnabled, string expectedCookieValue)
    {
        using var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        var getResponse = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, Url, cookie, token, [
            new KeyValuePair<string, string>("AnalyticsCookiesEnabled", analyticsCookiesEnabled.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse
            .AssertRedirect("/cookies")
            .AssertCookie("cookie_policy", expectedCookieValue);
    }

    [Theory]
    [InlineData(true, "enabled")]
    [InlineData(false, "disabled")]
    public async Task Banner_Consent_Sets_Cookie_Or_Returns_Bad_Request(bool analyticsCookiesEnabled, string expectedCookieValue)
    {
        using var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        var getResponse = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        // N.b. exceptional in these tests - checking a different endpoint.
        var postResponse = await HttpClientHelpers.PostFormAsync(client, "/cookies/bannerconsent", cookie, token, [
            new KeyValuePair<string, string>("AnalyticsCookiesEnabled", analyticsCookiesEnabled.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse
            .AssertNoContent()
            .AssertCookie("cookie_policy", expectedCookieValue);
    }
}
