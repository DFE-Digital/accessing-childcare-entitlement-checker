using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration;

public class SessionTimeoutDialogTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task DialogIsRenderedWithConfiguredTimingsWhenSessionExists()
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());
        using var client = host.CreateClient();

        var response = await client.GetAsync("/where-do-you-live", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var document = await HtmlHelpers.ParseHtmlAsync(response.Content);

        var dialog = document.QuerySelector("dialog#session-timeout-dialog");
        Assert.NotNull(dialog);
        Assert.Equal("30", dialog.GetAttribute("data-timeout-minutes"));
        Assert.Equal("2", dialog.GetAttribute("data-warning-minutes"));
        Assert.Equal("/session-keep-alive", dialog.GetAttribute("data-keep-alive-url"));
        Assert.Equal("/session-expired", dialog.GetAttribute("data-expired-url"));
        Assert.Contains(document.QuerySelectorAll("script"), s => s.GetAttribute("src")?.Contains("/js/session-timeout.js") == true);
    }

    [Fact]
    public async Task DialogIsNotRenderedWhenNoSessionExists()
    {
        await using var host = factory.CreateClientWithoutJourneySession();
        using var client = host.CreateClient();

        var response = await client.GetAsync("/where-do-you-live", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var document = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Null(document.QuerySelector("dialog#session-timeout-dialog"));
    }

    [Fact]
    public async Task KeepAliveReturnsNoContentWhenSessionExists()
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());
        using var client = host.CreateClient();
        var (token, cookie) = await GetAntiforgeryAsync(client);

        var response = await HttpClientHelpers.PostFormAsync(
            client, "/session-keep-alive", cookie, token, [], TestContext.Current.CancellationToken);

        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task KeepAliveReturnsUnauthorizedWhenSessionHasExpired()
    {
        await using var getHost = factory.CreateClientWithJourneyState(new JourneyState());
        using var getClient = getHost.CreateClient();
        var (token, cookie) = await GetAntiforgeryAsync(getClient);

        await using var postHost = factory.CreateClientWithoutJourneySession();
        using var postClient = postHost.CreateClient();
        var response = await HttpClientHelpers.PostFormAsync(
            postClient, "/session-keep-alive", cookie, token, [], TestContext.Current.CancellationToken);

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private static async Task<(string Token, string Cookie)> GetAntiforgeryAsync(HttpClient client)
    {
        var response = await client.GetAsync("/where-do-you-live", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var document = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(document);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(response);
        Assert.NotNull(token);
        Assert.NotNull(cookie);
        return (token, cookie);
    }
}
