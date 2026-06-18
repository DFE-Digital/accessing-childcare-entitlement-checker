using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class IntroductionControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IntroductionController _controller;
    private const string childId = "child-a";

    public IntroductionControllerTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Children[childId] = new Child(childId, "Child A");
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new IntroductionController(_journeyState, _journeySession);
        _controller.Url = Substitute.For<IUrlHelper>();
        _controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("backlink");
    }

    [Fact]
    public void ChildName_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildName());

        Assert.Null(result.Model<ChildNameViewModel>().ChildName);
    }

    [Fact]
    public void ChildName_IfChildDoesNotExistReturnsNotFound()
    {
        var result = Assert.IsType<NotFoundResult>(_controller.ChildName("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ChildName_Get_PopulatesModel_FromState()
    {
        var child = _journeyState.GetChild(childId)!;
        child.Name = "Example";
        var result = Assert.IsType<ViewResult>(_controller.ChildName(childId));

        Assert.Equal("Example", result.Model<ChildNameViewModel>().ChildName);
    }

    [Fact]
    public void ChildName_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildNameViewModel
        {
            ChildId = childId,
            ChildName = "Example"
        };

        var result = _controller.ChildName(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal("Example", _journeyState.GetChild(model.ChildId)!.Name);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(IntroductionController.IsChildBorn), redirect.ActionName);
        Assert.Equal("Introduction", redirect.ControllerName);
    }

    [Fact]
    public void ChildName_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildNameViewModel
        {
            ChildId = "child-a",
            ChildName = null
        };

        _controller.ModelState.AddModelError(nameof(model.ChildName), "Faked Model Binding Error");

        var result = _controller.ChildName(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildName)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void IsChildBorn_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.IsChildBorn(childId));

        Assert.Null(result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBorn_IfChildDoesNotExistReturnsNotFound()
    {
        var result = Assert.IsType<NotFoundResult>(_controller.IsChildBorn("DOES-NOT-EXIST"));
    }

    [Fact]
    public void IsChildBorn_Get_PopulatesModel_FromState()
    {
        var child = _journeyState.GetChild(childId)!;
        child.BirthStatus = BirthStatus.Born;
        var result = Assert.IsType<ViewResult>(_controller.IsChildBorn(childId));

        Assert.Equal(BirthStatus.Born, result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBorn_Post_WithBorn_SavesState_AndRedirects()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = childId,
            ChildIsBorn = BirthStatus.Born
        };

        var result = _controller.IsChildBorn(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(BirthStatus.Born, _journeyState.GetChild(model.ChildId)!.BirthStatus);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(BornChildDetailsController.ChildBirthDate), redirect.ActionName);
        Assert.Equal("BornChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void IsChildBorn_Post_WithDue_SavesState_AndRedirects()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = childId,
            ChildIsBorn = BirthStatus.Due
        };

        var result = _controller.IsChildBorn(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(BirthStatus.Due, _journeyState.GetChild(model.ChildId)!.BirthStatus);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ExpectedChildDetailsController.ChildDueDate), redirect.ActionName);
        Assert.Equal("ExpectedChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void IsChildBorn_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = "child-a",
            ChildIsBorn = null
        };

        _controller.ModelState.AddModelError(nameof(model.ChildIsBorn), "Faked Model Binding Error");

        var result = _controller.IsChildBorn(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildIsBorn)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }
}
