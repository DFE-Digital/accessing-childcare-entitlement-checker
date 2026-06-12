using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public record PageKey(string ControllerName, string ActionName)
{
    public static PageKey From(Controller controller)
    {
        var controllerName = controller.ControllerContext.ActionDescriptor.ControllerName;
        var actionName = controller.ControllerContext.ActionDescriptor.ActionName;
        return new PageKey(controllerName, actionName);
    }
}
