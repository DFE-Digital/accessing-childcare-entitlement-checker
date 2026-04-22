using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.UnitTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class UserControllerTests
{
    private readonly FakeJourneySession _fakeJourneySession;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _fakeJourneySession = new FakeJourneySession();
        _controller = new UserController(
            new FakeStringLocalizerFactory(),
            _fakeJourneySession);
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
        _fakeJourneySession.State.HasPartner = true;
        var result = _controller.HasPartner();
        Assert.True(result.Model<HasPartnerViewModel>().HasPartner);
    }

    [Fact]
    public void UserAge_Get_PopulatesModel_FromState()
    {
        _fakeJourneySession.State.UserAge = AgeRange.EighteenToTwenty;
        var result = _controller.UserAge();
        Assert.Equal(AgeRange.EighteenToTwenty, result.Model<UserAgeViewModel>().UserAge);
    }

    [Fact]
    public void HasPartner_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new HasPartnerViewModel()
        {
            HasPartner = null,
        };

        _controller.HasPartner(model);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.HasPartner)));
    }

    [Fact]
    public void UserAge_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new UserAgeViewModel()
        {
            UserAge = null,
        };

        _controller.UserAge(model);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.UserAge)));
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
        Assert.Equal(true, _fakeJourneySession.State.HasPartner);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(UserController.NextStepPlaceholder), redirect.ActionName);
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
        Assert.Equal(AgeRange.EighteenToTwenty, _fakeJourneySession.State.UserAge);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(PartnerController.PartnerAge), redirect.ActionName);
    }
}