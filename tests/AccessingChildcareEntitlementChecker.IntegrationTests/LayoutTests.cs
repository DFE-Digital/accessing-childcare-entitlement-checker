using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AngleSharp.Html.Dom;

namespace AccessingChildcareEntitlementChecker.IntegrationTests;

public class LayoutTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task LayoutIncludesGovUkFrontendScripts()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync("/cookies", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        var document = await HtmlHelpers.ParseHtmlAsync(response.Content);

        // Assert that the layout has the GOV.UK Frontend script imported
        var scripts = document.QuerySelectorAll("script");
        var hasGovUkScript = scripts.Any(s => s.GetAttribute("src")?.Contains("govuk-frontend") == true);

        Assert.True(hasGovUkScript, "The layout should import GOV.UK Frontend script in BodyEnd.");
    }
}
