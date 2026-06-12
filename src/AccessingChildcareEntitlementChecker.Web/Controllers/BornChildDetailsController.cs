using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using static AccessingChildcareEntitlementChecker.Web.IServiceCollectionExtensions;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class BornChildDetailsController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly Journey _journey;

    public BornChildDetailsController(
        JourneyState journeyState,
        IJourneySession journeySession,
        Journey journey)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
        _journey = journey;
    }

    [HttpGet]
    public IActionResult ChildBirthDate(string childId, string? returnTo = null)
    {
        var child = _journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new ChildBirthDateViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildBirthDate(ChildBirthDateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { childId = model.ChildId, returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { childId = model.ChildId, returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult ChildRelationship(string childId, string? returnTo = null)
    {
        var child = _journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new ChildRelationshipViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildRelationship(ChildRelationshipViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { childId = model.ChildId, returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { childId = model.ChildId, returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult ChildSupport(string childId, string? returnTo = null)
    {
        var child = _journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new ChildSupportViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildSupport(ChildSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { childId = model.ChildId, returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { childId = model.ChildId, returnTo = model.ReturnTo });
    }
}
