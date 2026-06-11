using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using static AccessingChildcareEntitlementChecker.Web.IServiceCollectionExtensions;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class IntroductionController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly Journey _journey;

    public IntroductionController(
        JourneyState journeyState,
        IJourneySession journeySession,
        Journey journey)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
        _journey = journey;
    }

    [HttpGet]
    public IActionResult ChildName(string? childId = null)
    {
        if (childId == null)
        {
            var childNameViewModel = new ChildNameViewModel();
            return View(childNameViewModel);
        }

        var child = _journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        return View(new ChildNameViewModel(child));
    }

    [HttpPost]
    public IActionResult ChildName(ChildNameViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        return _journey.Forwards(this, _journeyState, new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult IsChildBorn(string childId, string? returnTo = null)
    {
        var child = _journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        return View(new ChildIsBornViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult IsChildBorn(ChildIsBornViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        return _journey.Forwards(this, _journeyState);
    }
}
