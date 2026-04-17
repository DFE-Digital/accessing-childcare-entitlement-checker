using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.UnitTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class UserControllerTests
{

    private UserController CreateController(FakeJourneySession session)
    {
        return new UserController(
            new FakeStringLocalizerFactory(),
            session);
    }

    [Fact]
    public void HasPartner_ReturnsView()
    {
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        var result = controller.HasPartner();

        var viewResult = Assert.IsType<ViewResult>(result);

        var model = Assert.IsType<HasPartnerViewModel>(viewResult.Model);
        Assert.Null(model.HasPartner);
    }

    [Fact]
    public void HasPartner_Get_PopulatesModel_FromState()
    {
        var session = new FakeJourneySession();
        session.State.HasPartner = true;

        var controller = CreateController(session);

        var result = controller.HasPartner();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<HasPartnerViewModel>(view.Model);

        Assert.True(model.HasPartner);
    }

    [Fact]
    public void HasPartner_Post_InvalidSelection_ReturnsViewWithError()
    {
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        var model = new HasPartnerViewModel()
        {
            HasPartner = null,
        };

        var result = controller.HasPartner(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
        Assert.True(controller.ModelState.ContainsKey(nameof(model.HasPartner)));
    }

    [Fact]
    public void HasPartner_Post_ValidSelection_SavesState_AndRedirects()
    {
        var session = new FakeJourneySession();
        var controller = CreateController(session);

        var model = new HasPartnerViewModel()
        {
            HasPartner = true
        };

        var result = controller.HasPartner(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(true, session.State.HasPartner);
        Assert.True(controller.ModelState.IsValid);
        Assert.Equal(nameof(UserController.NextStepPlaceholder), redirect.ActionName);
    }
}