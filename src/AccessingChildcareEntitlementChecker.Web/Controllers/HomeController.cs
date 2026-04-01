using System.Diagnostics;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class HomeController : Controller
{
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly IJourneySession _journeySession;

    public HomeController(
        IStringLocalizerFactory localizerFactory,
        IJourneySession journeySession)
    {
        _localizerFactory = localizerFactory;
        _journeySession = journeySession;
    }

    //Commenting this out until we are ready to use it
    //private IActionResult? GuardJourneyStarted(JourneyState state) =>
    //state.CountryOfResidence is null
    //? RedirectToAction(nameof(SessionExpired))
    //: null;


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
    public IActionResult WhereDoYouLive()
    {
        var state = _journeySession.Get();

        return View(new WhereDoYouLiveViewModel
        {
            Country = state.CountryOfResidence
        });
    }

    [HttpPost]
    public IActionResult WhereDoYouLive(WhereDoYouLiveViewModel model)
    {
        var pageTexts = LocalizerForPage(nameof(WhereDoYouLive));

        if (model.Country is null)
        {
            ModelState.AddModelError(
                nameof(model.Country),
                pageTexts["Error_SelectWhereYouLive"]);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var state = _journeySession.Get();
        state.CountryOfResidence = model.Country;

        _journeySession.Save(state);

        return RedirectToAction(nameof(UserController.Index), "User");
    }

    private IStringLocalizer LocalizerForPage(string pageName)
    {
        var baseName = $"Views.Home.{pageName}";
        var appName = typeof(Program).Assembly.GetName().Name!;

        return _localizerFactory.Create(baseName, appName);
    }
}