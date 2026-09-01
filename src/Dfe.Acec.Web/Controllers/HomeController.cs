using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace Dfe.Acec.Web.Controllers;

public class HomeController(JourneyState journeyState, IJourneySession journeySession, IFeatureManager featureManager) : Controller
{
    public const string Name = "Home";

    [HttpGet]
    public IActionResult SessionExpired()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Start()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Location(string? returnTo = null)
    {
        if (await featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration))
        {
            journeyState.CountryOfResidence = CountryOfResidence.England;
            journeySession.SetState(journeyState);
            return RedirectToAction(nameof(IntroductionController.ChildName), IntroductionController.Name);
        }

        var backLink = GetLocationBackLink(returnTo);
        return View(new LocationViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult Location(LocationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetLocationBackLink(model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        if (journeyState.Children.Count > 0)
        {
            return RedirectToAction(nameof(SummaryController.CheckChildDetails), SummaryController.Name);
        }

        return RedirectToAction(nameof(IntroductionController.ChildName), IntroductionController.Name);
    }

    private string GetLocationBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(Start));
    }
}
