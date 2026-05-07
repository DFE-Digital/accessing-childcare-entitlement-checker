using AccessingChildcareEntitlementChecker.Web.Controllers;
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
        _journeyState.ChildBirthDate = new DateTime(2020, 1, 15);
        _journeyState.ChildName = "Child A";
        var result = _controller.ChildBirthDate();
        Assert.Equal(new DateTime(2020, 1, 15), result.Model<ChildBirthDateViewModel>().ChildBirthDate);
        Assert.Equal("Child A", result.Model<ChildBirthDateViewModel>().ChildName);
    }

    [Fact]
    public void ChildBirthDate_Post_ValidSelection_SavesState_AndRedirects()
    {
        var birthDate = new DateTime(2020, 1, 15);
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
}
