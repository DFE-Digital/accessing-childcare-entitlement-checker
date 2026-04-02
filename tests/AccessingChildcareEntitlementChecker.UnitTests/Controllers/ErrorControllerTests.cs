using AccessingChildcareEntitlementChecker.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;


public class ErrorControllerTests
{
    private ErrorController CreateController()
    {
        return new ErrorController(NullLogger<ErrorController>.Instance)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public void Error_ReturnsView_WithStatusCode500()
    {
        var controller = CreateController();

        var result = controller.Error();

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal(500, controller.Response.StatusCode);
    }
}