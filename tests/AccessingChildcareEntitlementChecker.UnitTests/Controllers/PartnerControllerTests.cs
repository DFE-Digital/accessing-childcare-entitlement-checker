using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class PartnerControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly PartnerController _controller;

    public PartnerControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new PartnerController(_journeyState, _journeySession);
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
        _journeyState.PartnerAge = AgeRange.EighteenToTwenty;
        var result = _controller.PartnerAge();
        Assert.Equal(AgeRange.EighteenToTwenty, result.Model<PartnerAgeViewModel>().PartnerAge);
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
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(AgeRange.EighteenToTwenty, _journeyState.PartnerAge);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(UserController.NextStepPlaceholder), redirect.ActionName);
    }
}