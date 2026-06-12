using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class IntroductionController(
    JourneyState journeyState,
    IJourneySession journeySession,
    INavigationService navigationService)
    : Controller
{
    [HttpGet]
    public IActionResult ChildName(string? childId = null)
    {
        if (childId == null)
        {
            var childNameViewModel = new ChildNameViewModel();
            return View(childNameViewModel);
        }

        var child = journeyState.GetChild(childId);
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

        journeyState.Apply(model);
        journeySession.Set(journeyState);

        return Redirect(navigationService.GetNextUrl(Page.ChildName, childId: model.ChildId));
    }

    [HttpGet]
    public IActionResult IsChildBorn(string childId, string? returnTo = null)
    {
        var child = journeyState.GetChild(childId);
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
        journeyState.Apply(model);
        journeySession.Set(journeyState);

        return Redirect(navigationService.GetNextUrl(Page.IsChildBorn, model.ReturnTo, model.ChildId));
    }
}
