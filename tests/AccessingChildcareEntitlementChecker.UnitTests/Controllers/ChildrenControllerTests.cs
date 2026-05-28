using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class ChildrenControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly ChildrenController _controller;
    private readonly Guid childId;

    public ChildrenControllerTests()
    {
        _journeyState = new JourneyState();
        childId = Guid.Parse("00000000-0000-0000-0000-00000000000a");
        var childA = new ChildState(childId, "Child A");
        _journeyState.AddChild(childA);

        var guidFactory = Substitute.For<IGuidFactory>();
        guidFactory.NewGuid().Returns(Guid.Parse("00000000-0000-0000-0000-00000000000b"));
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new ChildrenController(_journeyState, _journeySession, guidFactory);
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
        var doesNotExist = Guid.Parse("00000000-0000-0000-0000-00000000000c");
        var result = Assert.IsType<NotFoundResult>(_controller.ChildName(doesNotExist));
    }

    [Fact]
    public void ChildName_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.TryGetChild(childId, out var child));
        child.Name = "Example";
        var result = Assert.IsType<ViewResult>(_controller.ChildName(childId));

        Assert.Equal("Example", result.Model<ChildNameViewModel>().ChildName);
    }

    [Fact]
    public void ChildName_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildNameViewModel
        {
            ChildId = childId,
            ChildName = "Example"
        };

        var result = _controller.ChildName(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.TryGetChild(model.ChildId.Value, out var child));
        Assert.Equal("Example", child.Name);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ChildrenController.ChildIsBorn), redirect.ActionName);
        Assert.Equal("Children", redirect.ControllerName);
    }

    [Fact]
    public void ChildName_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildNameViewModel
        {
            ChildId = childId,
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
        var result = Assert.IsType<ViewResult>(_controller.ChildIsBorn(childId));

        Assert.Null(result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBorn_IfChildDoesNotExistReturnsNotFound()
    {
        var doesNotExist = Guid.Parse("00000000-0000-0000-0000-00000000000c");
        var result = Assert.IsType<NotFoundResult>(_controller.ChildIsBorn(doesNotExist));
    }

    [Fact]
    public void IsChildBorn_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.TryGetChild(childId, out var child));
        child.BirthStatus = BirthStatus.Born;
        var result = Assert.IsType<ViewResult>(_controller.ChildIsBorn(childId));

        Assert.Equal(BirthStatus.Born, result.Model<ChildIsBornViewModel>().ChildIsBorn);
    }

    [Fact]
    public void IsChildBorn_Post_WithBorn_SavesState_AndRedirects()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = childId,
            ChildIsBorn = BirthStatus.Born
        };

        var result = _controller.ChildIsBorn(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.TryGetChild(model.ChildId, out var child));
        Assert.Equal(BirthStatus.Born, child.BirthStatus);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ChildrenController.ChildBirthDate), redirect.ActionName);
        Assert.Equal("Children", redirect.ControllerName);
    }

    [Fact]
    public void IsChildBorn_Post_WithDue_SavesState_AndRedirects()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = childId,
            ChildIsBorn = BirthStatus.Due
        };

        var result = _controller.ChildIsBorn(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.TryGetChild(model.ChildId, out var child));
        Assert.Equal(BirthStatus.Due, child.BirthStatus);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ChildrenController.ChildDueDate), redirect.ActionName);
        Assert.Equal("Children", redirect.ControllerName);
    }

    [Fact]
    public void IsChildBorn_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildIsBornViewModel
        {
            ChildId = childId,
            ChildIsBorn = null
        };

        _controller.ModelState.AddModelError(nameof(model.ChildIsBorn), "Faked Model Binding Error");

        var result = _controller.ChildIsBorn(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildIsBorn)));
        _journeySession.DidNotReceive().Set(_journeyState);
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
        var doesNotExist = Guid.Parse("00000000-0000-0000-0000-00000000000c");
        var result = Assert.IsType<NotFoundResult>(_controller.ChildBirthDate(doesNotExist));
    }

    [Fact]
    public void ChildBirthDate_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.TryGetChild(childId, out var child));
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
            ChildBirthDate = new DateOnly(2020, 1, 15)
        };

        var result = _controller.ChildBirthDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.TryGetChild(model.ChildId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.BirthDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ChildrenController.ChildRelationship), redirect.ActionName);
        Assert.Equal("Children", redirect.ControllerName);
    }

    [Fact]
    public void ChildBirthDate_Post_ValidSelection_SavesState_AndReturnsTo()
    {
        var model = new ChildBirthDateViewModel
        {
            ChildId = childId,
            ChildBirthDate = new DateOnly(2020, 1, 15),
            ReturnTo = "check-your-childrens-details"
        };

        var result = _controller.ChildBirthDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.TryGetChild(model.ChildId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.BirthDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("CheckChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildBirthDate_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildBirthDateViewModel
        {
            ChildId = childId,
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
        var result = Assert.IsType<ViewResult>(_controller.ChildRelationship(childId));

        Assert.Null(result.Model<ChildRelationshipViewModel>().Relationship);
        Assert.Equal("Child A", result.Model<ChildRelationshipViewModel>().ChildName);
    }

    [Fact]
    public void ChildRelationship_IfChildDoesNotExistReturnsNotFound()
    {
        var doesNotExist = Guid.Parse("00000000-0000-0000-0000-00000000000c");
        var result = Assert.IsType<NotFoundResult>(_controller.ChildRelationship(doesNotExist));
    }

    [Fact]
    public void ChildRelationship_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.TryGetChild(childId, out var child));
        child.BornRelationship = Relationship.Parent;
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
        Assert.True(_journeyState.TryGetChild(model.ChildId, out var child));
        Assert.Equal(Relationship.Parent, child.BornRelationship);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ChildrenController.ChildSupport), redirect.ActionName);
        Assert.Equal("Children", redirect.ControllerName);
    }

    [Fact]
    public void ChildRelationship_Post_ValidSelection_SavesState_AndReturnsTo()
    {
        var model = new ChildRelationshipViewModel
        {
            ChildId = childId,
            Relationship = Relationship.Parent,
            ReturnTo = "check-your-childrens-details"
        };

        var result = _controller.ChildRelationship(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("CheckChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildRelationship_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildRelationshipViewModel
        {
            ChildId = childId,
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
    public void ChildSupport_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildSupport(childId));

        Assert.Equal(Array.Empty<ChildSupport>(), result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Fact]
    public void ChildSupport_IfChildDoesNotExistReturnsNotFound()
    {
        var doesNotExist = Guid.Parse("00000000-0000-0000-0000-00000000000c");
        var result = Assert.IsType<NotFoundResult>(_controller.ChildSupport(doesNotExist));
    }

    [Fact]
    public void ChildSupport_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.TryGetChild(childId, out var child));
        child.ChildSupportOptions = [ChildSupport.ArmedForcesIndependencePayment];
        var result = Assert.IsType<ViewResult>(_controller.ChildSupport(childId));

        Assert.Equal(new[] { ChildSupport.ArmedForcesIndependencePayment }, result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Fact]
    public void ChildSupport_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildSupportViewModel
        {
            ChildId = childId,
            ChildSupportOptions = [ChildSupport.ArmedForcesIndependencePayment]
        };

        var result = _controller.ChildSupport(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.TryGetChild(model.ChildId, out var child));
        Assert.Equal(new[] { ChildSupport.ArmedForcesIndependencePayment }, child.ChildSupportOptions);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("CheckChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildSupport_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildSupportViewModel
        {
            ChildId = childId,
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
    public void ChildDueDate_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildDueDate(childId));
        Assert.Null(result.Model<ChildDueDateViewModel>().ChildDueDate);
    }

    [Fact]
    public void ChildDueDate_IfChildDoesNotExistReturnsNotFound()
    {
        var doesNotExist = Guid.Parse("00000000-0000-0000-0000-00000000000c");
        var result = Assert.IsType<NotFoundResult>(_controller.ChildDueDate(doesNotExist));
    }

    [Fact]
    public void ChildDueDate_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.TryGetChild(childId, out var child));
        child.DueDate = new DateOnly(2020, 1, 15);
        var result = Assert.IsType<ViewResult>(_controller.ChildDueDate(childId));
        Assert.Equal(new DateOnly(2020, 1, 15), result.Model<ChildDueDateViewModel>().ChildDueDate);
    }

    [Fact]
    public void ChildDueDate_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildDueDateViewModel
        {
            ChildId = childId,
            ChildDueDate = new DateOnly(2020, 1, 15)
        };

        var result = _controller.ChildDueDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.TryGetChild(model.ChildId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.DueDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(ChildrenController.ExpectedChildRelationship), redirect.ActionName);
        Assert.Equal("Children", redirect.ControllerName);
    }

    [Fact]
    public void ChildDueDate_Post_ValidSelection_SavesState_AndReturnsTo()
    {
        var model = new ChildDueDateViewModel
        {
            ChildId = childId,
            ChildDueDate = new DateOnly(2020, 1, 15),
            ReturnTo = "check-your-childrens-details"
        };

        var result = _controller.ChildDueDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.TryGetChild(model.ChildId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.DueDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("CheckChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildDueDate_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildDueDateViewModel
        {
            ChildId = childId,
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
        var result = Assert.IsType<ViewResult>(_controller.ExpectedChildRelationship(childId));
        Assert.Null(result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Fact]
    public void ExpectedChildRelationship_IfChildDoesNotExistReturnsNotFound()
    {
        var doesNotExist = Guid.Parse("00000000-0000-0000-0000-00000000000c");
        var result = Assert.IsType<NotFoundResult>(_controller.ExpectedChildRelationship(doesNotExist));
    }

    [Fact]
    public void ExpectedChildRelationship_Get_PopulatesModel_FromState()
    {
        Assert.True(_journeyState.TryGetChild(childId, out var child));
        child.ExpectedRelationship = Relationship.Parent;
        var result = Assert.IsType<ViewResult>(_controller.ExpectedChildRelationship(childId));
        Assert.Equal(Relationship.Parent, result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Fact]
    public void ExpectedChildRelationship_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ExpectedChildRelationshipViewModel
        {
            ChildId = childId,
            ExpectedChildRelationship = Relationship.Parent
        };

        var result = _controller.ExpectedChildRelationship(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_journeyState.TryGetChild(model.ChildId, out var child));
        Assert.Equal(Relationship.Parent, child.ExpectedRelationship);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("CheckChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ExpectedChildRelationship_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ExpectedChildRelationshipViewModel
        {
            ChildId = childId,
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
