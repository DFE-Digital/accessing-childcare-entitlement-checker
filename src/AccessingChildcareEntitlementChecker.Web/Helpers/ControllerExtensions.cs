using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AccessingChildcareEntitlementChecker.Web.Helpers;

public static class ControllerExtensions
{
    public static void SetBackButton(this Controller controller, string href)
    {
        controller.ViewData["BackButton"] = new NavigationLinkModel
        {
            Href = href
        };
    }
}