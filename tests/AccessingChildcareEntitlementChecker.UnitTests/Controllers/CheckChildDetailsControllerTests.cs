using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class CheckChildDetailsControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly CheckChildDetailsController _controller;
    private const string childId = "child-a";

    public CheckChildDetailsControllerTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Children[childId] = new Child(childId, "Child A");
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new CheckChildDetailsController(_journeyState, _journeySession);
    }

    [Fact]
    public void CheckChildDetails_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.CheckChildDetails());
        Assert.IsType<CheckChildDetailsViewModel>(result.Model);
    }

    [Fact]
    public void Remove_Get_ReturnsView_WhenChildExists()
    {
        var result = Assert.IsType<ViewResult>(_controller.Remove(childId));
        Assert.IsType<RemoveChildViewModel>(result.Model);
        Assert.Equal("Child A", result.Model<RemoveChildViewModel>().Name);
    }

    [Fact]
    public void Remove_Get_Redirects_WhenChildDoesNotExist()
    {
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove("DOES-NOT-EXIST"));
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), result.ActionName);
    }
}
