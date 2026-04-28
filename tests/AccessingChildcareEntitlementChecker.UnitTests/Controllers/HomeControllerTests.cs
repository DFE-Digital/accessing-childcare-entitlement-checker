using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class HomeControllerTests
{

    private HomeController CreateController(FakeJourneySession session)
    {
        return new HomeController(
            new FakeStringLocalizerFactory(),
            session);
    }

    [Fact]
    public void Start_ReturnsView()
    {
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        var result = controller.Start();

        Assert.IsType<ViewResult>(result);
    }


    [Fact]
    public void WhereDoYouLive_Get_PopulatesModel_FromState()
    {
        var session = new FakeJourneySession();
        session.State.CountryOfResidence = CountryOfResidence.England;

        var controller = CreateController(session);

        var result = controller.Location();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LocationViewModel>(view.Model);

        Assert.Equal(CountryOfResidence.England, model.Country);
    }

    [Fact]
    public void WhereDoYouLive_Post_InvalidSelection_ReturnsViewWithError()
    {
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        var model = new LocationViewModel
        {
            Country = null
        };

        var result = controller.Location(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
    }

    [Fact]
    public void WhereDoYouLive_Post_ValidSelection_SavesState_AndRedirects()
    {
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        var model = new LocationViewModel
        {
            Country = CountryOfResidence.England
        };

        var result = controller.Location(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(CountryOfResidence.England, session.State.CountryOfResidence);
        Assert.Equal(nameof(UserController.HasPartner), redirect.ActionName);
    }

    [Fact]
    public void SessionExpired_ReturnsView()
    {
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        var result = controller.SessionExpired();

        Assert.IsType<ViewResult>(result);
    }
}