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

    public const string Name = "BornChildDetails";
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

        var backLink = GetChildBirthDateBackLink(childId, returnTo);
        return View(new ChildBirthDateViewModel(child, backLink, returnTo));
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
            model.BackLink = GetChildBirthDateBackLink(model.ChildId, model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        var nextAnswerMissing = child.BornRelationship == null;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(
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

        var backLink = GetChildRelationshipBackLink(childId, returnTo);
        return View(new ChildRelationshipViewModel(child, backLink, returnTo));
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
            model.BackLink = GetChildRelationshipBackLink(model.ChildId, model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAnswerMissing = child.ChildSupportOptions.Count == 0;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(
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

        var backLink = GetChildSupportBackLink(childId, returnTo);
        return View(new ChildSupportViewModel(child, backLink, returnTo));
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
            model.BackLink = GetChildSupportBackLink(model.ChildId, model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo is not null)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(
            nameof(SummaryController.CheckChildDetails),
            SummaryController.Name,
            new { childId = model.ChildId, returnTo = model.ReturnTo });
    }

    private string GetChildBirthDateBackLink(string childId, string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, childId, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(
            nameof(IntroductionController.IsChildBorn),
            IntroductionController.Name,
            new { childId });
    }

    private string GetChildRelationshipBackLink(string childId, string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, childId, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(ChildBirthDate), new { childId });
    }

    private string GetChildSupportBackLink(string childId, string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, childId, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(ChildRelationship), new { childId });
    }
}
