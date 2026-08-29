using System.Net;
using Dfe.Acec.Tests.Integration.Fixtures;
using Dfe.Acec.Tests.Integration.Helpers;

namespace Dfe.Acec.Tests.Integration.Pages;

public class NotFoundTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/this-page-does-not-exist";

    [Fact]
    public async Task Get()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertNavigationBar()
            .AssertBetaBanner()
            .AssertMainContainsLink("/")
            .AssertHeading("Page not found");
    }
}
