using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class IntroductionController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public IntroductionController(JourneyState journeyState, IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public IActionResult ChildName(string? childId = null)
    {
        if (childId == null)
        {
            var childNameViewModel = new ChildNameViewModel();
            return View(childNameViewModel);
        }

        if (!_journeyState.TryGetChild(childId, out var child))
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

        return this.RedirectTo<IntroductionController>(
            nameof(IsChildBorn),
            new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult IsChildBorn(string childId, string? returnTo = null)
    {
        if (!_journeyState.TryGetChild(childId, out var child))
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

        if (model.ChildIsBorn == BirthStatus.Born)
        {
            return this.RedirectTo<BornChildDetailsController>(
                nameof(BornChildDetailsController.ChildBirthDate),
                new { childId = model.ChildId, returnTo = model.ReturnTo });
        }

        return this.RedirectTo<ExpectedChildDetailsController>(
            nameof(ExpectedChildDetailsController.ChildDueDate),
            new { childId = model.ChildId, returnTo = model.ReturnTo });
    }
}
