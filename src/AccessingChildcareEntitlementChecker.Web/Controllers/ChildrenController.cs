using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class ChildrenController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public ChildrenController(
        JourneyState journeyState,
        IJourneySession journeySession)
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

        if (model.ChildId == null)
        {
            var childState = new ChildState(model.ChildName!);
            _journeyState.AddChild(childState);
        }
        else
        {
            if (!_journeyState.TryGetChild(model.ChildId, out var child))
            {
                return NotFound();
            }

            child.Apply(model);
        }
        
        _journeySession.Set(_journeyState);

        return this.RedirectTo<ChildrenController>(
            nameof(ChildIsBorn),
            new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult ChildIsBorn(string childId, string? returnTo = null)
    {
        if (!_journeyState.TryGetChild(childId, out var child))
        {
            return NotFound();
        }

        return View(new ChildIsBornViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildIsBorn(ChildIsBornViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        if (!_journeyState.TryGetChild(model.ChildId, out var child))
        {
            return NotFound();
        }

        child.Apply(model);
        _journeySession.Set(_journeyState);

        if (model.ChildIsBorn == BirthStatus.Born)
        {
            return this.RedirectTo<ChildrenController>(
                nameof(ChildrenController.ChildBirthDate),
                new { childId = model.ChildId });
        }

        return this.RedirectTo<ChildrenController>(
            nameof(ChildrenController.ChildDueDate),
            new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult ChildBirthDate(string childId, string? returnTo = null)
    {
        if (!_journeyState.TryGetChild(childId, out var child))
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

        if (!_journeyState.TryGetChild(model.ChildId, out var child))
        {
            return NotFound();
        }

        child.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo == "check-your-childrens-details")
        {
            return this.RedirectTo<CheckChildDetailsController>(
                nameof(CheckChildDetailsController.CheckChildDetails),
                new { fromChildId = model.ChildId });
        }

        return this.RedirectTo<ChildrenController>(
            nameof(ChildrenController.ChildRelationship),
            new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult ChildRelationship(string childId, string? returnTo = null)
    {
        if (!_journeyState.TryGetChild(childId, out var child))
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

        if (!_journeyState.TryGetChild(model.ChildId, out var child))
        {
            return NotFound();
        }

        child.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo == "check-your-childrens-details")
        {
            return this.RedirectTo<CheckChildDetailsController>(
                nameof(CheckChildDetailsController.CheckChildDetails),
                new { fromChildId = model.ChildId });
        }

        return this.RedirectTo<ChildrenController>(
            nameof(ChildrenController.ChildSupport),
            new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult ChildSupport(string childId, string? returnTo = null)
    {
        if (!_journeyState.TryGetChild(childId, out var child))
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

        if (!_journeyState.TryGetChild(model.ChildId, out var child))
        {
            return NotFound();
        }

        child.Apply(model);
        _journeySession.Set(_journeyState);
        return this.RedirectTo<CheckChildDetailsController>(
            nameof(CheckChildDetailsController.CheckChildDetails),
            new { fromChildId = model.ChildId });
    }

[HttpGet]
    public IActionResult ChildDueDate(string childId, string? returnTo = null)
    {
        if (!_journeyState.TryGetChild(childId, out var child))
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

        if (!_journeyState.TryGetChild(model.ChildId, out var child))
        {
            return NotFound();
        }

        child.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo == "check-your-childrens-details")
        {
            return this.RedirectTo<CheckChildDetailsController>(
                nameof(CheckChildDetailsController.CheckChildDetails),
                new { fromChildId = model.ChildId });
        }

        return this.RedirectTo<ChildrenController>(
            nameof(ChildrenController.ExpectedChildRelationship),
            new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult ExpectedChildRelationship(string childId, string? returnTo = null)
    {
        if (!_journeyState.TryGetChild(childId, out var child))
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

        if (!_journeyState.TryGetChild(model.ChildId, out var child))
        {
            return NotFound();
        }

        child.Apply(model);
        _journeySession.Set(_journeyState);
        return this.RedirectTo<CheckChildDetailsController>(
            nameof(CheckChildDetailsController.CheckChildDetails),
            new { fromChildId = model.ChildId });
    }
}
