using AccessingChildcareEntitlementChecker.Web.Filters;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

[ServiceFilter(typeof(RequireJourneySessionFilter))]
public class UserController(JourneyState journeyState, IJourneySession journeySession) : Controller
{
    public const string Name = "User";

    [HttpGet]
    public ViewResult UserAge(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(SummaryController.CheckChildDetails), SummaryController.Name);
        return View(new UserAgeViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult UserAge(UserAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(SummaryController.CheckChildDetails), SummaryController.Name);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(Nationality));
    }

    [HttpGet]
    public IActionResult Nationality(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(UserAge));
        return View(new NationalityViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult Nationality(NationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(UserAge));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        var nextAction = journeyState.Nationality switch
        {
            NationalityOption.BritishOrIrishCitizen => nameof(PaidWork),
            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland => nameof(SettledStatus),
            NationalityOption.CitizenOfADifferentCountry => nameof(PaidWork),
            _ => throw new UnreachableException($"Unexpected nationality option: {journeyState.Nationality}")
        };

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult SettledStatus(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(Nationality));
        return View(new SettledStatusViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult SettledStatus(SettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(Nationality));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(PaidWork));
    }

    [HttpGet]
    public IActionResult PaidWork(string? returnTo = null)
    {
        var backLink = GetPaidWorkBackLink(returnTo);
        return View(new PaidWorkViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PaidWork(PaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPaidWorkBackLink(model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        var nextAction = journeyState.PaidWork switch
        {
            PaidWorkOption.Yes => nameof(WorkStatus),
            PaidWorkOption.ParentalLeave => nameof(ParentalLeave),
            PaidWorkOption.SickLeave => nameof(WorkStatus),
            PaidWorkOption.No => nameof(UniversalCredit),
            _ => throw new UnreachableException($"Unexpected PaidWork: {journeyState.PaidWork}"),
        };

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult ParentalLeave(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PaidWork));
        return View(new ParentalLeaveViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ParentalLeave(ParentalLeaveViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PaidWork));
            model.Children = journeyState.Children.Values.ToList();
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(WorkStatus));
    }

    [HttpGet]
    public IActionResult WorkStatus(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PaidWork));
        return View(new WorkStatusViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult WorkStatus(WorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PaidWork));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);

        var nextAction = nameof(WeeklyEarnings);
        if (journeyState.WorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            nextAction = nameof(SelfEmployedDuration);
        }
        else if (journeyState.PaidWork == PaidWorkOption.SickLeave)
        {
            nextAction = nameof(YearlyEarnings);
        }

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult SelfEmployedDuration(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(WorkStatus));
        return View(new SelfEmployedDurationViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult SelfEmployedDuration(SelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(WorkStatus));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);

        // Complex logic for sick leave falls through
        var nextAction = nameof(WeeklyEarnings);
        if (journeyState.PaidWork == PaidWorkOption.SickLeave)
        {
            nextAction = nameof(YearlyEarnings);
        }

        if (journeyState.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            nextAction = nameof(UniversalCredit);
        }

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult YearlyEarnings(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(WeeklyEarnings));
        return View(new YearlyEarningsViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult YearlyEarnings(YearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(WeeklyEarnings));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        var nextAction = journeyState.YearlyEarnings switch
        {
            YearlyEarningsOption.AboveThreshold => nameof(Benefits),
            YearlyEarningsOption.BelowThreshold => nameof(UniversalCredit),
            _ => throw new UnreachableException($"Unexpected YearlyEarnings: {journeyState.YearlyEarnings}"),
        };

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult WeeklyEarnings(string? returnTo = null)
    {
        var backLink = GetWeeklyEarningsBackLink(returnTo);
        var weeklyEarningsThresholds = WeeklyEarningsThresholds.Create(journeyState.UserAge, journeyState.WorkStatus);
        var isOnParentalLeave = journeyState.PaidWork == PaidWorkOption.ParentalLeave;
        return View(new WeeklyEarningsViewModel(journeyState, weeklyEarningsThresholds, isOnParentalLeave, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult WeeklyEarnings(WeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var weeklyEarningsThresholds = WeeklyEarningsThresholds.Create(journeyState.UserAge, journeyState.WorkStatus);
            model.WeeklyEarningsThresholds = weeklyEarningsThresholds;
            model.IsOnParentalLeave = journeyState.PaidWork == PaidWorkOption.ParentalLeave;
            model.BackLink = GetWeeklyEarningsBackLink(model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        var nextAction = journeyState.WeeklyEarnings switch
        {
            WeeklyEarningsOption.AboveThreshold => nameof(YearlyEarnings),
            WeeklyEarningsOption.BelowThreshold => nameof(UniversalCredit),
            _ => throw new UnreachableException($"Unexpected WeeklyEarnings: {journeyState.WeeklyEarnings}"),
        };

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult UniversalCredit(string? returnTo = null)
    {
        var backLink = GetUniversalCreditBackLink(returnTo);
        return View(new UniversalCreditViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult UniversalCredit(UniversalCreditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetUniversalCreditBackLink(model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(Benefits));
    }

    [HttpGet]
    public IActionResult Benefits(string? returnTo = null)
    {
        var backLink = GetBenefitsBackLink(returnTo);
        return View(new BenefitsViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult Benefits(BenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetBenefitsBackLink(model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(ChildcareSupport));
    }

    [HttpGet]
    public IActionResult ChildcareSupport(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(Benefits));
        return View(new ChildcareSupportViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ChildcareSupport(ChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(Benefits));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        if (journeyState.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers))
        {
            return RedirectToAction(nameof(ChildcareVoucherReceipt));
        }

        return RedirectToAction(nameof(HasPartner));
    }

    [HttpGet]
    public IActionResult ChildcareVoucherReceipt(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(ChildcareSupport));
        return View(new ChildcareVoucherReceiptViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ChildcareVoucherReceipt(ChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(ChildcareSupport));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(HasPartner));
    }

    [HttpGet]
    public ViewResult HasPartner(string? returnTo = null)
    {
        var backLink = GetHasPartnerBackLink(returnTo);
        return View(new HasPartnerViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult HasPartner(HasPartnerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetHasPartnerBackLink(model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);

        if (journeyState.HasPartner == true)
        {
            return RedirectToAction(
                nameof(PartnerController.PartnerAge),
                PartnerController.Name);
        }

        return RedirectToAction(
            nameof(SummaryController.CheckAnswers),
            SummaryController.Name);
    }

    private string GetPaidWorkBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (journeyState.Nationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland)
        {
            return Url.ActionOrThrow(nameof(SettledStatus));
        }

        return Url.ActionOrThrow(nameof(Nationality));
    }

    private string GetWeeklyEarningsBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (journeyState.WorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            return Url.ActionOrThrow(nameof(SelfEmployedDuration));
        }

        return Url.ActionOrThrow(nameof(WorkStatus));
    }

    private string GetUniversalCreditBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (journeyState.PaidWork == PaidWorkOption.No)
        {
            return Url.ActionOrThrow(nameof(PaidWork));
        }

        if (journeyState.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            return Url.ActionOrThrow(nameof(SelfEmployedDuration));
        }

        if (journeyState.WeeklyEarnings == WeeklyEarningsOption.AboveThreshold)
        {
            return Url.ActionOrThrow(nameof(YearlyEarnings));
        }

        return Url.ActionOrThrow(nameof(WeeklyEarnings));
    }

    private string GetBenefitsBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (journeyState.YearlyEarnings == YearlyEarningsOption.AboveThreshold)
        {
            return Url.Action(nameof(YearlyEarnings), Name)
                ?? throw new InvalidOperationException("Unable to generate back link");
        }

        return Url.ActionOrThrow(nameof(UniversalCredit));
    }

    private string GetHasPartnerBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (journeyState.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers))
        {
            return Url.ActionOrThrow(nameof(ChildcareVoucherReceipt));
        }

        return Url.ActionOrThrow(nameof(ChildcareSupport));
    }
}
