using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
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

    [Fact]
    public async Task LayoutIncludesRequiredFooterLinks()
    {
        var routesWithoutFooter = new[]
        {
            "/throw",
            "/robots.txt",
        };

        var routes = RouteHelper.GetEndpointsExcept(factory, "GET", routesWithoutFooter);
        foreach (var url in routes)
        {
            using var getClient = factory.CreateClientWithJourneyState(new JourneyState
            {
                Children = new Dictionary<string, Child>
                {
                   {
                       "1",
                       new Child("1", "Child 1")
                   }
                },
            });

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var getResponse = await getClient.SendAsync(request, TestContext.Current.CancellationToken);
            var document = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
            document
                .AssertFooterContainsLink("https://accessibility-statements.education.gov.uk/s/89") // accessibility statement
                .AssertFooterContainsLink("https://www.gov.uk/government/publications/privacy-information-members-of-the-public/privacy-information-members-of-the-public#using-your-data-when-you-use-our-websites") // privacy notice
                .AssertFooterContainsLink("/cookies") // cookies
                .AssertFooterContainsLink("https://dferesearch.fra1.qualtrics.com/jfe/form/SV_5doFVpOqJt1dD7g"); // contact us & feedback form
        }
    }
}
