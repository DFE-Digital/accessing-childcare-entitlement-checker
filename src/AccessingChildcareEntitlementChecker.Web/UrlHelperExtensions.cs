using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web;

public static class UrlHelperExtensions
{
    public static string ActionOrThrow(this IUrlHelper urlHelper, string actionName, object? routeValues = null)
    {
        var url = urlHelper.Action(actionName, routeValues);
        return url ?? throw new InvalidOperationException($"Could not generate URL for action '{actionName}'");
    }

    public static string ActionOrThrow(this IUrlHelper urlHelper, string actionName, string controllerName, object? routeValues = null)
    {
        var url = urlHelper.Action(actionName, controllerName, routeValues);
        return url ?? throw new InvalidOperationException($"Could not generate URL for action '{actionName}' in controller '{controllerName}'");
    }
}
