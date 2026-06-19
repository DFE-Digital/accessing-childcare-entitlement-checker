using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class ExpectedChildDetailsController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public const string Name = "ExpectedChildDetails";

    public ExpectedChildDetailsController(
        JourneyState journeyState,
        IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public IActionResult ChildDueDate(string childId, string? returnTo = null)
    {
        if (!_journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        var backLink = GetChildDueDateBackLink(childId, returnTo);
        return View(new ChildDueDateViewModel(child, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ChildDueDate(ChildDueDateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetChildDueDateBackLink(model.ChildId, model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo is not null)
        {
            return this.RedirectToReturnTo(model.ReturnTo, model.ChildId);
        }

        return this.RedirectTo<ExpectedChildDetailsController>(
            nameof(ExpectedChildRelationship),
            new { childId = model.ChildId, returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult ExpectedChildRelationship(string childId, string? returnTo = null)
    {
        if (!_journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        var backLink = GetExpectedChildRelationshipBackLink(childId, returnTo);
        return View(new ExpectedChildRelationshipViewModel(child, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ExpectedChildRelationship(ExpectedChildRelationshipViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetExpectedChildRelationshipBackLink(model.ChildId, model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return this.RedirectToReturnTo(model.ReturnTo ?? ReturnTo.CheckChildDetails, model.ChildId);
    }

    private string GetChildDueDateBackLink(string childId, string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, childId, out var url))
        {
            return url;
        }

        return this.Url.Action(nameof(IntroductionController.IsChildBorn), IntroductionController.Name, new { childId })
            ?? throw new InvalidOperationException("Unable to generate back link");
    }

    private string GetExpectedChildRelationshipBackLink(string childId, string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, childId, out var url))
        {
            return url;
        }

        return this.Url.Action(nameof(ChildDueDate), Name, new { childId })
            ?? throw new InvalidOperationException("Unable to generate back link");
    }
}
