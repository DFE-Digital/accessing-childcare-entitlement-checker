using Dfe.Acec.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web;

public static class UrlHelperExtensions
{
    public static string GetBackLinkOrAction(this IUrlHelper urlHelper, string? returnTo, string actionName, string? controllerName = null, object? routeValues = null)
    {
        if (ReturnTo.TryGetReturnToUrl(urlHelper, returnTo, out var url))
        {
            return url;
        }

        return controllerName == null
            ? urlHelper.ActionOrThrow(actionName, routeValues)
            : urlHelper.ActionOrThrow(actionName, controllerName, routeValues);
    }

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
