using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class UserController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public UserController(JourneyState journeyState, IJourneySession journeySession)
    {
        _journeyState = journeyState;
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
        return View(new HasPartnerViewModel(_journeyState));
    }

    [HttpPost]
    public IActionResult HasPartner(HasPartnerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        if (_journeyState.HasPartner == true)
        {
            return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerAge));
        }

        return this.RedirectTo<UserController>(nameof(NextStepPlaceholder));
    }

    [HttpGet]
    public ViewResult UserAge()
    {
        return View(new UserAgeViewModel(_journeyState));
    }

    [HttpPost]
    public IActionResult UserAge(UserAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return this.RedirectTo<UserController>(nameof(Nationality));
    }

    [HttpGet]
    public IActionResult Nationality(string? returnTo = null)
    {
        return View(new NationalityViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult Nationality(NationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        switch (model.Nationality)
        {
            case NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland:
                return this.RedirectTo<UserController>(nameof(SettledStatus));
            default:
                return this.RedirectTo<UserController>(nameof(PaidWork));
        }
    }

    [HttpGet]
    public IActionResult PaidWork()
    {
        return Content("Are you in paid work?");
    }

    [HttpGet]
    public IActionResult SettledStatus()
    {
        return Content("Do you have settled or pre-settled status under the EU Settlement Scheme?");
    }
}
