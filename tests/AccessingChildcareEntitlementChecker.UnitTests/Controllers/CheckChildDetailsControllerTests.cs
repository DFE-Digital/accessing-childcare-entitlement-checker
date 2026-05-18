using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class CheckChildDetailsControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly CheckChildDetailsController _controller;

    public CheckChildDetailsControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new CheckChildDetailsController(_journeyState, _journeySession)
        {
            TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>())
        };
    }

    [Fact]
    public void CheckChildDetails_ReturnsView_WithChildren()
    {
        var child = AddChild("child-a", "Child A");

        var result = _controller.CheckChildDetails(child.ChildId);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CheckChildDetailsViewModel>(view.Model);
        Assert.True(model.HasChildren);
        Assert.Single(model.YourChildren);
        Assert.Equal(child.ChildId, model.LastEditedChild?.ChildId);
    }

    [Fact]
    public void CheckChildDetails_ReturnsView_WithEmptyChildren()
    {
        var result = _controller.CheckChildDetails();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CheckChildDetailsViewModel>(view.Model);
        Assert.False(model.HasChildren);
        Assert.Empty(model.YourChildren);
    }

    [Fact]
    public void Delete_Get_WithExistingChild_ReturnsView()
    {
        var child = AddChild("child-a", "Child A");

        var result = _controller.Delete(child.ChildId);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<DeleteChildViewModel>(view.Model);
        Assert.Equal(child.ChildId, model.ChildId);
        Assert.Equal(child.Name, model.Name);
        Assert.Null(model.DeleteConfirmed);
    }

    [Fact]
    public void Delete_Get_WithMissingChild_RedirectsToSummary()
    {
        var result = _controller.Delete("missing-child");

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
    }

    [Fact]
    public void Delete_Post_WhenConfirmed_RemovesChild_SavesState_AndRedirectsToSummary()
    {
        var child = AddChild("child-a", "Child A");
        var model = new DeleteChildViewModel
        {
            ChildId = child.ChildId,
            Name = child.Name,
            DeleteConfirmed = true
        };

        var result = _controller.Delete(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.False(_journeyState.Children.ContainsKey(child.ChildId));
        Assert.Equal(child.Name, _controller.TempData["DeletedChildName"]);
        _journeySession.Received(1).Set(_journeyState);
    }

    [Fact]
    public void Delete_Post_WhenNotConfirmed_RedirectsToSummary_WithoutDeleting()
    {
        var child = AddChild("child-a", "Child A");
        var model = new DeleteChildViewModel
        {
            ChildId = child.ChildId,
            Name = child.Name,
            DeleteConfirmed = false
        };

        var result = _controller.Delete(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.True(_journeyState.Children.ContainsKey(child.ChildId));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void Delete_Post_WithInvalidModel_ReturnsView_WithoutSaving()
    {
        var model = new DeleteChildViewModel
        {
            ChildId = "child-a",
            Name = "Child A",
            DeleteConfirmed = null
        };
        _controller.ModelState.AddModelError(nameof(model.DeleteConfirmed), "Faked Model Binding Error");

        var result = _controller.Delete(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.DeleteConfirmed)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    private Child AddChild(string childId, string name)
    {
        var child = new Child(childId, name)
        {
            BirthStatus = BirthStatusOption.Born,
            BirthDate = new DateOnly(2020, 1, 15),
            BornRelationship = RelationshipOption.Parent,
            ChildSupportOptions = [ChildSupportOption.ArmedForcesIndependencePayment],
            DueDate = new DateOnly(2020, 1, 15),
            ExpectedRelationship = RelationshipOption.Parent,
        };
        _journeyState.Children[childId] = child;
        return child;
    }
}
