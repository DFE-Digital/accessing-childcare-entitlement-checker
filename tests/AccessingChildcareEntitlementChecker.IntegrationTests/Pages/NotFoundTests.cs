using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using System.Net;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

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
