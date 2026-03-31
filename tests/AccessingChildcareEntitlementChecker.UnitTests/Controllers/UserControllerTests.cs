using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class UserControllerTests
{

    private UserController CreateController(FakeJourneySession session)
    {
        return new UserController(
            new FakeStringLocalizerFactory(),
            session);
    }
    
    [Fact]
    public void Index_ReturnsContentResult_WithPlaceholderText()
    {
        // Arrange
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        // Act≥
        var result = controller.Index();

        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("User controller placeholder", contentResult.Content);
    }
    
}