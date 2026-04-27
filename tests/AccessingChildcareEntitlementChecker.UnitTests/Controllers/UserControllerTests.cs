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
    public void UserAge_ReturnsView()
    {
        var result = _controller.UserAge();
        Assert.Null(result.Model<UserAgeViewModel>().UserAge);
    }

    [Fact]
    public void HasPartner_Get_PopulatesModel_FromState()
    {
        _journeyState.HasPartner = true;
        var result = _controller.HasPartner();
        Assert.True(result.Model<HasPartnerViewModel>().HasPartner);
    }

    [Fact]
    public void UserAge_Get_PopulatesModel_FromState()
    {
        _journeyState.UserAge = AgeRange.EighteenToTwenty;
        var result = _controller.UserAge();
        Assert.Equal(AgeRange.EighteenToTwenty, result.Model<UserAgeViewModel>().UserAge);
    }

    [Fact]
    public void HasPartner_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new HasPartnerViewModel()
        {
            HasPartner = true
        };

        var result = _controller.HasPartner(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(true, _journeyState.HasPartner);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(PartnerController.PartnerAge), redirect.ActionName);
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
}