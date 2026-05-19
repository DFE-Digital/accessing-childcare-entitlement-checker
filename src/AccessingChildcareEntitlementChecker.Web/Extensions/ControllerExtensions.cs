using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Extensions;

public static class ControllerExtensions
{
    public static RedirectToActionResult RedirectTo<TController>(this Controller controller, string actionName, object? routeValues = null)
    {
        return controller.RedirectToAction(actionName, typeof(TController).Name.Replace("Controller", ""), routeValues);
    }
}
