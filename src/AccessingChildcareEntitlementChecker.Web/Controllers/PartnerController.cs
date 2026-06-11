using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using System.Diagnostics.CodeAnalysis;
using static AccessingChildcareEntitlementChecker.Web.IServiceCollectionExtensions;
namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class PartnerController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly Journey _journey;

    public PartnerController(
        JourneyState journeyState,
        IJourneySession journeySession,
        Journey journey)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
        _journey = journey;
    }

    [HttpGet]
    public ViewResult PartnerAge(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerAgeViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerAge(PartnerAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerNationality(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerNationalityViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerNationality(PartnerNationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerSettledStatus(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerSettledStatusViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerSettledStatus(PartnerSettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerPaidWork(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerPaidWorkViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerPaidWork(PartnerPaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerWorkStatus(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerWorkStatusViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerWorkStatus(PartnerWorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerBenefits(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerBenefitsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerBenefits(PartnerBenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerSelfEmployedDuration(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerSelfEmployedDurationViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerSelfEmployedDuration(PartnerSelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerWeeklyEarnings(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerWeeklyEarningsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerWeeklyEarnings(PartnerWeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerYearlyEarnings(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerYearlyEarningsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerYearlyEarnings(PartnerYearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerChildcareSupport(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerChildcareSupportViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerChildcareSupport(PartnerChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    [ExcludeFromCodeCoverage(Justification = "This page is a stub for a future page")]
    public IActionResult PartnerTypeOfLeave()
    {
        return Content("<h1>What type of leave is your partner on?</h1>", "text/html");
    }

    [HttpGet]
    public IActionResult PartnerChildcareVoucherReceipt(string? returnTo = null)
    {
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo });
        return View(new PartnerChildcareVoucherReceiptViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerChildcareVoucherReceipt(PartnerChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState, new { returnTo = model.ReturnTo });
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _journey.Forwards(this, _journeyState, new { returnTo = model.ReturnTo });
    }
}
