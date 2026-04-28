using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Services;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class UserController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly JourneyActions _journeyActions;

    public UserController(JourneyState journeyState, JourneyActions.Factory journeyActionsFactory)
    {
        _journeyState = journeyState;
        _journeyActions = journeyActionsFactory.Create(this);
    }

    [HttpGet]
    public IActionResult NextStepPlaceholder()
    {
        return Content("Next step placeholder");
    }

    [HttpGet]
    public ViewResult HasPartner()
    {
        return View(new HasPartnerViewModel(_journeyState));
    }

    [HttpPost]
    public IActionResult HasPartner(HasPartnerViewModel model)
    {
        return _journeyActions.HandlePost(
            model,
            (state) => state.Apply(model),
            (UserController c) => c.NextStepPlaceholder());
    }

    [HttpGet]
    public ViewResult UserAge()
    {
        return View(new UserAgeViewModel(_journeyState));
    }

    [HttpPost]
    public IActionResult UserAge(UserAgeViewModel model)
    {
        return _journeyActions.HandlePost(
           model,
           (state) => state.Apply(model),
           (PartnerController c) => c.PartnerAge());
    }

    [HttpGet]
    [ExcludeFromCodeCoverage]
    public IActionResult ChildDetails()
    {
        return View();
    }
}