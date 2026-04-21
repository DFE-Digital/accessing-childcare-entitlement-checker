using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AccessingChildcareEntitlementChecker.Web.Services;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class UserController : Controller
{
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly IJourneySession _journeySession;

    public UserController(
        IStringLocalizerFactory localizerFactory,
        IJourneySession journeySession)
    {
        _localizerFactory = localizerFactory;
        _journeySession = journeySession;
    }

    [HttpGet]
    public IActionResult NextStepPlaceholder()
    {
        return Content("Next step placeholder");
    }

    [HttpGet]
    public ViewResult HasPartner()
    {
        var state = _journeySession.Get();

        return View(new HasPartnerViewModel
        {
            HasPartner = state.HasPartner
        });
    }

    [HttpPost]
    public IActionResult HasPartner(HasPartnerViewModel model)
    {
        var pageTexts = LocalizerForPage(nameof(HasPartner));

        if (model.HasPartner is null)
        {
            ModelState.AddModelError(
                nameof(model.HasPartner),
                pageTexts["Error_SelectIfYouHavePartner"]);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var state = _journeySession.Get();
        state.HasPartner = model.HasPartner ?? false;

        _journeySession.Save(state);

        return RedirectToAction(nameof(UserController.NextStepPlaceholder), "User");
    }

    [HttpGet]
    public ViewResult UserAge()
    {
        var state = _journeySession.Get();

        return View(new UserAgeViewModel
        {
            UserAge = state.UserAge
        });
    }

    [HttpPost]
    public IActionResult UserAge(UserAgeViewModel model)
    {
        var pageTexts = LocalizerForPage(nameof(UserAge));

        if (model.UserAge is null)
        {
            ModelState.AddModelError(
                nameof(model.UserAge),
                pageTexts["Error_Select-your-age"]);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var state = _journeySession.Get();
        state.UserAge = model.UserAge;

        _journeySession.Save(state);

        return RedirectToAction(nameof(PartnerController.PartnerAge), "Partner");
    }

    [HttpGet]
    [ExcludeFromCodeCoverage]
    public IActionResult ChildDetails()
    {
        throw new NotImplementedException();
    }

    private IStringLocalizer LocalizerForPage(string pageName)
    {
        var baseName = $"Views.User.{pageName}";
        var appName = typeof(Program).Assembly.GetName().Name!;

        return _localizerFactory.Create(baseName, appName);
    }
}