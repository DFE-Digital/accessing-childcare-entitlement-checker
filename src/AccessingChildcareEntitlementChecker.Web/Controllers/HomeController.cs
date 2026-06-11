using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using static AccessingChildcareEntitlementChecker.Web.IServiceCollectionExtensions;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class HomeController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly Journey _journey;

    public HomeController(
        JourneyState journeyState,
        IJourneySession journeySession,
        Journey journey)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
        _journey = journey;
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
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new LocationViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult Location(LocationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }
}
