using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class BornChildDetailsController(
    JourneyState journeyState,
    IJourneySession journeySession,
    INavigationService navigationService)
    : Controller
{
    [HttpGet]
    public IActionResult ChildBirthDate(string childId, string? returnTo = null)
    {
        var child = journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        return View(new ChildBirthDateViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildBirthDate(ChildBirthDateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);

        return Redirect(navigationService.GetNextUrl(Page.ChildBirthDate, model.ReturnTo, model.ChildId));
    }

    [HttpGet]
    public IActionResult ChildRelationship(string childId, string? returnTo = null)
    {
        var child = journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        return View(new ChildRelationshipViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildRelationship(ChildRelationshipViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);

        return Redirect(navigationService.GetNextUrl(Page.ChildRelationship, model.ReturnTo, model.ChildId));
    }

    [HttpGet]
    public IActionResult ChildSupport(string childId, string? returnTo = null)
    {
        var child = journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        return View(new ChildSupportViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildSupport(ChildSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);

        return Redirect(navigationService.GetNextUrl(Page.ChildSupport, model.ReturnTo, model.ChildId));
    }
}
