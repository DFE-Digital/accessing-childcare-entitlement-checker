using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

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
        _journeyState.ChildName = "Child A";
        var result = _controller.ChildBirthDate();
        Assert.Null(result.Model<ChildBirthDateViewModel>().ChildBirthDate);
        Assert.Equal("Child A", result.Model<ChildBirthDateViewModel>().ChildName);
    }

    [Fact]
    public void ChildBirthDate_Get_PopulatesModel_FromState()
    {
        _journeyState.ChildBirthDate = new DateOnly(2020, 1, 15);
        _journeyState.ChildName = "Child A";
        var result = _controller.ChildBirthDate();
        Assert.Equal(new DateOnly(2020, 1, 15), result.Model<ChildBirthDateViewModel>().ChildBirthDate);
        Assert.Equal("Child A", result.Model<ChildBirthDateViewModel>().ChildName);
    }

    [Fact]
    public void ChildBirthDate_Post_ValidSelection_SavesState_AndRedirects()
    {
        var birthDate = new DateOnly(2020, 1, 15);
        var model = new ChildBirthDateViewModel()
        {
            ChildBirthDate = birthDate,
        };

        var result = _controller.ChildBirthDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(birthDate, _journeyState.ChildBirthDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(BornChildDetailsController.ChildRelationship), redirect.ActionName);
        Assert.Equal("BornChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildBirthDate_Post_InvalidSelection_ReturnsViewWithError()
    {
        _journeyState.ChildName = "Child A";
        var model = new ChildBirthDateViewModel();

        _controller.ModelState.AddModelError(nameof(model.ChildBirthDate), "Faked Model Binding Error");

        var result = _controller.ChildBirthDate(model);

        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<ChildBirthDateViewModel>(view.Model);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildBirthDate)));
        Assert.Equal("Child A", viewModel.ChildName);
    }

    [Fact]
    public void ChildBirthDate_Post_NoChildName_ReturnsViewWithError()
    {
        _journeyState.ChildName = null;
        var model = new ChildBirthDateViewModel();
        _controller.ModelState.AddModelError(nameof(model.ChildBirthDate), "Faked Model Binding Error");
        Assert.Throws<InvalidOperationException>(() => _controller.ChildBirthDate(model));
    }

    [Fact]
    public void ChildRelationship_ReturnsView()
    {
        _journeyState.ChildName = "Child A";
        var result = _controller.ChildRelationship();
        Assert.Null(result.Model<ChildRelationshipViewModel>().Relationship);
        Assert.Equal("Child A", result.Model<ChildRelationshipViewModel>().ChildName);
    }

    [Fact]
    public void ChildRelationship_Get_PopulatesModel_FromState()
    {
        _journeyState.Relationship = Relationship.Parent;
        _journeyState.ChildName = "Child A";
        var result = _controller.ChildRelationship();
        Assert.Equal(Relationship.Parent, result.Model<ChildRelationshipViewModel>().Relationship);
        Assert.Equal("Child A", result.Model<ChildRelationshipViewModel>().ChildName);
    }

    [Fact]
    public void ChildRelationship_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildRelationshipViewModel()
        {
            Relationship = Relationship.Parent,
        };

        var result = _controller.ChildRelationship(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(Relationship.Parent, _journeyState.Relationship);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(BornChildDetailsController.ChildSupport), redirect.ActionName);
        Assert.Equal("BornChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildRelationship_Post_InvalidSelection_ReturnsViewWithError()
    {
        _journeyState.ChildName = "Child A";
        var model = new ChildRelationshipViewModel();

        _controller.ModelState.AddModelError(nameof(model.Relationship), "Faked Model Binding Error");

        var result = _controller.ChildRelationship(model);

        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<ChildRelationshipViewModel>(view.Model);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.Relationship)));
        Assert.Equal("Child A", viewModel.ChildName);
    }

    [Fact]
    public void ChildRelationship_Post_NoChildName_ReturnsViewWithError()
    {
        _journeyState.ChildName = null;
        var model = new ChildRelationshipViewModel();
        _controller.ModelState.AddModelError(nameof(model.Relationship), "Faked Model Binding Error");
        Assert.Throws<InvalidOperationException>(() => _controller.ChildRelationship(model));
    }

    [Fact]
    public void ChildSupport_ReturnsView()
    {
        _journeyState.ChildName = "Child A";
        var result = _controller.ChildSupport();
        Assert.Equal([], result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Fact]
    public void ChildSupport_Get_PopulatesModel_FromState()
    {
        _journeyState.ChildSupportOptions = [ChildSupport.PersonalIndependencePayment];
        _journeyState.ChildName = "Child A";
        var result = _controller.ChildSupport();
        Assert.Equal(new[] { ChildSupport.PersonalIndependencePayment }, result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Fact]
    public void ChildSupport_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildSupportViewModel
        {
            ChildSupportOptions = [ChildSupport.PersonalIndependencePayment],
        };

        var result = _controller.ChildSupport(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(new[] { ChildSupport.PersonalIndependencePayment }, _journeyState.ChildSupportOptions);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(CheckChildDetailsController.CheckChildDetails), redirect.ActionName);
        Assert.Equal("CheckChildDetails", redirect.ControllerName);
    }

    [Fact]
    public void ChildSupport_Post_InvalidSelection_ReturnsViewWithError()
    {
        _journeyState.ChildName = "Child A";
        var model = new ChildSupportViewModel();

        _controller.ModelState.AddModelError(nameof(model.ChildSupportOptions), "Faked Model Binding Error");

        var result = _controller.ChildSupport(model);

        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<ChildSupportViewModel>(view.Model);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildSupportOptions)));
        Assert.Equal("Child A", viewModel.ChildName);
    }

    [Fact]
    public void ChildSupport_Post_NoChildName_ReturnsViewWithError()
    {
        _journeyState.ChildName = null;
        var model = new ChildSupportViewModel();
        _controller.ModelState.AddModelError(nameof(model.ChildSupportOptions), "Faked Model Binding Error");
        Assert.Throws<InvalidOperationException>(() => _controller.ChildSupport(model));
    }
}
