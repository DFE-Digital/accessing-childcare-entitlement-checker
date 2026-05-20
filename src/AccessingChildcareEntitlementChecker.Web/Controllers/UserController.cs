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
        return Content("<h1 >Next step placeholder</h1>", "text/html");
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
        return model.Nationality switch
        {
            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland => this.RedirectTo<UserController>(nameof(SettledStatus)),
            _ => this.RedirectTo<UserController>(nameof(PaidWork)),
        };
    }

    [HttpGet]
    public IActionResult SettledStatus(string? returnTo = null)
    {
        return View(new SettledStatusViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult SettledStatus(SettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return this.RedirectTo<UserController>(nameof(PaidWork));
    }

    [HttpGet]
    public IActionResult PaidWork(string? returnTo = null)
    {
        return View(new PaidWorkViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PaidWork(PaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var redirect = model.PaidWork switch
        {
            PaidWorkOption.Yes => this.RedirectTo<UserController>(nameof(WorkStatus)),
            PaidWorkOption.OnLeave => this.RedirectTo<UserController>(nameof(TypeOfLeave)),
            _ => this.RedirectTo<UserController>(nameof(UniversalCredit)),
        };

        return redirect;
    }

    [HttpGet]
    public IActionResult WorkStatus(string? returnTo = null)
    {
        return Content("<h1>How would you describe your work status?</h1>", "text/html");
    }

    [HttpGet]
    public IActionResult TypeOfLeave(string? returnTo = null)
    {
        // This page a stub as not yet confirmed in design.
        return Content("<h1>TypeOfLeave</h1>", "text/html");
    }

    [HttpGet]
    public IActionResult UniversalCredit(string? returnTo = null)
    {
        return Content("<h1>Does your household receive universal credit?</h1>", "text/html");
    }
}
