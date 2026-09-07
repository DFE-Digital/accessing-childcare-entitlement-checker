using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Dfe.Acec.Web.Tests.Unit.Component;

public class ComponentTests
{
    private sealed class CustomWebApplicationFactory(string environmentName) : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(environmentName);
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["DevelopmentBasicAuthPassword"] = "dev-only"
                });
            });
        }
    }

    private static WebApplicationFactory<Program> CreateFactory(string environmentName) =>
        new CustomWebApplicationFactory(environmentName);

    [Fact]
    public async Task GetRootReturnsSuccess()
    {
        await using var factory = CreateFactory("Production");
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);

        // Checking the status code is sufficient to know that the DI is all hooked up properly.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetRootRequiresBasicAuthInDevelopment()
    {
        await using var factory = CreateFactory("Development");
        var client = factory.CreateClient();

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Contains("Basic", response.Headers.WwwAuthenticate.ToString());
    }

    [Fact]
    public async Task GetRootSucceedsWithBasicAuthInDevelopment()
    {
        await using var factory = CreateFactory("Development");
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String("user:dev-only"u8.ToArray()));

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetRobotsTxtReturnsNoIndexInstructionsInDevelopment()
    {
        await using var factory = CreateFactory("Development");
        var client = factory.CreateClient();

        var response = await client.GetAsync("/robots.txt", TestContext.Current.CancellationToken);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal("User-agent: *\nDisallow: /", body);
    }

    [Fact]
    public async Task GetRobotsTxtReturnsNotFoundOutsideDevelopment()
    {
        await using var factory = CreateFactory("Production");
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        var response = await client.GetAsync("/robots.txt", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetHealthCheckDoesNotRequireBasicAuthInDevelopment()
    {
        await using var factory = CreateFactory("Development");
        var client = factory.CreateClient();

        var response = await client.GetAsync("/health", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
