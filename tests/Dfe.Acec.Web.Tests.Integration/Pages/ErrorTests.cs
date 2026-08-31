using System.Net;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class ErrorTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _url = "/error";

    [Fact]
    public async Task Get()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync(_url, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertNavigationBar()
            .AssertBetaBanner()
            .AssertMainContainsLink("https://www.gov.uk/get-childcare");
    }
}
