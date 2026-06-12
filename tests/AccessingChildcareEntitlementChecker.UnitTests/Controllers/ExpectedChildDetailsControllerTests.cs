using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
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
    private const string ChildId = "child-a";

    public ExpectedChildDetailsControllerTests()
    {
        var navigationService = Substitute.For<INavigationService>();
        navigationService.GetNextUrl(Arg.Any<Page>(), Arg.Any<string>(), Arg.Any<string>()).Returns(x => $"/mock-url-for-{x[0]}");

        _journeyState = new JourneyState
        {
            Children =
            {
                [ChildId] = new Child(ChildId, "Child A")
            }
        };
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new ExpectedChildDetailsController(_journeyState, _journeySession, navigationService);
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
        Assert.IsType<NotFoundResult>(_controller.ChildDueDate("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ChildDueDate_Get_PopulatesModel_FromState()
    {
        var child = _journeyState.GetChild(ChildId)!;
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

        Assert.IsType<RedirectResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(new DateOnly(2020, 1, 15), _journeyState.GetChild(model.ChildId)!.DueDate);
        Assert.True(_controller.ModelState.IsValid);


    }

    [Fact]
    public void ChildDueDate_Post_ValidSelection_SavesState_AndReturnsTo()
    {
        var model = new ChildDueDateViewModel
        {
            ChildId = ChildId,
            ChildDueDate = new DateOnly(2020, 1, 15),
            ReturnTo = ReturnTo.CheckChildDetails
        };

        var result = _controller.ChildDueDate(model);

        Assert.IsType<RedirectResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(new DateOnly(2020, 1, 15), _journeyState.GetChild(model.ChildId)!.DueDate);
        Assert.True(_controller.ModelState.IsValid);


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
        var result = Assert.IsType<ViewResult>(_controller.ExpectedChildRelationship(ChildId));
        Assert.Null(result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Fact]
    public void ExpectedChildRelationship_IfChildDoesNotExistReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(_controller.ExpectedChildRelationship("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ExpectedChildRelationship_Get_PopulatesModel_FromState()
    {
        var child = _journeyState.GetChild(ChildId)!;
        child.ExpectedRelationship = Relationship.Parent;
        var result = Assert.IsType<ViewResult>(_controller.ExpectedChildRelationship(ChildId));
        Assert.Equal(Relationship.Parent, result.Model<ExpectedChildRelationshipViewModel>().ExpectedChildRelationship);
    }

    [Theory]
    [InlineData(ReturnTo.CheckChildDetails)]
    [InlineData(ReturnTo.CheckAnswers)]
    public void ExpectedChildRelationship_Post_ValidSelection_SavesState_AndRedirects(string? returnTo)
    {
        var model = new ExpectedChildRelationshipViewModel
        {
            ChildId = ChildId,
            ExpectedChildRelationship = Relationship.Parent,
            ReturnTo = returnTo,
        };

        var result = _controller.ExpectedChildRelationship(model);

        Assert.IsType<RedirectResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(Relationship.Parent, _journeyState.GetChild(model.ChildId)!.ExpectedRelationship);
        Assert.True(_controller.ModelState.IsValid);


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
