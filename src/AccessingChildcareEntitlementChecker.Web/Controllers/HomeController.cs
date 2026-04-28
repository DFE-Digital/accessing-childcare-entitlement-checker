using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class HomeController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly JourneyActions _journeyActions;

    public HomeController(JourneyState journeyState, JourneyActions.Factory journeyActionsFactory)
    {
        _journeyState = journeyState;
        _journeyActions = journeyActionsFactory.Create(this);
    }

    [HttpGet]
    public IActionResult SessionExpired()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Start()
    {
        return View();
    }

    [HttpGet]
    public ViewResult Location()
    {
        return View(new LocationViewModel(_journeyState));
    }

    [HttpPost]
    public IActionResult Location(LocationViewModel model)
    {
        return _journeyActions.HandlePost(
            model,
            (state) => state.Apply(model),
            (UserController c) => c.HasPartner());
    }
}