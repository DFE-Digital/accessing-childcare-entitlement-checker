using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class IntroductionController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public static string Name => "Introduction";

    public IntroductionController(JourneyState journeyState, IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public IActionResult ChildName(string? childId = null, string? returnTo = null)
    {
        var backLink = GetChildNameBackLink(childId, returnTo);
        if (childId == null)
        {
            var childNameViewModel = new ChildNameViewModel(null, backLink, returnTo);
            return View(childNameViewModel);
        }

        if (!_journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        return View(new ChildNameViewModel(child, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ChildName(ChildNameViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var backLink = GetChildNameBackLink(model.ChildId, model.ReturnTo);
            model.BackLink = backLink;
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        return this.RedirectTo<IntroductionController>(
            nameof(IsChildBorn),
            new { childId = model.ChildId, returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult IsChildBorn(string childId, string? returnTo = null)
    {
        if (!_journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        var backLink = GetIsChildBornBackLink(childId, returnTo);
        return View(new ChildIsBornViewModel(child, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult IsChildBorn(ChildIsBornViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetIsChildBornBackLink(model.ChildId, model.ReturnTo);
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

    private string? GetChildNameBackLink(string? childId, string? returnTo)
    {
        if (returnTo == ReturnTo.CheckAnswers)
        {
            return Url.Action(nameof(SummaryController.CheckAnswers), SummaryController.Name, new { childId });
        }

        if (returnTo == ReturnTo.CheckChildDetails)
        {
            return Url.Action(nameof(SummaryController.CheckChildDetails), SummaryController.Name, new { childId });
        }

        return Url.Action(nameof(HomeController.Location), HomeController.Name);
    }

    private string? GetIsChildBornBackLink(string childId, string? returnTo)
    {
        if (returnTo == ReturnTo.CheckAnswers)
        {
            return Url.Action(nameof(SummaryController.CheckAnswers), SummaryController.Name, new { childId });
        }

        if (returnTo == ReturnTo.CheckChildDetails)
        {
            return Url.Action(nameof(SummaryController.CheckChildDetails), SummaryController.Name, new { childId });
        }

        return Url.Action(nameof(ChildName), new { childId });
    }
}
