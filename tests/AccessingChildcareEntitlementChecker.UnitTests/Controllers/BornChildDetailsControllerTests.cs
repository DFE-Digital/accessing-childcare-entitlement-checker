using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class BornChildDetailsControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly BornChildDetailsController _controller;

    public BornChildDetailsControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new BornChildDetailsController(_journeyState, _journeySession);
    }

    [Fact]
    public void ChildBirthDate_ReturnsView()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var result = Assert.IsType<ViewResult>(_controller.ChildBirthDate(childId));

        Assert.Null(result.Model<ChildBirthDateViewModel>().ChildBirthDate);
        Assert.Equal("Child A", result.Model<ChildBirthDateViewModel>().ChildName);
    }

    [Fact]
    public void ChildBirthDate_Get_PopulatesModel_FromState()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var child = _journeyState.GetChild(childId);
        child.BirthDate = new DateOnly(2020, 1, 15);
        var result = Assert.IsType<ViewResult>(_controller.ChildBirthDate(childId));

        Assert.Equal(new DateOnly(2020, 1, 15), result.Model<ChildBirthDateViewModel>().ChildBirthDate);
        Assert.Equal("Child A", result.Model<ChildBirthDateViewModel>().ChildName);
    }

    [Fact]
    public void ChildBirthDate_Post_ValidSelection_SavesState_AndRedirects()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var model = new ChildBirthDateViewModel
        {
            ChildId = childId,
            ChildBirthDate = new DateOnly(2020, 1, 15)
        };

        var result = _controller.ChildBirthDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(new DateOnly(2020, 1, 15), _journeyState.GetChild(model.ChildId).BirthDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(BornChildDetailsController.ChildRelationship), redirect.ActionName);
        Assert.Equal("BornChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildBirthDate_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildBirthDateViewModel
        {
            ChildId = "child-a",
            ChildBirthDate = null
        };

        _controller.ModelState.AddModelError(nameof(model.ChildBirthDate), "Faked Model Binding Error");

        var result = _controller.ChildBirthDate(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildBirthDate)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void ChildRelationship_ReturnsView()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var result = Assert.IsType<ViewResult>(_controller.ChildRelationship(childId));

        Assert.Null(result.Model<ChildRelationshipViewModel>().ChildRelationship);
        Assert.Equal("Child A", result.Model<ChildRelationshipViewModel>().ChildName);
    }

    [Fact]
    public void ChildRelationship_Get_PopulatesModel_FromState()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var child = _journeyState.GetChild(childId);
        child.BornRelationship = RelationshipOption.Parent;
        var result = Assert.IsType<ViewResult>(_controller.ChildRelationship(childId));

        Assert.Equal(RelationshipOption.Parent, result.Model<ChildRelationshipViewModel>().ChildRelationship);
        Assert.Equal("Child A", result.Model<ChildRelationshipViewModel>().ChildName);
    }

    [Fact]
    public void ChildRelationship_Post_ValidSelection_SavesState_AndRedirects()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var model = new ChildRelationshipViewModel
        {
            ChildId = childId,
            ChildRelationship = RelationshipOption.Parent
        };

        var result = _controller.ChildRelationship(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(RelationshipOption.Parent, _journeyState.GetChild(model.ChildId).BornRelationship);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(BornChildDetailsController.ChildSupport), redirect.ActionName);
        Assert.Equal("BornChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildRelationship_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildRelationshipViewModel
        {
            ChildId = "child-a",
            ChildRelationship = null
        };

        _controller.ModelState.AddModelError(nameof(model.ChildRelationship), "Faked Model Binding Error");

        var result = _controller.ChildRelationship(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildRelationship)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void ChildSupport_ReturnsView()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var result = Assert.IsType<ViewResult>(_controller.ChildSupport(childId));

        Assert.Equal(Array.Empty<ChildSupportOption>(), result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Fact]
    public void ChildSupport_Get_PopulatesModel_FromState()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var child = _journeyState.GetChild(childId);
        child.ChildSupportOptions = [ChildSupportOption.ArmedForcesIndependencePayment];
        var result = Assert.IsType<ViewResult>(_controller.ChildSupport(childId));

        Assert.Equal(new[] { ChildSupportOption.ArmedForcesIndependencePayment }, result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Fact]
    public void ChildSupport_Post_ValidSelection_SavesState_AndRedirects()
    {
        const string childId = "child-a";
        _journeyState.Children[childId] = new Child(childId, "Child A");
        var model = new ChildSupportViewModel
        {
            ChildId = childId,
            ChildSupportOptions = [ChildSupportOption.ArmedForcesIndependencePayment]
        };

        var result = _controller.ChildSupport(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(new[] { ChildSupportOption.ArmedForcesIndependencePayment }, _journeyState.GetChild(model.ChildId).ChildSupportOptions);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("CheckChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildSupport_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildSupportViewModel
        {
            ChildId = "child-a",
            ChildSupportOptions = []
        };

        _controller.ModelState.AddModelError(nameof(model.ChildSupportOptions), "Faked Model Binding Error");

        var result = _controller.ChildSupport(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildSupportOptions)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }
}
