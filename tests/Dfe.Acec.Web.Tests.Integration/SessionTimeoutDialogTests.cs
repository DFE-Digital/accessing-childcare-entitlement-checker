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
    public async Task KeepAliveReturnsGoneWithoutErrorPageWhenSessionHasExpired()
    {
        await using var getHost = factory.CreateClientWithJourneyState(new JourneyState());
        using var getClient = getHost.CreateClient();
        var (token, cookie) = await GetAntiforgeryAsync(getClient);

        await using var postHost = factory.CreateClientWithoutJourneySession();
        using var postClient = postHost.CreateClient();
        var response = await HttpClientHelpers.PostFormAsync(
            postClient, "/session-keep-alive", cookie, token, [], TestContext.Current.CancellationToken);

        Assert.Equal(System.Net.HttpStatusCode.Gone, response.StatusCode);
        Assert.Empty(await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task KeepAliveExtendsServerSession()
    {
        var clock = new FakeClock();
        await using var host = factory.CreateClientWithSessionClock(clock);
        using var client = host.CreateClient();
        var token = await StartSessionAsync(client);

        clock.Advance(TimeSpan.FromMinutes(20));
        Assert.Equal(System.Net.HttpStatusCode.NoContent, (await KeepAliveAsync(client, token)).StatusCode);

        clock.Advance(TimeSpan.FromMinutes(20));
        Assert.Equal(System.Net.HttpStatusCode.NoContent, (await KeepAliveAsync(client, token)).StatusCode);
    }

    [Fact]
    public async Task ServerSessionExpiresWithoutKeepAlive()
    {
        var clock = new FakeClock();
        await using var host = factory.CreateClientWithSessionClock(clock);
        using var client = host.CreateClient();
        var token = await StartSessionAsync(client);

        clock.Advance(TimeSpan.FromMinutes(31));

        Assert.Equal(System.Net.HttpStatusCode.Gone, (await KeepAliveAsync(client, token)).StatusCode);
    }

    [Fact]
    public async Task SessionExpiredPageDeletesAnswers()
    {
        await using var host = factory.CreateClientWithSessionClock(new FakeClock());
        using var client = host.CreateClient();
        await StartSessionAsync(client);

        var expiredResponse = await client.GetAsync("/session-expired", TestContext.Current.CancellationToken);
        expiredResponse.EnsureSuccessStatusCode();
        var document = await HtmlHelpers.ParseHtmlAsync(expiredResponse.Content);
        Assert.Null(document.QuerySelector("dialog#session-timeout-dialog"));

        var journeyResponse = await client.GetAsync("/children/add-child-details", TestContext.Current.CancellationToken);
        journeyResponse.AssertRedirect("/session-expired");
    }

    private static async Task<string> StartSessionAsync(HttpClient client)
    {
        var (token, _) = await GetAntiforgeryAsync(client);
        var response = await HttpClientHelpers.PostFormAsync(
            client, "/where-do-you-live", null, token, [new("Country", "England")], TestContext.Current.CancellationToken);
        response.AssertRedirect("/children/add-child-details");
        return token;
    }

    private static Task<HttpResponseMessage> KeepAliveAsync(HttpClient client, string token)
    {
        return HttpClientHelpers.PostFormAsync(
            client, "/session-keep-alive", null, token, [], TestContext.Current.CancellationToken);
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
