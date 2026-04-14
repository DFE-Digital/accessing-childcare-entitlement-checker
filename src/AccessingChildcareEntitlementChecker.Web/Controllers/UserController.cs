using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AccessingChildcareEntitlementChecker.Web.Services;

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
    public IActionResult HasPartner()
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
    public IActionResult Age()
    {
        var state = _journeySession.Get();

        return View(new AgeModel
        {
            Age = state.Age
        });
    }

    [HttpPost]
    public IActionResult Age(AgeModel model)
    {
        var pageTexts = LocalizerForPage(nameof(Age));

        if (model.Age is null)
        {
            ModelState.AddModelError(
                nameof(model.Age),
                pageTexts["Select your age"]);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var state = _journeySession.Get();
        state.Age = model.Age;

        _journeySession.Save(state);

        return RedirectToAction(nameof(HowOldIsYourPartner), "User");
    }

    [HttpGet]
    public IActionResult ChildrensDetails()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public IActionResult HowOldIsYourPartner()
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