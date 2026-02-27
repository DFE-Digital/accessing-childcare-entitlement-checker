using System.Diagnostics;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class EntitlementController : Controller
{
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly IJourneySession _journeySession;

    public EntitlementController(
        IStringLocalizerFactory localizerFactory,
        IJourneySession journeySession)
    {
        _localizerFactory = localizerFactory;
        _journeySession = journeySession;
    }
    
    private static class PageNames
    {
        public const string WhereDoYouLive = "WhereDoYouLive";
    }
    
    private IActionResult? GuardJourneyStarted(JourneyState state) =>
        state.CountryOfResidence is null
            ? RedirectToAction(nameof(SessionExpired))
            : null;

    
    [HttpGet]
    public IActionResult SessionExpired()
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
        var pageTexts = LocalizerForPage(PageNames.WhereDoYouLive);

        if (model.Country is null)
        {
            ModelState.AddModelError(
                nameof(model.Country),
                pageTexts["Error_SelectWhereYouLive"]);

            return View(model);
        }
        
        var state = _journeySession.Get();
        state.CountryOfResidence = model.Country;

        _journeySession.Save(state);

        return RedirectToAction(nameof(PlaceholderNextStep));
    }

    public IActionResult PlaceholderNextStep()
    {
        return Content("Next step placeholder");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }

    private IStringLocalizer LocalizerForPage(string pageName)
    {
        var baseName = $"Views.Entitlement.{pageName}";
        var appName = typeof(Program).Assembly.GetName().Name!;

        return _localizerFactory.Create(baseName, appName);
    }
}