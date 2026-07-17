using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;

internal static partial class RouteHelper
{
    public static IEnumerable<string> GetEndpointsExcept(IntegrationTestFixture factory, string verb, IEnumerable<string> exceptRoutes)
    {
        var endpointDataSource = factory.Services.GetRequiredService<EndpointDataSource>();
        var endpoints = endpointDataSource.Endpoints.OfType<RouteEndpoint>();
        return endpoints
            .Where(e => e.Metadata.OfType<HttpMethodMetadata>().Any(m => m.HttpMethods.Contains(verb)))
            .Select(e => e.RoutePattern.RawText)
            .Where(route => route != null)
            .Select(route => "/" + route!.TrimStart('/'))
            .Except(exceptRoutes, StringComparer.OrdinalIgnoreCase)
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
