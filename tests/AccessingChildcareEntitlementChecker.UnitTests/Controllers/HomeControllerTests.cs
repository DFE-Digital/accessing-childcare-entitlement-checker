using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class HomeControllerTests
{
    private JourneyState _journeyState;
    private IJourneySession _journeySession;
    private HomeController _controller;

    public HomeControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new HomeController(_journeyState, _journeySession);
    }

    [Fact]
    public void Start_ReturnsView()
    {
        var result = _controller.Start();
        Assert.IsType<ViewResult>(result);
    }


    [Fact]
    public void Location_Get_PopulatesModel_FromState()
    {
        _journeyState.CountryOfResidence = CountryOfResidence.England;
        var result = _controller.Location();
        Assert.Equal(CountryOfResidence.England, result.Model<LocationViewModel>().Country);
    }

    [Fact]
    public void Location_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new LocationViewModel
        {
            Country = CountryOfResidence.England
        };

        var result = _controller.Location(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(CountryOfResidence.England, _journeyState.CountryOfResidence);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(IntroductionController.ChildName), redirect.ActionName);
    }

    [Fact]
    public void Location_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new LocationViewModel
        {
            Country = null
        };

        _controller.ModelState.AddModelError(nameof(model.Country), "Faked Model Binding Error");

        var result = _controller.Location(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.Country)));
    }

    [Fact]
    public void SessionExpired_ReturnsView()
    {
        var result = _controller.SessionExpired();
        Assert.IsType<ViewResult>(result);
    }
}
