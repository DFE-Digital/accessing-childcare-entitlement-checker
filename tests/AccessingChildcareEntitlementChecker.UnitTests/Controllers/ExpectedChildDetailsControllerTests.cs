using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class ExpectedChildDetailsControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly ExpectedChildDetailsController _controller;

    public ExpectedChildDetailsControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new ExpectedChildDetailsController(_journeyState, _journeySession);
    }

    [Fact]
    public void ChildDueDate_ReturnsView()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var result = Assert.IsType<ViewResult>(_controller.ChildDueDate(childId));

        Assert.Null(result.Model<ChildDueDateViewModel>().ChildDueDate);
    }

    [Fact]
    public void ChildDueDate_Get_PopulatesModel_FromState()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var child = _journeyState.GetChild(childId);
        child.DueDate = new DateOnly(2020, 1, 15);
        var result = Assert.IsType<ViewResult>(_controller.ChildDueDate(childId));

        Assert.Equal(new DateOnly(2020, 1, 15), result.Model<ChildDueDateViewModel>().ChildDueDate);
    }

    [Fact]
    public void ChildDueDate_Post_ValidSelection_SavesState_AndRedirects()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var model = new ChildDueDateViewModel
        {
            ChildId = childId,
            ChildDueDate = new DateOnly(2020, 1, 15)
        };

        var result = _controller.ChildDueDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(new DateOnly(2020, 1, 15), _journeyState.GetChild(model.ChildId).DueDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ExpectedChildDetailsController.ExpectedChildRelationship), redirect.ActionName);
        Assert.Equal("ExpectedChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildDueDate_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildDueDateViewModel
        {
            ChildId = "child-a",
            ChildDueDate = null
        };

        _controller.ModelState.AddModelError(nameof(model.ChildDueDate), "Faked Model Binding Error");

        var result = _controller.ChildDueDate(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildDueDate)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void ExpectedChildRelationship_ReturnsView()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var result = Assert.IsType<ViewResult>(_controller.ExpectedChildRelationship(childId));

        Assert.Null(result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Fact]
    public void ExpectedChildRelationship_Get_PopulatesModel_FromState()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var child = _journeyState.GetChild(childId);
        child.ExpectedRelationship = RelationshipOption.Parent;
        var result = Assert.IsType<ViewResult>(_controller.ExpectedChildRelationship(childId));

        Assert.Equal(RelationshipOption.Parent, result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Fact]
    public void ExpectedChildRelationship_Post_ValidSelection_SavesState_AndRedirects()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var model = new ExpectedChildRelationshipViewModel
        {
            ChildId = childId,
            ExpectedChildRelationship = RelationshipOption.Parent
        };

        var result = _controller.ExpectedChildRelationship(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(RelationshipOption.Parent, _journeyState.GetChild(model.ChildId).ExpectedRelationship);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("CheckChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ExpectedChildRelationship_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ExpectedChildRelationshipViewModel
        {
            ChildId = "child-a",
            ExpectedChildRelationship = null
        };

        _controller.ModelState.AddModelError(nameof(model.ExpectedChildRelationship), "Faked Model Binding Error");

        var result = _controller.ExpectedChildRelationship(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ExpectedChildRelationship)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }
}
