using Dfe.Acec.Web.Tests.Integration.Fixtures;

namespace Dfe.Acec.Web.Tests.Integration;

public class CacheControlTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task DynamicPagesHaveSecureCacheControlHeaders()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        var cacheControl = response.Headers.CacheControl;
        Assert.NotNull(cacheControl);
        Assert.True(cacheControl.NoCache, "Should have no-cache");
        Assert.True(cacheControl.NoStore, "Should have no-store");
        Assert.True(cacheControl.MustRevalidate, "Should have must-revalidate");
    }

    [Theory]
    [InlineData("/assets/manifest.json")]
    public async Task StaticAssetsHaveLongTermCachingHeaders(string assetUrl)
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync(assetUrl, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        var cacheControl = response.Headers.CacheControl;
        Assert.NotNull(cacheControl);
        Assert.True(cacheControl.Public, "Should be public");
        Assert.Equal(TimeSpan.FromDays(365), cacheControl.MaxAge);

        // "immutable" might be present as an extension in .NET's CacheControlHeaderValue
        var extensions = cacheControl.Extensions;
        Assert.Contains(extensions, e => e.Name == "immutable");
    }
}
