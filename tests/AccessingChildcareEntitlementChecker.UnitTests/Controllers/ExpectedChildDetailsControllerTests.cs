using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

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
        var result = _controller.ChildDueDate();
        Assert.Null(result.Model<ChildDueDateViewModel>().ChildDueDate);
    }

    [Fact]
    public void ChildDueDate_Get_PopulatesModel_FromState()
    {
        _journeyState.ChildDueDate = new DateOnly(2020, 1, 15);
        var result = _controller.ChildDueDate();
        Assert.Equal(new DateOnly(2020, 1, 15), result.Model<ChildDueDateViewModel>().ChildDueDate);
    }

    [Fact]
    public void ChildBirthDate_Post_ValidSelection_SavesState_AndRedirects()
    {
        var dueDate = new DateOnly(2020, 1, 15);
        var model = new ChildDueDateViewModel()
        {
            ChildDueDate = dueDate,
        };

        var result = _controller.ChildDueDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(dueDate, _journeyState.ChildDueDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ExpectedChildDetailsController.ExpectedChildRelationship), redirect.ActionName);
        Assert.Equal("ExpectedChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildDueDate_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildDueDateViewModel();

        _controller.ModelState.AddModelError(nameof(model.ChildDueDate), "Faked Model Binding Error");

        var result = _controller.ChildDueDate(model);

        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<ChildDueDateViewModel>(view.Model);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildDueDate)));
    }

    [Fact]
    public void ChildRelationship_ReturnsView()
    {
        var result = _controller.ExpectedChildRelationship();
        Assert.Null(result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Fact]
    public void ChildRelationship_Get_PopulatesModel_FromState()
    {
        _journeyState.ExpectedChildRelationship = Relationship.Parent;
        var result = _controller.ExpectedChildRelationship();
        Assert.Equal(Relationship.Parent, result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Fact]
    public void ChildRelationship_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ExpectedChildRelationshipViewModel()
        {
            ExpectedChildRelationship = Relationship.Parent,
        };

        var result = _controller.ExpectedChildRelationship(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(Relationship.Parent, _journeyState.ExpectedChildRelationship);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("CheckChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildRelationship_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ExpectedChildRelationshipViewModel();

        _controller.ModelState.AddModelError(nameof(model.ExpectedChildRelationship), "Faked Model Binding Error");

        var result = _controller.ExpectedChildRelationship(model);

        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<ExpectedChildRelationshipViewModel>(view.Model);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ExpectedChildRelationship)));
    }
}
