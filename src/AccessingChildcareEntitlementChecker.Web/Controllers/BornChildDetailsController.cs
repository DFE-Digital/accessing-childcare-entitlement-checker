using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class BornChildDetailsController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public BornChildDetailsController(
        JourneyState journeyState,
        IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public IActionResult ChildBirthDate(string? childId = null, string? returnTo = null)
    {
        return View(new ChildBirthDateViewModel(childId, _journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildBirthDate(ChildBirthDateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo == "check-your-childrens-details")
        {
            return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails", new { fromChildId = model.ChildId });
        }

        return RedirectToAction(nameof(BornChildDetailsController.ChildRelationship), "BornChildDetails", new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult ChildRelationship(string? childId = null, string? returnTo = null)
    {
        return View(new ChildRelationshipViewModel(childId, _journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildRelationship(ChildRelationshipViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo == "check-your-childrens-details")
        {
            return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails", new { fromChildId = model.ChildId });
        }

        return RedirectToAction(nameof(BornChildDetailsController.ChildSupport), "BornChildDetails", new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult ChildSupport(string? childId = null, string? returnTo = null)
    {
        return View(new ChildSupportViewModel(childId, _journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildSupport(ChildSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo == "check-your-childrens-details")
        {
            return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails", new { fromChildId = model.ChildId });
        }

        return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails", new { fromChildId = model.ChildId });
    }
}
