using System;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class ErrorControllerTests : IDisposable
{
    private readonly ErrorController _errorController;

    public ErrorControllerTests()
    {
        _errorController = new ErrorController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public void InternalServerErrorReturnsViewWithStatusCode500()
    {
        var result = _errorController.InternalServerError();

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal(500, _errorController.Response.StatusCode);
    }

    [Theory]
    [InlineData(403, "InternalServerError")]
    [InlineData(404, "NotFound")]
    public void StatusCodePageReturnsViewWithMatchingStatusCode(int statusCode, string viewName)
    {
        var result = _errorController.StatusCodePage(statusCode);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewName, view.ViewName);
        Assert.Equal(statusCode, _errorController.Response.StatusCode);
    }

    [Fact]
    public void StatusCodePageReturnsErrorIf500()
    {
        var result = _errorController.StatusCodePage(500);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal("InternalServerError", view.ViewName);
        Assert.Equal(500, _errorController.Response.StatusCode);
    }

    public void Dispose() { _errorController?.Dispose(); GC.SuppressFinalize(this); }
}
