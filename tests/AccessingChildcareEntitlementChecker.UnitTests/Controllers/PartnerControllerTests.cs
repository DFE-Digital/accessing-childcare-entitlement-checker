using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.UnitTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class PartnerControllerTests
{
    private readonly FakeJourneySession _fakeJourneySession;
    private readonly PartnerController _controller;

    public PartnerControllerTests()
    {
        _fakeJourneySession = new FakeJourneySession();
        _controller = new PartnerController(
            new FakeStringLocalizerFactory(),
            _fakeJourneySession);
    }

    [Fact]
    public void PartnerAge_ReturnsView()
    {
        var result = _controller.PartnerAge();
        Assert.Null(result.Model<PartnerAgeViewModel>().PartnerAge);
    }

    [Fact]
    public void PartnerAge_Get_PopulatesModel_FromState()
    {
        _fakeJourneySession.State.PartnerAge = AgeRange.EighteenToTwenty;
        var result = _controller.PartnerAge();
        Assert.Equal(AgeRange.EighteenToTwenty, result.Model<PartnerAgeViewModel>().PartnerAge);
    }

    [Fact]
    public void PartnerAge_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerAgeViewModel()
        {
            PartnerAge = null,
        };

        _controller.PartnerAge(model);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerAge)));
    }

    [Fact]
    public void PartnerAge_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new PartnerAgeViewModel()
        {
            PartnerAge = AgeRange.EighteenToTwenty
        };

        var result = _controller.PartnerAge(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(AgeRange.EighteenToTwenty, _fakeJourneySession.State.PartnerAge);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(UserController.NextStepPlaceholder), redirect.ActionName);
    }


}