using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class ExpectedChildDetailsController(
    JourneyState journeyState,
    IJourneySession journeySession,
    INavigationService navigationService)
    : Controller
{
    [HttpGet]
    public IActionResult ChildDueDate(string childId, string? returnTo = null)
    {
        var child = journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        return View(new ChildDueDateViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildDueDate(ChildDueDateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);

        return Redirect(navigationService.GetNextUrl(Page.ChildDueDate, model.ReturnTo, model.ChildId));
    }

    [HttpGet]
    public IActionResult ExpectedChildRelationship(string childId, string? returnTo = null)
    {
        var child = journeyState.GetChild(childId);
        if (child == null)
        {
            return NotFound();
        }

        return View(new ExpectedChildRelationshipViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ExpectedChildRelationship(ExpectedChildRelationshipViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);

        return Redirect(navigationService.GetNextUrl(Page.ExpectedChildRelationship, model.ReturnTo, model.ChildId));
    }
}
