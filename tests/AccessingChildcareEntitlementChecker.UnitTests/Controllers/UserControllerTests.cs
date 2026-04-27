using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Services;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class UserControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new UserController(_journeyState, _journeySession);
    }

    [Fact]
    public void HasPartner_ReturnsView()
    {
        var result = _controller.HasPartner();
        Assert.Null(result.Model<HasPartnerViewModel>().HasPartner);
    }

    [Fact]
    public void HasPartner_Get_PopulatesModel_FromState()
    {
        _journeyState.HasPartner = true;
        var result = _controller.HasPartner();
        Assert.True(result.Model<HasPartnerViewModel>().HasPartner);
    }

    [Theory]
    [InlineData(true, nameof(PartnerController.PartnerAge))]
    [InlineData(false, nameof(UserController.NextStepPlaceholder))]
    public void HasPartner_Post_ValidSelection_SavesState_AndRedirects(bool hasPartner, string redirectsTo)
    {
        var model = new HasPartnerViewModel()
        {
            HasPartner = hasPartner,
        };

        var result = _controller.HasPartner(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(hasPartner, _journeyState.HasPartner);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(redirectsTo, redirect.ActionName);
    }

    [Fact]
    public void HasPartner_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new HasPartnerViewModel
        {
            HasPartner = null
        };

        _controller.ModelState.AddModelError(nameof(model.HasPartner), "Faked Model Binding Error");

        var result = _controller.HasPartner(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.HasPartner)));
    }

    [Fact]
    public void UserAge_ReturnsView()
    {
        var result = _controller.UserAge();
        Assert.Null(result.Model<UserAgeViewModel>().UserAge);
    }

    [Fact]
    public void UserAge_Get_PopulatesModel_FromState()
    {
        _journeyState.UserAge = AgeRange.EighteenToTwenty;
        var result = _controller.UserAge();
        Assert.Equal(AgeRange.EighteenToTwenty, result.Model<UserAgeViewModel>().UserAge);
    }
    
    [Fact]
    public void ChildDateOfBirth_ReturnsView()
    {
        var result = _controller.ChildDateOfBirth();
        Assert.Null(result.Model<ChildDateOfBirthViewModel>().DateOfBirth);
    }

    [Fact]
    public void ChildDateOfBirth_Get_PopulatesModel_FromState()
    {
        var dateOfBirth = new DateTime(2020, 3, 31);
        _journeyState.ChildDateOfBirth = dateOfBirth;
        _journeyState.HasPartner = true;

        var result = _controller.ChildDateOfBirth();

        Assert.Equal(dateOfBirth, result.Model<ChildDateOfBirthViewModel>().DateOfBirth);
        Assert.True(result.Model<ChildDateOfBirthViewModel>().HasPartner);
    }

    [Fact]
    public void HasPartner_Post_NoPartner_SavesState_AndRedirectsToChildDateOfBirth()
    {
        var model = new HasPartnerViewModel()
        {
            HasPartner = false
        };

        var result = _controller.HasPartner(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.False(_journeyState.HasPartner);
        Assert.Equal(nameof(UserController.ChildDateOfBirth), redirect.ActionName);
    }

    [Fact]
    public void UserAge_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new UserAgeViewModel()
        {
            UserAge = AgeRange.EighteenToTwenty
        };

        var result = _controller.UserAge(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(AgeRange.EighteenToTwenty, _journeyState.UserAge);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(PartnerController.PartnerAge), redirect.ActionName);
    }

    [Fact]
    public void UserAge_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new UserAgeViewModel
        {
            UserAge = null
        };

        _controller.ModelState.AddModelError(nameof(model.UserAge), "Faked Model Binding Error");

        var result = _controller.UserAge(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.UserAge)));
    }

    [Fact]
    public void ChildDateOfBirth_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new ChildDateOfBirthViewModel()
        {
            DateOfBirth = DateTime.Today.AddDays(-1)
        };

        var result = _controller.ChildDateOfBirth(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(DateTime.Today.AddDays(-1), _journeyState.ChildDateOfBirth);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(UserController.NextStepPlaceholder), redirect.ActionName);
    }
}
