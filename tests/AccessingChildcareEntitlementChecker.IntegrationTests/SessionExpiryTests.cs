using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace AccessingChildcareEntitlementChecker.IntegrationTests;

public partial class SessionExpiryTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task GetWithoutSessionRedirectsToExpiry()
    {
        var sessionRequiredEndpoints = GetSessionRequiredEndpoints("GET");
        foreach (var url in sessionRequiredEndpoints)
        {
            using var client = factory.CreateClientWithoutJourneySession();
            var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
            getResponse.AssertRedirect("/session-expired");
        }
    }

    [Fact]
    public async Task PostWithoutSessionRedirectsToExpiry()
    {
        var sessionRequiredEndpoints = GetSessionRequiredEndpoints("POST");
        var children = new Dictionary<string, Child>
               {
                   {
                       "1",
                       new Child("1", "Child 1")
                   }
               };

        foreach (var url in sessionRequiredEndpoints)
        {
            // Resource filter will fire after auth filters, which will check antiforgery.
            // So we need to run a valid GET first to obtain the token, and `WeeklyEarnings` checks
            // prerequisites during construction; which is a pain.
            using var getClient = factory.CreateClientWithJourneyState(new JourneyState
            {
                Children = children,
                UserAge = AgeRange.UnderEighteen,
                WorkStatus = [WorkStatusOption.PaidEmployment],
                PartnerAge = AgeRange.UnderEighteen,
                PartnerWorkStatus = [WorkStatusOption.PaidEmployment],
            });

            var getResponse = await getClient.GetAsync(url, TestContext.Current.CancellationToken);
            getResponse.EnsureSuccessStatusCode();
            var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
            var title = getDocument.Title;
            var content = getDocument.TextContent;
            var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
            var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
            Assert.NotNull(token);
            Assert.NotNull(cookie);

            // Now we simulate the user timing out while sat on a page,
            // and then submitting the form.
            using var postClient = factory.CreateClientWithoutJourneySession();
            var postResponse = await HttpClientHelpers.PostFormAsync(
                postClient,
                url,
                cookie,
                token,
                [],
                TestContext.Current.CancellationToken);
            postResponse.AssertRedirect("/session-expired");
        }
    }

    private IEnumerable<string> GetSessionRequiredEndpoints(string verb)
    {
        var endpointDataSource = factory.Services.GetRequiredService<EndpointDataSource>();
        var endpoints = endpointDataSource.Endpoints.OfType<RouteEndpoint>();
        var publicRoutes = new[]
        {
            "/",
            "/session-expired",
            "/privacy-notice",
            "/accessibility-statement",
            "/cookies",
            "/throw",
            "/robots.txt",
            "/{controller=Home}/{action=Start}/{id?}",
            "/start-page",
            "/where-do-you-live"
        };

        return endpoints
            .Where(e => e.Metadata.OfType<HttpMethodMetadata>().Any(m => m.HttpMethods.Contains(verb)))
            .Select(e => e.RoutePattern.RawText)
            .Where(route => route != null)
            .Select(route => "/" + route!.TrimStart('/'))
            .Except(publicRoutes, StringComparer.OrdinalIgnoreCase)
            .Select(MaterializeRoute)
            .Distinct();
    }

    /// <summary>
    /// Converts a route pattern with parameters into a concrete route by replacing parameters with a placeholder value.
    /// </summary>
    /// <param name="routePattern">The route pattern to convert.</param>
    /// <returns>A concrete route with placeholder values.</returns>
    private static string MaterializeRoute(string routePattern)
    {
        return RouteParameterRegex().Replace(routePattern, "1");
    }

    [GeneratedRegex(@"\{[^}]+\}")]
    private static partial Regex RouteParameterRegex();
}
