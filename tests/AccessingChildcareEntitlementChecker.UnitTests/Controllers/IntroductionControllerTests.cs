using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using AccessingChildcareEntitlementChecker.Web.Models;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class IntroductionControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IntroductionController _controller;
    private const string ChildId = "child-a";

    public IntroductionControllerTests()
    {
        var navigationService = Substitute.For<INavigationService>();
        navigationService.GetNextUrl(Arg.Any<Page>(), Arg.Any<string>(), Arg.Any<string>()).Returns(x => $"/mock-url-for-{x[0]}");

        _journeyState = new JourneyState
        {
            Children =
            {
                [ChildId] = new Child(ChildId, "Child A")
            }
        };
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new IntroductionController(_journeyState, _journeySession, navigationService);

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
        Assert.IsType<NotFoundResult>(_controller.ChildName("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ChildName_Get_PopulatesModel_FromState()
    {
        var child = _journeyState.GetChild(ChildId)!;
        child.Name = "Example";
        var result = Assert.IsType<ViewResult>(_controller.ChildName(ChildId));

        Assert.Equal("Example", result.Model<ChildNameViewModel>().ChildName);
    }

    [Fact]
    public void ChildName_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildNameViewModel
        {
            ChildId = ChildId,
            ChildName = "Example"
        };

        var result = _controller.ChildName(model);

        Assert.IsType<RedirectResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal("Example", _journeyState.GetChild(model.ChildId)!.Name);
        Assert.True(_controller.ModelState.IsValid);


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
        var result = Assert.IsType<ViewResult>(_controller.IsChildBorn(ChildId));

        Assert.Null(result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBorn_IfChildDoesNotExistReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(_controller.IsChildBorn("DOES-NOT-EXIST"));
    }

    [Fact]
    public void IsChildBorn_Get_PopulatesModel_FromState()
    {
        var child = _journeyState.GetChild(ChildId)!;
        child.BirthStatus = BirthStatus.Born;
        var result = Assert.IsType<ViewResult>(_controller.IsChildBorn(ChildId));

        Assert.Equal(BirthStatus.Born, result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBorn_Post_WithBorn_SavesState_AndRedirects()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = ChildId,
            ChildIsBorn = BirthStatus.Born
        };

        var result = _controller.IsChildBorn(model);

        Assert.IsType<RedirectResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(BirthStatus.Born, _journeyState.GetChild(model.ChildId)!.BirthStatus);
        Assert.True(_controller.ModelState.IsValid);


    }

    [Fact]
    public void IsChildBorn_Post_WithDue_SavesState_AndRedirects()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = ChildId,
            ChildIsBorn = BirthStatus.Due
        };

        var result = _controller.IsChildBorn(model);

        Assert.IsType<RedirectResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(BirthStatus.Due, _journeyState.GetChild(model.ChildId)!.BirthStatus);
        Assert.True(_controller.ModelState.IsValid);


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
