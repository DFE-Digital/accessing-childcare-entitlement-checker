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
    private readonly IntroductionController _controller;
    private const string ChildId = "child-a";

    public IntroductionControllerTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Children[ChildId] = new Child(ChildId, "Child A");
        _journeySession = Substitute.For<IJourneySession>();
        var featureManager = Substitute.For<IFeatureManager>();
        _controller = new IntroductionController(_journeyState, _journeySession, featureManager);
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
        Assert.IsType<NotFoundResult>(await _controller.ChildName("DOES-NOT-EXIST"));
    }

    [Fact]
    public async Task ChildNameGetPopulatesModelFromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(ChildId, out var child));
        child.Name = "Example";
        var result = Assert.IsType<ViewResult>(await _controller.ChildName(ChildId));

        Assert.Equal("Example", result.Model<ChildNameViewModel>().ChildName);
    }

    [Fact]
    public async Task ChildNamePostValidSelectionSavesStateAndRedirects()
    {
        var model = new ChildNameViewModel
        {
            ChildId = ChildId,
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
        var result = Assert.IsType<ViewResult>(_controller.IsChildBorn(ChildId));

        Assert.Null(result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBornIfChildDoesNotExistReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(_controller.IsChildBorn("DOES-NOT-EXIST"));
    }

    [Fact]
    public void IsChildBornGetPopulatesModelFromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(ChildId, out var child));
        child.BirthStatus = BirthStatus.Born;
        var result = Assert.IsType<ViewResult>(_controller.IsChildBorn(ChildId));

        Assert.Equal(BirthStatus.Born, result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBornPostWithBornSavesStateAndRedirects()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = ChildId,
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
            ChildId = ChildId,
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
