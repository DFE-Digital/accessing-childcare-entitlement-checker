using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class BornChildDetailsController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public const string Nme = "BornChildDetails";
    public BornChildDetailsController(
        JourneyState journeyState,
        IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public IActionResult ChildBirthDate(string childId, string? returnTo = null)
    {
        if (!_journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        return View(new ChildBirthDateViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildBirthDate(ChildBirthDateViewModel model)
    {
        if (!_journeyState.Children.TryGetValue(model.ChildId, out var child))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.ChildName = child.Name;
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo is not null)
        {
            return this.RedirectToReturnTo(model.ReturnTo, model.ChildId);
        }

        return this.RedirectTo<BornChildDetailsController>(
            nameof(ChildRelationship),
            new { childId = model.ChildId, returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult ChildRelationship(string childId, string? returnTo = null)
    {
        if (!_journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        return View(new ChildRelationshipViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildRelationship(ChildRelationshipViewModel model)
    {
        if (!_journeyState.Children.TryGetValue(model.ChildId, out var child))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.ChildName = child.Name;
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo is not null)
        {
            return this.RedirectToReturnTo(model.ReturnTo, model.ChildId);
        }

        return this.RedirectTo<BornChildDetailsController>(
            nameof(ChildSupport),
            new { childId = model.ChildId, returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult ChildSupport(string childId, string? returnTo = null)
    {
        if (!_journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        return View(new ChildSupportViewModel(child) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildSupport(ChildSupportViewModel model)
    {
        if (!_journeyState.Children.TryGetValue(model.ChildId, out var child))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.ChildName = child.Name;
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return this.RedirectToReturnTo(model.ReturnTo ?? ReturnTo.CheckChildDetails, model.ChildId);
    }
}
