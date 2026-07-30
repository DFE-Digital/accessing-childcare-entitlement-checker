using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using NSubstitute;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;
using AccessingChildcareEntitlementChecker.Web;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class HomeControllerTests
{
    private JourneyState _journeyState;
    private IJourneySession _journeySession;
    private IFeatureManager _featureManager;
    private HomeController _controller;

    public HomeControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _featureManager = Substitute.For<IFeatureManager>();
        _controller = new HomeController(_journeyState, _journeySession, _featureManager);
        _controller.Url = Substitute.For<IUrlHelper>();
        _controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("backlink");
    }

    [Fact]
    public void Start_ReturnsView()
    {
        var result = _controller.Start();
        Assert.IsType<ViewResult>(result);
    }


    [Fact]
    public async Task Location_Get_PopulatesModel_FromState_WhenFeatureFlagDisabled()
    {
        _featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration).Returns(false);
        _journeyState.CountryOfResidence = CountryOfResidence.England;
        var result = await _controller.Location();
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LocationViewModel>(viewResult.Model);
        Assert.Equal(CountryOfResidence.England, model.Country);
    }

    [Fact]
    public async Task Location_Get_RedirectsAndSetsEngland_WhenFeatureFlagEnabled()
    {
        _featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration).Returns(true);
        var result = await _controller.Location();
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(CountryOfResidence.England, _journeyState.CountryOfResidence);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(nameof(IntroductionController.ChildName), redirectResult.ActionName);
        Assert.Equal(IntroductionController.Name, redirectResult.ControllerName);
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
