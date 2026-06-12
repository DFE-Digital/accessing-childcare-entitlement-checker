using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class UserController(
    JourneyState journeyState,
    IJourneySession journeySession,
    INavigationService navigationService)
    : Controller
{
    [HttpGet]
    public ViewResult UserAge()
    {
        return View(new UserAgeViewModel(journeyState));
    }

    [HttpPost]
    public IActionResult UserAge(UserAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.UserAge));
    }

    [HttpGet]
    public IActionResult Nationality(string? returnTo = null)
    {
        return View(new NationalityViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult Nationality(NationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.Nationality, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult SettledStatus(string? returnTo = null)
    {
        return View(new SettledStatusViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult SettledStatus(SettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.SettledStatus, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult PaidWork(string? returnTo = null)
    {
        return View(new PaidWorkViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PaidWork(PaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.PaidWork, model.ReturnTo));
    }

    [HttpGet]
    [ExcludeFromCodeCoverage(Justification = "This page is a stub for a future page")]
    public IActionResult WorkStatus(string? returnTo = null)
    {
        return View(new WorkStatusViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult WorkStatus(WorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.WorkStatus, model.ReturnTo));
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
        return View(new SelfEmployedDurationViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult SelfEmployedDuration(SelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.SelfEmployedDuration, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult YearlyEarnings(string? returnTo = null)
    {
        return View(new YearlyEarningsViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult YearlyEarnings(YearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.YearlyEarnings, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult WeeklyEarnings(string? returnTo = null)
    {
        return View(new WeeklyEarningsViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult WeeklyEarnings(WeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.WeeklyEarnings, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult UniversalCredit(string? returnTo = null)
    {
        return View(new UniversalCreditViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult UniversalCredit(UniversalCreditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.UniversalCredit, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult Benefits(string? returnTo = null)
    {
        return View(new BenefitsViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult Benefits(BenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.Benefits, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult ChildcareSupport(string? returnTo = null)
    {
        return View(new ChildcareSupportViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildcareSupport(ChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.ChildcareSupport, model.ReturnTo));
    }

    [HttpGet]
    public IActionResult ChildcareVoucherReceipt(string? returnTo = null)
    {
        return View(new ChildcareVoucherReceiptViewModel(journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildcareVoucherReceipt(ChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.ChildcareVoucherReceipt, model.ReturnTo));
    }

    [HttpGet]
    public ViewResult HasPartner()
    {
        return View(new HasPartnerViewModel(journeyState));
    }

    [HttpPost]
    public IActionResult HasPartner(HasPartnerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.HasPartner));
    }
}
