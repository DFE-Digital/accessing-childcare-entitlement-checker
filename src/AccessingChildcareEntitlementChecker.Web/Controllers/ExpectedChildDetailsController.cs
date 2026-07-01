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
        if (!_journeyState.Children.TryGetValue(model.ChildId, out var child))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.BackLink = GetChildDueDateBackLink(model.ChildId, model.ReturnTo);
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

    private string GetChildDueDateBackLink(string childId, string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, childId, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(IntroductionController.IsChildBorn), IntroductionController.Name, new { childId });
    }
}
