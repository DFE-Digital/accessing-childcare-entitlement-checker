using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class PartnerController(
    JourneyState journeyState,
    IJourneySession journeySession,
    INavigationService navigationService)
    : Controller
{
    [HttpGet]
    public ViewResult PartnerAge(string? returnTo = null)
    {
        return View(new PartnerAgeViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerAge(PartnerAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerAge, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PartnerNationality(string? returnTo = null)
    {
        return View(new PartnerNationalityViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerNationality(PartnerNationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerNationality, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PartnerSettledStatus(string? returnTo = null)
    {
        return View(new PartnerSettledStatusViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerSettledStatus(PartnerSettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerSettledStatus, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PartnerPaidWork(string? returnTo = null)
    {
        return View(new PartnerPaidWorkViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerPaidWork(PartnerPaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerPaidWork, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PartnerWorkStatus(string? returnTo = null)
    {
        return View(new PartnerWorkStatusViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerWorkStatus(PartnerWorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerWorkStatus, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PartnerBenefits(string? returnTo = null)
    {
        return View(new PartnerBenefitsViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerBenefits(PartnerBenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerBenefits, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PartnerSelfEmployedDuration(string? returnTo = null)
    {
        return View(new PartnerSelfEmployedDurationViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerSelfEmployedDuration(PartnerSelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerSelfEmployedDuration, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PartnerWeeklyEarnings(string? returnTo = null)
    {
        return View(new PartnerWeeklyEarningsViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerWeeklyEarnings(PartnerWeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerWeeklyEarnings, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PartnerYearlyEarnings(string? returnTo = null)
    {
        return View(new PartnerYearlyEarningsViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerYearlyEarnings(PartnerYearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerYearlyEarnings, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PartnerChildcareSupport(string? returnTo = null)
    {
        return View(new PartnerChildcareSupportViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerChildcareSupport(PartnerChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerChildcareSupport, model.ReturnTo));
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
        return View(new PartnerChildcareVoucherReceiptViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerChildcareVoucherReceipt(PartnerChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PartnerChildcareVoucherReceipt, model.ReturnTo));
    }
}
