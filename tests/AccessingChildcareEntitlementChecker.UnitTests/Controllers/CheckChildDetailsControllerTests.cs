using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        _journeyState.Children[childId] = new ChildState("Child A");
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new CheckChildDetailsController(_journeyState, _journeySession);
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());
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

    [Fact]
    public void Remove_Post_WhenNotValidReturns()
    {
        var model = new RemoveChildViewModel { ChildId = childId, Name = "Child A", RemoveConfirmed = null, };

        _controller.ModelState.AddModelError(nameof(model.RemoveConfirmed), "Faked Model Binding Error");

        var result = _controller.Remove(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.RemoveConfirmed)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void Remove_Post_WhenNotConfirmed_Redirects()
    {
        var model = new RemoveChildViewModel { ChildId = childId, Name = "Child A", RemoveConfirmed = false, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), result.ActionName);
        _journeySession.Received(0).Set(_journeyState);
    }

    [Fact]
    public void Remove_Post_WhenConfirmed_AndFound_Redirects()
    {
        var model = new RemoveChildViewModel { ChildId = childId, Name = "Child A", RemoveConfirmed = true, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), result.ActionName);
        _journeySession.Received(1).Set(_journeyState);
    }

    [Fact]
    public void Remove_Post_WhenConfirmed_AndNotFound_Redirects()
    {
        var model = new RemoveChildViewModel { ChildId = "child-b", Name = "Child B", RemoveConfirmed = true, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), result.ActionName);
        _journeySession.Received(0).Set(_journeyState);
    }
}
