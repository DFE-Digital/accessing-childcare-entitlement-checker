using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.UnitTests.Helpers;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class EntitlementControllerTests
{
    
    private EntitlementController CreateController(FakeJourneySession session)
    {
        return new EntitlementController(
            new FakeStringLocalizerFactory(),
            session);
    }
    
    [Fact]
    public void WhereDoYouLive_Get_PopulatesModel_FromState()
    {
        var session = new FakeJourneySession();
        session.State.CountryOfResidence = CountryOfResidence.England;

        var controller = CreateController(session);

        var result = controller.WhereDoYouLive();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<WhereDoYouLiveViewModel>(view.Model);

        Assert.Equal(CountryOfResidence.England, model.Country);
    }
    
    [Fact]
    public void WhereDoYouLive_Post_InvalidSelection_ReturnsViewWithError()
    {
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        var model = new WhereDoYouLiveViewModel
        {
            Country = null
        };

        var result = controller.WhereDoYouLive(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
    }
    
    [Fact]
    public void WhereDoYouLive_Post_ValidSelection_SavesState_AndRedirects()
    {
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        var model = new WhereDoYouLiveViewModel
        {
            Country = CountryOfResidence.England
        };

        var result = controller.WhereDoYouLive(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(CountryOfResidence.England, session.State.CountryOfResidence);
        Assert.Equal(nameof(EntitlementController.PlaceholderNextStep), redirect.ActionName);
    }
    
}