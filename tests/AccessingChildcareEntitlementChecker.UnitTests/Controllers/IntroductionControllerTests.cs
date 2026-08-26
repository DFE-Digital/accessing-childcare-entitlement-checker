using System;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.FeatureManagement;
using NSubstitute;
using System.Diagnostics;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class IntroductionControllerTests : IDisposable
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IFeatureManager _featureManager;
    private readonly IntroductionController _controller;
    private const string childId = "child-a";

    public IntroductionControllerTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Children[childId] = new Child(childId, "Child A");
        _journeySession = Substitute.For<IJourneySession>();
        _featureManager = Substitute.For<IFeatureManager>();
        _controller = new IntroductionController(_journeyState, _journeySession, _featureManager);
        _controller.Url = Substitute.For<IUrlHelper>();
        _controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("backlink");
    }

    [Fact]
    public async Task ChildNameReturnsView()
    {
        var result = Assert.IsType<ViewResult>(await _controller.ChildName());

        Assert.Null(result.Model<ChildNameViewModel>().ChildName);
    }

    [Fact]
    public async Task ChildNameIfChildDoesNotExistReturnsNotFound()
    {
        var result = Assert.IsType<NotFoundResult>(await _controller.ChildName("DOES-NOT-EXIST"));
    }

    [Fact]
    public async Task ChildNameGetPopulatesModelFromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(childId, out var child));
        child.Name = "Example";
        var result = Assert.IsType<ViewResult>(await _controller.ChildName(childId));

        Assert.Equal("Example", result.Model<ChildNameViewModel>().ChildName);
    }

    [Fact]
    public async Task ChildNamePostValidSelectionSavesStateAndRedirects()
    {
        var model = new ChildNameViewModel
        {
            ChildId = childId,
            ChildName = "Example"
        };

        var result = await _controller.ChildName(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(model.ChildId, out var child));
        Assert.Equal("Example", child.Name);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(IntroductionController.IsChildBorn), redirect.ActionName);
        Assert.Equal("Introduction", redirect.ControllerName);
    }

    [Fact]
    public async Task ChildNamePostInvalidSelectionReturnsViewWithError()
    {
        var model = new ChildNameViewModel
        {
            ChildId = "child-a",
            ChildName = null
        };

        _controller.ModelState.AddModelError(nameof(model.ChildName), "Faked Model Binding Error");

        var result = await _controller.ChildName(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildName)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void IsChildBornReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.IsChildBorn(childId));

        Assert.Null(result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBornIfChildDoesNotExistReturnsNotFound()
    {
        var result = Assert.IsType<NotFoundResult>(_controller.IsChildBorn("DOES-NOT-EXIST"));
    }

    [Fact]
    public void IsChildBornGetPopulatesModelFromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(childId, out var child));
        child.BirthStatus = BirthStatus.Born;
        var result = Assert.IsType<ViewResult>(_controller.IsChildBorn(childId));

        Assert.Equal(BirthStatus.Born, result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBornPostWithBornSavesStateAndRedirects()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = childId,
            ChildIsBorn = BirthStatus.Born
        };

        var result = _controller.IsChildBorn(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(model.ChildId, out var child));
        Assert.Equal(BirthStatus.Born, child.BirthStatus);
        Assert.Null(child.DueDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(BornChildDetailsController.ChildBirthDate), redirect.ActionName);
        Assert.Equal("BornChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void IsChildBornPostWithDueSavesStateAndRedirects()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = childId,
            ChildIsBorn = BirthStatus.Due
        };

        var result = _controller.IsChildBorn(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(model.ChildId, out var child));
        Assert.Equal(BirthStatus.Due, child.BirthStatus);
        Assert.Null(child.BirthDate);
        Assert.Empty(child.ChildSupportOptions);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ExpectedChildDetailsController.ChildDueDate), redirect.ActionName);
        Assert.Equal("ExpectedChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void IsChildBornPostInvalidSelectionReturnsViewWithError()
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
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void IsChildBornPostUnreachableCoverage()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = "child-a",
            ChildIsBorn = (BirthStatus)99,
        };

        Assert.Throws<UnreachableException>(() => _controller.IsChildBorn(model));
    }

    [Fact]
    public void IsChildBornPostNotFound()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = "child-b",
        };

        var result = _controller.IsChildBorn(model);
        Assert.IsType<NotFoundResult>(result);
    }

    public void Dispose() { _controller?.Dispose(); GC.SuppressFinalize(this); }
}
