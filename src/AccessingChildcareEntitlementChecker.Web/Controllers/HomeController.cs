using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class HomeController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public HomeController(JourneyState journeyState, IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
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
    public ViewResult Location(string? returnTo = null)
    {
        var backLink = Url.Action(nameof(Start), new { returnTo });
        return View(new LocationViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult Location(LocationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.Action(nameof(Start), new { returnTo = model.ReturnTo });
            model.ReturnTo = model.ReturnTo;
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return RedirectToAction(nameof(IntroductionController.ChildName), "Introduction");
    }
}
