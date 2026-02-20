using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using Xunit;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class HomeControllerTests
{
    [Fact]
    public void Index_Returns_ViewResult()
    {
        var logger = new LoggerFactory().CreateLogger<HomeController>();
        var controller = new HomeController(logger);

        var result = controller.Index();

        Assert.IsType<ViewResult>(result);
    }
}