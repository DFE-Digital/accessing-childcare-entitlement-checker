using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static AccessingChildcareEntitlementChecker.Web.IServiceCollectionExtensions;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class UserController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly Journey _journey;

    public UserController(
        JourneyState journeyState,
        IJourneySession journeySession,
        Journey journey)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
        _journey = journey;
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
        return _journey.Forwards(this, _journeyState);
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
        return _journey.Forwards(this, _journeyState);
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
        return _journey.Forwards(this, _journeyState);
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
        return _journey.Forwards(this, _journeyState);
    }

    [HttpGet]
    [ExcludeFromCodeCoverage(Justification = "This page is a stub for a future page")]
    public IActionResult WorkStatus(string? returnTo = null)
    {
        return View(new WorkStatusViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult WorkStatus(WorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState);
    }

    [HttpGet]
    [ExcludeFromCodeCoverage(Justification = "This page is a stub for a future page")]
    public IActionResult TypeOfLeave(string? returnTo = null)
    {
        // This page a stub as not yet confirmed in design.
        return Content("<h1>TypeOfLeave</h1>", "text/html");
    }

    [HttpGet]
    public IActionResult SelfEmployedDuration(string? returnTo = null)
    {
        return View(new SelfEmployedDurationViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult SelfEmployedDuration(SelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState);
    }

    [HttpGet]
    public IActionResult YearlyEarnings(string? returnTo = null)
    {
        return View(new YearlyEarningsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult YearlyEarnings(YearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState);
    }

    [HttpGet]
    public IActionResult WeeklyEarnings(string? returnTo = null)
    {
        return View(new WeeklyEarningsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult WeeklyEarnings(WeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState);
    }

    [HttpGet]
    public IActionResult UniversalCredit(string? returnTo = null)
    {
        return View(new UniversalCreditViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult UniversalCredit(UniversalCreditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState);
    }

    [HttpGet]
    public IActionResult Benefits(string? returnTo = null)
    {
        return View(new BenefitsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult Benefits(BenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState); ;
    }

    [HttpGet]
    public IActionResult ChildcareSupport(string? returnTo = null)
    {
        return View(new ChildcareSupportViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildcareSupport(ChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState);
    }

    [HttpGet]
    public IActionResult ChildcareVoucherReceipt(string? returnTo = null)
    {
        return View(new ChildcareVoucherReceiptViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildcareVoucherReceipt(ChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState);
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
        return _journey.Forwards(this, _journeyState);
    }
}
