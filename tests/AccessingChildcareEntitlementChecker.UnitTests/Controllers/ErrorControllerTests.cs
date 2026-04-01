using System.Diagnostics;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class ErrorControllerTests : Controller
{
    private ErrorController CreateController()
    {
        return new ErrorController();
    }

    [Fact]
    public void Index_ReturnsView_WithActivityId()
    {
        var controller = CreateController();

        var activity = new Activity("test");
        activity.Start();

        var result = controller.Index();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(view.Model);

        Assert.Equal(activity.Id, model.RequestId);

        activity.Stop();
    }

    [Fact]
    public void Index_ReturnsView_WithTraceIdentifier_WhenNoActivity()
    {
        var controller = CreateController();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var expectedTraceId = controller.HttpContext.TraceIdentifier;

        var result = controller.Index();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(view.Model);

        Assert.Equal(expectedTraceId, model.RequestId);
    }
}