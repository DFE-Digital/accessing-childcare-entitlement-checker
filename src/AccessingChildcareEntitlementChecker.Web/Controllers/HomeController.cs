using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class HomeController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public const string Name = "Home";

    public HomeController(JourneyState journeyState, IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
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
    public ViewResult Location(string? returnTo = null)
    {
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
        _journeySession.Set(_journeyState);
        if (model.ReturnTo is not null)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
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
