using System.Diagnostics;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class EntitlementController : Controller
{
    private readonly IStringLocalizer<WhereDoYouLiveViewModel> _whereDoYouLiveLocalizer;
    private readonly IJourneySession _journeySession;

    public EntitlementController(
        IStringLocalizer<WhereDoYouLiveViewModel> whereDoYouLiveLocalizer,
        IJourneySession journeySession)
    {
        _whereDoYouLiveLocalizer = whereDoYouLiveLocalizer;
        _journeySession = journeySession;
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
        if (model.Country is null)
        {
            ModelState.AddModelError(
                nameof(model.Country),
                _whereDoYouLiveLocalizer["Country_Required"]);
        }

        if (!ModelState.IsValid)
        {
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
}