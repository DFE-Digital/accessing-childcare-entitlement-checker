using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public record EdgeContext(JourneyState JourneyState, RouteValueDictionary RouteValues)
{
    public static EdgeContext From(Controller controller, JourneyState journeyState, object? routeValues = null)
    {
        return new EdgeContext(journeyState, BuildRouteValues(controller, routeValues));
    }

    public RouteValueDictionary RouteValuesWithoutReturnTo()
    {
        var values = new RouteValueDictionary(RouteValues);
        values.Remove("returnTo");
        return values;
    }

    private static RouteValueDictionary BuildRouteValues(Controller controller, object? routeValues)
    {
        var routeValuesDict = new RouteValueDictionary(controller.RouteData.Values);

        if (routeValues != null)
        {
            foreach (var routeValue in new RouteValueDictionary(routeValues))
            {
                routeValuesDict[routeValue.Key] = routeValue.Value;
            }
        }

        foreach (var query in controller.Request.Query)
        {
            routeValuesDict[query.Key] = query.Value.ToString();
        }

        routeValuesDict.Remove("controller");
        routeValuesDict.Remove("action");

        return routeValuesDict;
    }
}
