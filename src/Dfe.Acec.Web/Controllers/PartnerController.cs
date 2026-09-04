using System.Diagnostics;
using Dfe.Acec.Web.Filters;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web.Controllers;

[ServiceFilter(typeof(RequireJourneySessionFilter))]
public class PartnerController(JourneyState journeyState, IJourneySession journeySession) : Controller
{
    public const string Name = "Partner";

    [HttpGet]
    public ViewResult PartnerAge(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(UserController.HasPartner), UserController.Name);
        return View(new PartnerAgeViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerAge(PartnerAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(UserController.HasPartner), UserController.Name);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);

        var nextAction = nameof(PartnerPaidWork);
        if (!journeyState.NationalityOptions.Contains(NationalityOption.BritishOrIrishCitizen)
            && journeyState.SettledStatus != SettledStatusOption.Yes)
        {
            nextAction = nameof(PartnerNationality);
        }

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult PartnerNationality(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PartnerAge));
        return View(new PartnerNationalityViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerNationality(PartnerNationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PartnerAge));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);

        var needsSettledStatus = journeyState.PartnerNationalityOptions.Contains(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland);
        var nextAction = needsSettledStatus ? nameof(PartnerSettledStatus) : nameof(PartnerPaidWork);
        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult PartnerSettledStatus(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PartnerNationality));
        return View(new PartnerSettledStatusViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerSettledStatus(PartnerSettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PartnerNationality));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(PartnerPaidWork));
    }

    [HttpGet]
    public IActionResult PartnerPaidWork(string? returnTo = null)
    {
        var backLink = GetPartnerPaidWorkBackLink(returnTo);
        return View(new PartnerPaidWorkViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerPaidWork(PartnerPaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerPaidWorkBackLink(model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        var nextAction = journeyState.PartnerPaidWork switch
        {
            PartnerPaidWorkOption.Yes => nameof(PartnerWorkStatus),
            PartnerPaidWorkOption.ParentalLeave => nameof(PartnerParentalLeave),
            PartnerPaidWorkOption.SickLeave => nameof(PartnerWorkStatus),
            PartnerPaidWorkOption.No => nameof(PartnerBenefits),
            _ => throw new UnreachableException($"Unexpected PartnerPaidWork: {journeyState.PartnerPaidWork}"),
        };

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult PartnerParentalLeave(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PartnerPaidWork));
        return View(new PartnerParentalLeaveViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerParentalLeave(PartnerParentalLeaveViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PartnerPaidWork));
            model.Children = [.. journeyState.Children.Values];
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(PartnerWorkStatus));
    }

    [HttpGet]
    public IActionResult PartnerWorkStatus(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PartnerPaidWork));
        return View(new PartnerWorkStatusViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerWorkStatus(PartnerWorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PartnerPaidWork));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        var nextAction = nameof(PartnerWeeklyEarnings);
        if (journeyState.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            nextAction = nameof(PartnerSelfEmployedDuration);
        }
        else if (journeyState.PartnerPaidWork == PartnerPaidWorkOption.SickLeave)
        {
            nextAction = nameof(PartnerYearlyEarnings);
        }

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult PartnerBenefits(string? returnTo = null)
    {
        var backLink = GetPartnerBenefitsBackLink(returnTo);
        return View(new PartnerBenefitsViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerBenefits(PartnerBenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerBenefitsBackLink(model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(PartnerChildcareSupport));
    }

    [HttpGet]
    public IActionResult PartnerSelfEmployedDuration(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PartnerWorkStatus));
        return View(new PartnerSelfEmployedDurationViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerSelfEmployedDuration(PartnerSelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PartnerWorkStatus));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);

        // Complex logic for sick leave falls through
        var nextAction = nameof(PartnerWeeklyEarnings);
        if (journeyState.PartnerPaidWork == PartnerPaidWorkOption.SickLeave)
        {
            nextAction = nameof(PartnerYearlyEarnings);
        }

        if (journeyState.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            nextAction = nameof(PartnerBenefits);
        }

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult PartnerWeeklyEarnings(string? returnTo = null)
    {
        var backLink = GetPartnerWeeklyEarningsBackLink(returnTo);
        var weeklyEarningsThresholds = WeeklyEarningsThresholds.Create(journeyState.PartnerAge, journeyState.PartnerWorkStatus);
        var isOnParentalLeave = journeyState.PartnerPaidWork == PartnerPaidWorkOption.ParentalLeave;
        return View(new PartnerWeeklyEarningsViewModel(journeyState, weeklyEarningsThresholds, isOnParentalLeave, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerWeeklyEarnings(PartnerWeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.WeeklyEarningsThresholds = WeeklyEarningsThresholds.Create(journeyState.PartnerAge, journeyState.PartnerWorkStatus);
            model.IsOnParentalLeave = journeyState.PartnerPaidWork == PartnerPaidWorkOption.ParentalLeave;
            model.BackLink = GetPartnerWeeklyEarningsBackLink(model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        var nextAction = journeyState.PartnerWeeklyEarnings switch
        {
            WeeklyEarningsOption.AboveThreshold => nameof(PartnerYearlyEarnings),
            WeeklyEarningsOption.BelowThreshold => nameof(PartnerBenefits),
            _ => throw new UnreachableException($"Unexpected PartnerWeeklyEarnings: {journeyState.PartnerWeeklyEarnings}"),
        };

        return RedirectToAction(nextAction);
    }

    [HttpGet]
    public IActionResult PartnerYearlyEarnings(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PartnerWeeklyEarnings));
        return View(new PartnerYearlyEarningsViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerYearlyEarnings(PartnerYearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PartnerWeeklyEarnings));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(PartnerBenefits));
    }

    [HttpGet]
    public IActionResult PartnerChildcareSupport(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PartnerBenefits));
        return View(new PartnerChildcareSupportViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerChildcareSupport(PartnerChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PartnerBenefits));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        if (journeyState.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers))
        {
            return RedirectToAction(nameof(PartnerChildcareVoucherReceipt));
        }

        return RedirectToAction(nameof(SummaryController.CheckAnswers), SummaryController.Name);
    }

    [HttpGet]
    public IActionResult PartnerChildcareVoucherReceipt(string? returnTo = null)
    {
        var backLink = Url.GetBackLinkOrAction(returnTo, nameof(PartnerChildcareSupport));
        return View(new PartnerChildcareVoucherReceiptViewModel(journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerChildcareVoucherReceipt(PartnerChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = Url.GetBackLinkOrAction(model.ReturnTo, nameof(PartnerChildcareSupport));
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);
        return RedirectToAction(nameof(SummaryController.CheckAnswers), SummaryController.Name);
    }

    private string GetPartnerPaidWorkBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (journeyState.NationalityOptions.Contains(NationalityOption.BritishOrIrishCitizen))
        {
            return Url.ActionOrThrow(nameof(PartnerAge));
        }

        var userNeedsSettledStatus = journeyState.NationalityOptions.Contains(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland);
        if (userNeedsSettledStatus && journeyState.SettledStatus == SettledStatusOption.Yes)
        {
            return Url.ActionOrThrow(nameof(PartnerAge));
        }

        if (journeyState.PartnerNationalityOptions.Contains(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland))
        {
            return Url.ActionOrThrow(nameof(PartnerSettledStatus));
        }

        return Url.ActionOrThrow(nameof(PartnerNationality));
    }

    private string GetPartnerWeeklyEarningsBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (journeyState.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            return Url.ActionOrThrow(nameof(PartnerSelfEmployedDuration));
        }

        return Url.ActionOrThrow(nameof(PartnerWorkStatus));
    }

    private string GetPartnerBenefitsBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (journeyState.PartnerYearlyEarnings == YearlyEarningsOption.AboveThreshold)
        {
            return Url.ActionOrThrow(nameof(PartnerYearlyEarnings));
        }

        if (journeyState.PartnerWeeklyEarnings == WeeklyEarningsOption.AboveThreshold)
        {
            return Url.ActionOrThrow(nameof(PartnerYearlyEarnings));
        }

        if (journeyState.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            return Url.ActionOrThrow(nameof(PartnerSelfEmployedDuration));
        }

        if (journeyState.PartnerPaidWork == PartnerPaidWorkOption.No)
        {
            return Url.ActionOrThrow(nameof(PartnerPaidWork));
        }

        return Url.ActionOrThrow(nameof(PartnerWeeklyEarnings));
    }
}
