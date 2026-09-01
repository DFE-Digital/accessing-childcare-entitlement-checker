using Dfe.Acec.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dfe.Acec.Web.Filters;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Microsoft.FeatureManagement;

namespace Dfe.Acec.Web.Controllers;

[ServiceFilter(typeof(RequireJourneySessionFilter))]
public class IntroductionController(JourneyState journeyState, IJourneySession journeySession, IFeatureManager featureManager) : Controller
{
    public const string Name = "Introduction";

    [HttpGet]
    public async Task<IActionResult> ChildName(string? childId = null, string? returnTo = null)
    {
        var backLink = await GetChildNameBackLink(childId, returnTo);
        if (childId == null)
        {
            var childNameViewModel = new ChildNameViewModel(null, backLink, returnTo);
            return View(childNameViewModel);
        }

        if (!journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        return View(new ChildNameViewModel(child, backLink, returnTo));
    }

    [HttpPost]
    public async Task<IActionResult> ChildName(ChildNameViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var backLink = await GetChildNameBackLink(model.ChildId, model.ReturnTo);
            model.BackLink = backLink;
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);

        return this.RedirectTo<IntroductionController>(
            nameof(IsChildBorn),
            new { childId = model.ChildId });
    }

    [HttpGet]
    public IActionResult IsChildBorn(string childId, string? returnTo = null)
    {
        if (!journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        var backLink = GetIsChildBornBackLink(childId, returnTo);
        return View(new ChildIsBornViewModel(child, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult IsChildBorn(ChildIsBornViewModel model)
    {
        if (!journeyState.Children.TryGetValue(model.ChildId, out var child))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.BackLink = GetIsChildBornBackLink(model.ChildId, model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);

        var (nextAction, nextController) = child.BirthStatus switch
        {
            BirthStatus.Born => (nameof(BornChildDetailsController.ChildBirthDate), BornChildDetailsController.Name),
            BirthStatus.Due => (nameof(ExpectedChildDetailsController.ChildDueDate), ExpectedChildDetailsController.Name),
            _ => throw new UnreachableException($"Unexpected birth status: {child.BirthStatus}")
        };

        return this.RedirectToAction(
            nextAction,
            nextController,
            new { childId = model.ChildId });
    }

    private async Task<string> GetChildNameBackLink(string? childId, string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, childId, out var url))
        {
            return url;
        }

        return await featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration)
                ? Url.ActionOrThrow(nameof(HomeController.Start), HomeController.Name)
                : Url.ActionOrThrow(nameof(HomeController.Location), HomeController.Name);
    }

    private string GetIsChildBornBackLink(string childId, string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, childId, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(ChildName), new { childId });
    }
}
