using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Services;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class IntroductionControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IntroductionController _controller;

    public IntroductionControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new IntroductionController(_journeyState, _journeySession);
    }

    [Fact]
    public void ChildName_ReturnsView()
    {
        var result = _controller.ChildName();
        Assert.Null(result.Model<ChildNameViewModel>().ChildName);
    }

    [Fact]
    public void ChildName_Get_PopulatesModel_FromState()
    {
        _journeyState.ChildName = "Child A";
        var result = _controller.ChildName();
        Assert.Equal("Child A", result.Model<ChildNameViewModel>().ChildName);
    }

    [Fact]
    public void ChildName_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildNameViewModel()
        {
            ChildName = "Child A",
        };

        var result = _controller.ChildName(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal("Child A", _journeyState.ChildName);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(IntroductionController.IsChildBorn), redirect.ActionName);
    }

    [Fact]
    public void ChildName_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildNameViewModel
        {
            ChildName = null
        };

        _controller.ModelState.AddModelError(nameof(model.ChildName), "Faked Model Binding Error");

        var result = _controller.ChildName(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildName)));
    }
}