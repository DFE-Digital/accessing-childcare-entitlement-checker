using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using NSubstitute;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.FeatureManagement;
using AccessingChildcareEntitlementChecker.Web;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class HomeControllerTests : IDisposable
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
    public void StartReturnsView()
    {
        var result = _controller.Start();
        Assert.IsType<ViewResult>(result);
    }


    [Fact]
    public async Task LocationGetPopulatesModelFromStateWhenFeatureFlagDisabled()
    {
        _featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration).Returns(false);
        _journeyState.CountryOfResidence = CountryOfResidence.England;
        var result = await _controller.Location();
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LocationViewModel>(viewResult.Model);
        Assert.Equal(CountryOfResidence.England, model.Country);
    }

    [Fact]
    public async Task LocationGetRedirectsAndSetsEnglandWhenFeatureFlagEnabled()
    {
        _featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration).Returns(true);
        var result = await _controller.Location();
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(CountryOfResidence.England, _journeyState.CountryOfResidence);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(nameof(IntroductionController.ChildName), redirectResult.ActionName);
        Assert.Equal(IntroductionController.Name, redirectResult.ControllerName);
    }

    [Fact]
    public void LocationPostValidSelectionSavesStateAndRedirects()
    {
        var model = new LocationViewModel
        {
            Country = CountryOfResidence.England
        };

        var result = _controller.Location(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(CountryOfResidence.England, _journeyState.CountryOfResidence);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(IntroductionController.ChildName), redirect.ActionName);
    }

    [Fact]
    public void LocationPostValidSelectionWithExistingChildrenRedirects()
    {
        _journeyState.Children["child1"] = new Child("child1", "Child 1");
        var model = new LocationViewModel
        {
            Country = CountryOfResidence.England
        };

        var result = _controller.Location(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(CountryOfResidence.England, _journeyState.CountryOfResidence);
        Assert.True(_controller.ModelState.IsValid);

        // We should navigate to the child details page if the user already has children
        Assert.Equal(nameof(SummaryController.CheckChildDetails), redirect.ActionName);
    }

    [Fact]
    public void LocationPostInvalidSelectionReturnsViewWithError()
    {
        var model = new LocationViewModel
        {
            Country = null
        };

        _controller.ModelState.AddModelError(nameof(model.Country), "Faked Model Binding Error");

        var result = _controller.Location(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.Country)));
    }

    [Fact]
    public void SessionExpiredReturnsView()
    {
        var result = _controller.SessionExpired();
        Assert.IsType<ViewResult>(result);
    }

    public void Dispose() { _controller?.Dispose(); GC.SuppressFinalize(this); }
}
