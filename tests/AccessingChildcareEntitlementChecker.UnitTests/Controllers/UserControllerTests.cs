using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.UnitTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class UserControllerTests
{
    private readonly FakeJourneySession _fakeJourneySession;
    private readonly UserController _sut;

    public UserControllerTests()
    {
        _fakeJourneySession = new FakeJourneySession();
        _sut = new UserController(
            new FakeStringLocalizerFactory(),
            _fakeJourneySession);
    }

    [Fact]
    public void HasPartner_ReturnsView()
    {
        var result = _sut.HasPartner();
        Assert.Null(result.Model<HasPartnerViewModel>().HasPartner);
    }

    [Fact]
    public void Age_ReturnsView()
    {
        var result = _sut.Age();
        Assert.Null(result.Model<AgeModel>().Age);
    }

    [Fact]
    public void HasPartner_Get_PopulatesModel_FromState()
    {
        _fakeJourneySession.State.HasPartner = true;
        var result = _sut.HasPartner();
        Assert.True(result.Model<HasPartnerViewModel>().HasPartner);
    }

    [Fact]
    public void Age_Get_PopulatesModel_FromState()
    {
        _fakeJourneySession.State.Age = Age.EighteenToTwenty;
        var result = _sut.Age();
        Assert.Equal(Age.EighteenToTwenty, result.Model<AgeModel>().Age);
    }

    [Fact]
    public void HasPartner_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new HasPartnerViewModel()
        {
            HasPartner = null,
        };

        _sut.HasPartner(model);
        Assert.False(_sut.ModelState.IsValid);
        Assert.True(_sut.ModelState.ContainsKey(nameof(model.HasPartner)));
    }

    [Fact]
    public void Age_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new AgeModel()
        {
            Age = null,
        };

        _sut.Age(model);
        Assert.False(_sut.ModelState.IsValid);
        Assert.True(_sut.ModelState.ContainsKey(nameof(model.Age)));
    }

    [Fact]
    public void HasPartner_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new HasPartnerViewModel()
        {
            HasPartner = true
        };

        var result = _sut.HasPartner(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(true, _fakeJourneySession.State.HasPartner);
        Assert.True(_sut.ModelState.IsValid);
        Assert.Equal(nameof(UserController.NextStepPlaceholder), redirect.ActionName);
    }

    [Fact]
    public void Age_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new AgeModel()
        {
            Age = Age.EighteenToTwenty
        };

        var result = _sut.Age(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(Age.EighteenToTwenty, _fakeJourneySession.State.Age);
        Assert.True(_sut.ModelState.IsValid);
        Assert.Equal(nameof(UserController.HowOldIsYourPartner), redirect.ActionName);
    }
}