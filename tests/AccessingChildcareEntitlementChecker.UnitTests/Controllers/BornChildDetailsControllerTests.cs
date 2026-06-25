using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NSubstitute;
using System.Diagnostics;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class BornChildDetailsControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly BornChildDetailsController _controller;
    private const string childId = "child-a";

    public BornChildDetailsControllerTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Children[childId] = new Child(childId, "Child A")
        {
            BirthStatus = BirthStatus.Born,
        };

        _journeySession = Substitute.For<IJourneySession>();
        _controller = new BornChildDetailsController(_journeyState, _journeySession);
        _controller.Url = Substitute.For<IUrlHelper>();
        _controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("backlink");
    }

    [Fact]
    public void ChildBirthDate_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildBirthDate(childId));
        Assert.Null(result.Model<ChildBirthDateViewModel>().ChildBirthDate);
        Assert.Equal("Child A", result.Model<ChildBirthDateViewModel>().ChildName);
    }

    [Fact]
    public void ChildBirthDate_IfChildDoesNotExistReturnsNotFound()
    {
        var result = Assert.IsType<NotFoundResult>(_controller.ChildBirthDate("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ChildBirthDate_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(childId, out var child));
        child.BirthDate = new DateOnly(2020, 1, 15);
        var result = Assert.IsType<ViewResult>(_controller.ChildBirthDate(childId));
        Assert.Equal(new DateOnly(2020, 1, 15), result.Model<ChildBirthDateViewModel>().ChildBirthDate);
        Assert.Equal("Child A", result.Model<ChildBirthDateViewModel>().ChildName);
    }

    [Fact]
    public void ChildBirthDate_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildBirthDateViewModel
        {
            ChildId = childId,
            ChildBirthDate = new DateOnly(2020, 1, 15),
        };

        var result = _controller.ChildBirthDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(childId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.BirthDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(BornChildDetailsController.ChildRelationship), redirect.ActionName);
    }

    [Fact]
    public void ChildBirthDate_Post_ValidSelection_SavesState_AndReturnsTo()
    {
        _journeyState.Children[childId].BornRelationship = Relationship.Parent;
        _journeyState.Children[childId].ChildSupportOptions = [ChildSupport.ArmedForcesIndependencePayment];

        var model = new ChildBirthDateViewModel
        {
            ChildId = childId,
            ChildBirthDate = new DateOnly(2020, 1, 15),
            ReturnTo = ReturnTo.CheckChildDetails
        };

        var result = _controller.ChildBirthDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(model.ChildId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.BirthDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(SummaryController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("Summary", redirect.ControllerName);
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
    public void ChildBirthDate_Post_NotFound()
    {
        var model = new ChildBirthDateViewModel
        {
            ChildId = "child-b",
        };

        var result = _controller.ChildBirthDate(model);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void ChildRelationship_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildRelationship(childId));

        Assert.Null(result.Model<ChildRelationshipViewModel>().Relationship);
        Assert.Equal("Child A", result.Model<ChildRelationshipViewModel>().ChildName);
    }

    [Fact]
    public void ChildRelationship_IfChildDoesNotExistReturnsNotFound()
    {
        var result = Assert.IsType<NotFoundResult>(_controller.ChildRelationship("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ChildRelationship_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(childId, out var child));
        child.BornRelationship = Relationship.Parent;
        var result = Assert.IsType<ViewResult>(_controller.ChildRelationship(childId));

        Assert.Equal(Relationship.Parent, result.Model<ChildRelationshipViewModel>().Relationship);
        Assert.Equal("Child A", result.Model<ChildRelationshipViewModel>().ChildName);
    }

    [Fact]
    public void ChildRelationship_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildRelationshipViewModel
        {
            ChildId = childId,
            Relationship = Relationship.Parent
        };

        var result = _controller.ChildRelationship(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(childId, out var child));
        Assert.Equal(Relationship.Parent, child.BornRelationship);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(BornChildDetailsController.ChildSupport), redirect.ActionName);
    }

    [Fact]
    public void ChildRelationship_Post_ValidSelection_SavesState_AndReturnsTo()
    {
        _journeyState.Children[childId].ChildSupportOptions = [ChildSupport.ArmedForcesIndependencePayment];

        var model = new ChildRelationshipViewModel
        {
            ChildId = childId,
            Relationship = Relationship.Parent,
            ReturnTo = ReturnTo.CheckChildDetails
        };

        var result = _controller.ChildRelationship(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_controller.ModelState.IsValid);
        Assert.True(_journeyState.Children.TryGetValue(childId, out var child));
        Assert.Equal(Relationship.Parent, child.BornRelationship);
        Assert.Equal(nameof(SummaryController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("Summary", redirect.ControllerName);
    }

    [Fact]
    public void ChildRelationship_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildRelationshipViewModel
        {
            ChildId = "child-a",
            Relationship = null
        };

        _controller.ModelState.AddModelError(nameof(model.Relationship), "Faked Model Binding Error");

        var result = _controller.ChildRelationship(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.Relationship)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void ChildRelationship_Post_NotFound()
    {
        var model = new ChildRelationshipViewModel
        {
            ChildId = "child-b",
        };

        var result = _controller.ChildRelationship(model);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void ChildSupport_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildSupport(childId));

        Assert.Equal(Array.Empty<ChildSupport>(), result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Fact]
    public void ChildSupport_IfChildDoesNotExistReturnsNotFound()
    {
        var result = Assert.IsType<NotFoundResult>(_controller.ChildSupport("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ChildSupport_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(childId, out var child));
        child.ChildSupportOptions = [ChildSupport.ArmedForcesIndependencePayment];
        var result = Assert.IsType<ViewResult>(_controller.ChildSupport(childId));

        Assert.Equal(new[] { ChildSupport.ArmedForcesIndependencePayment }, result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Theory]
    [InlineData(ReturnTo.CheckChildDetails, nameof(SummaryController.CheckChildDetails))]
    [InlineData(ReturnTo.CheckAnswers, nameof(SummaryController.CheckAnswers))]
    public void ChildSupport_Post_ValidSelection_SavesState_AndRedirects(string returnTo, string actionName)
    {
        var model = new ChildSupportViewModel
        {
            ChildId = childId,
            ChildSupportOptions = [ChildSupport.ArmedForcesIndependencePayment],
            ReturnTo = returnTo,
        };

        var result = _controller.ChildSupport(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(childId, out var child));
        Assert.Equal(new[] { ChildSupport.ArmedForcesIndependencePayment }, child.ChildSupportOptions);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal("Summary", redirect.ControllerName);
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

    [Fact]
    public void ChildSupport_Post_NotFound()
    {
        var model = new ChildSupportViewModel
        {
            ChildId = "child-b",
        };

        var result = _controller.ChildSupport(model);
        Assert.IsType<NotFoundResult>(result);
    }
}
