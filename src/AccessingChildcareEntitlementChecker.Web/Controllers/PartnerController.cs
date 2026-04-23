using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class PartnerController : Controller
{
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly IJourneySession _journeySession;

    public PartnerController(
        IStringLocalizerFactory localizerFactory,
        IJourneySession journeySession)
    {
        _localizerFactory = localizerFactory;
        _journeySession = journeySession;
    }

    [HttpGet]
    public ViewResult PartnerAge()
    {
        var state = _journeySession.Get();

        return View(new PartnerAgeViewModel
        {
            PartnerAge = state.PartnerAge
        });
    }

    [HttpPost]
    public IActionResult PartnerAge(PartnerAgeViewModel model)
    {
        var pageTexts = LocalizerForPage(nameof(PartnerAge));

        if (model.PartnerAge is null)
        {
            ModelState.AddModelError(
                nameof(model.PartnerAge),
                pageTexts["Error_SelectYourPartnersAge"]);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var state = _journeySession.Get();
        state.PartnerAge = model.PartnerAge;

        _journeySession.Save(state);

        return RedirectToAction(nameof(UserController.NextStepPlaceholder), "User");
    }

    private IStringLocalizer LocalizerForPage(string pageName)
    {
        var baseName = $"Views.Partner.{pageName}";
        var appName = typeof(Program).Assembly.GetName().Name!;

        return _localizerFactory.Create(baseName, appName);
    }
}