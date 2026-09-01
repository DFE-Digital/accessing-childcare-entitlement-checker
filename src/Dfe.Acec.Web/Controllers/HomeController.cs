using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace Dfe.Acec.Web.Controllers;

public class HomeController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IFeatureManager _featureManager;

    public const string Name = "Home";

    public HomeController(JourneyState journeyState, IJourneySession journeySession, IFeatureManager featureManager)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
        _featureManager = featureManager;
    }

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
        if (await _featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration))
        {
            _journeyState.CountryOfResidence = CountryOfResidence.England;
            _journeySession.SetState(_journeyState);
            return RedirectToAction(nameof(IntroductionController.ChildName), IntroductionController.Name);
        }

        var backLink = GetLocationBackLink(returnTo);
        return View(new LocationViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult Location(LocationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetLocationBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.SetState(_journeyState);
        if (_journeyState.Children.Count > 0)
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
