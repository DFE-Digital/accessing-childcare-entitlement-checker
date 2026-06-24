using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;

namespace AccessingChildcareEntitlementChecker.Web.Extensions;

public static class ControllerExtensions
{
    public static RedirectToActionResult RedirectTo<TController>(this Controller controller, string actionName, object? routeValues = null)
    {
        return controller.RedirectToAction(actionName, typeof(TController).Name.Replace("Controller", ""), routeValues);
    }

    public static RedirectToActionResult RedirectToReturnTo(this Controller controller, string returnTo, string? childId = null)
    {
        return returnTo switch
        {
            ReturnTo.CheckAnswers => controller.RedirectTo<SummaryController>(
                nameof(SummaryController.CheckAnswers),
                childId is null ? null : new { fromChildId = childId }),
            ReturnTo.CheckChildDetails => controller.RedirectTo<SummaryController>(
                nameof(SummaryController.CheckChildDetails),
                childId is null ? null : new { childId }),
            _ => throw new InvalidOperationException($"Unexpected return destination: {returnTo}")
        };
    }
}
