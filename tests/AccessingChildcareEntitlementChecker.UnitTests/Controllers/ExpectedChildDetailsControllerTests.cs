using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class ExpectedChildDetailsControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly ExpectedChildDetailsController _controller;
    private const string ChildId = "child-a";

    public ExpectedChildDetailsControllerTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Children[ChildId] = new Child(ChildId, "Child A");
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new ExpectedChildDetailsController(_journeyState, _journeySession);
        _controller.Url = Substitute.For<IUrlHelper>();
        _controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("backlink");
    }

    [Fact]
    public void ChildDueDate_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildDueDate(ChildId));
        Assert.Null(result.Model<ChildDueDateViewModel>().ChildDueDate);
    }

    [Fact]
    public void ChildDueDate_IfChildDoesNotExistReturnsNotFound()
    {
        var result = Assert.IsType<NotFoundResult>(_controller.ChildDueDate("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ChildDueDate_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(ChildId, out var child));
        child.DueDate = new DateOnly(2020, 1, 15);
        var result = Assert.IsType<ViewResult>(_controller.ChildDueDate(ChildId));
        Assert.Equal(new DateOnly(2020, 1, 15), result.Model<ChildDueDateViewModel>().ChildDueDate);
    }

    [Fact]
    public void ChildDueDate_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildDueDateViewModel
        {
            ChildId = ChildId,
            ChildDueDate = new DateOnly(2020, 1, 15)
        };

        var result = _controller.ChildDueDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(ChildId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.DueDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ExpectedChildDetailsController.ExpectedChildRelationship), redirect.ActionName);
    }

    [Fact]
    public void ChildDueDate_Post_ValidSelection_SavesState_AndReturnsTo()
    {
        _journeyState.Children[ChildId].ExpectedRelationship = Relationship.Parent;
        var model = new ChildDueDateViewModel
        {
            ChildId = ChildId,
            ChildDueDate = new DateOnly(2020, 1, 15),
            ReturnTo = ReturnTo.CheckChildDetails
        };

        var result = _controller.ChildDueDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(model.ChildId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.DueDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(SummaryController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("Summary", redirect.ControllerName);
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
    public void ChildDueDate_Post_NotFound()
    {
        var model = new ChildDueDateViewModel
        {
            ChildId = "child-b",
        };

        var result = _controller.ChildDueDate(model);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void ExpectedChildRelationship_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ExpectedChildRelationship(ChildId));
        Assert.Null(result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Fact]
    public void ExpectedChildRelationship_IfChildDoesNotExistReturnsNotFound()
    {
        var result = Assert.IsType<NotFoundResult>(_controller.ExpectedChildRelationship("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ExpectedChildRelationship_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(ChildId, out var child));
        child.ExpectedRelationship = Relationship.Parent;
        var result = Assert.IsType<ViewResult>(_controller.ExpectedChildRelationship(ChildId));
        Assert.Equal(Relationship.Parent, result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Theory]
    [InlineData(ReturnTo.CheckChildDetails, nameof(SummaryController.CheckChildDetails))]
    [InlineData(ReturnTo.CheckAnswers, nameof(SummaryController.CheckAnswers))]
    public void ExpectedChildRelationship_Post_ValidSelection_SavesState_AndRedirects(string? returnTo, string actionName)
    {
        var model = new ExpectedChildRelationshipViewModel
        {
            ChildId = ChildId,
            ExpectedChildRelationship = Relationship.Parent,
            ReturnTo = returnTo,
        };

        var result = _controller.ExpectedChildRelationship(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(model.ChildId, out var child));
        Assert.Equal(Relationship.Parent, child.ExpectedRelationship);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal("Summary", redirect.ControllerName);
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

    [Fact]
    public void ExpectedChildRelationship_Post_NotFound()
    {
        var model = new ExpectedChildRelationshipViewModel
        {
            ChildId = "child-b",
        };

        var result = _controller.ExpectedChildRelationship(model);
        Assert.IsType<NotFoundResult>(result);
    }
}
