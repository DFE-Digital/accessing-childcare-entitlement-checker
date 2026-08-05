using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Filters;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

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
        journeySession.Set(journeyState);

        // Logic here a little complex because of the dependencies between questions
        // We need to walk forwards through the journey to find the next dependent,
        // unanswered question.
        // See also - UserController.UserAge
        var requiresPartnerNationality = journeyState.Nationality != NationalityOption.BritishOrIrishCitizen
            && journeyState.SettledStatus != SettledStatusOption.Yes;
        var partnerNationalityMissing = requiresPartnerNationality && journeyState.PartnerNationality == null;
        var partnerPaidWorkMissing = journeyState.PartnerPaidWork == null;
        var partnerWeeklyEarningsMissing = journeyState is { PartnerPaidWork: PartnerPaidWorkOption.Yes, PartnerWeeklyEarnings: null };
        var nextAnswerMissing = partnerNationalityMissing || partnerPaidWorkMissing || partnerWeeklyEarningsMissing;

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        // Now walk backwards from weekly earnings.
        var nextAction = nameof(PartnerWeeklyEarnings);
        if (partnerPaidWorkMissing)
        {
            nextAction = nameof(PartnerPaidWork);
        }
        if (partnerNationalityMissing)
        {
            nextAction = nameof(PartnerNationality);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var (nextAction, nextAnswerMissing) = journeyState.PartnerNationality switch
        {
            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland => (nameof(PartnerSettledStatus), journeyState.PartnerSettledStatus is null),
            NationalityOption.BritishOrIrishCitizen => (nameof(PartnerPaidWork), journeyState.PartnerPaidWork is null),
            NationalityOption.CitizenOfADifferentCountry => (nameof(PartnerPaidWork), journeyState.PartnerPaidWork is null),
            _ => throw new UnreachableException($"Unexpected PartnerNationality: {journeyState.PartnerNationality}"),
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAnswerMissing = journeyState.PartnerPaidWork is null;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nameof(PartnerPaidWork), new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var (nextAction, nextAnswerMissing) = journeyState.PartnerPaidWork switch
        {
            PartnerPaidWorkOption.Yes => (nameof(PartnerWorkStatus), journeyState.PartnerWorkStatus.Count == 0),
            PartnerPaidWorkOption.ParentalLeave => (nameof(PartnerParentalLeave), journeyState.PartnerParentalLeaveChildrenIds.Count == 0),
            PartnerPaidWorkOption.SickLeave => (nameof(PartnerWorkStatus), journeyState.PartnerWorkStatus.Count == 0),
            PartnerPaidWorkOption.No => (nameof(PartnerBenefits), journeyState.PartnerBenefits.Count == 0),
            _ => throw new UnreachableException($"Unexpected PartnerPaidWork: {journeyState.PartnerPaidWork}"),
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
            model.Children = journeyState.Children.Values.ToList();
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        var nextAction = nameof(PartnerWorkStatus);
        var nextAnswerMissing = journeyState.PartnerWorkStatus.Count == 0;

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAction = nameof(PartnerWeeklyEarnings);
        var nextAnswerMissing = journeyState.PartnerWeeklyEarnings is null;
        if (journeyState.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            nextAction = nameof(PartnerSelfEmployedDuration);
            nextAnswerMissing = journeyState.PartnerSelfEmployedDuration is null;
        }
        else if (journeyState.PartnerPaidWork == PartnerPaidWorkOption.SickLeave)
        {
            nextAction = nameof(PartnerYearlyEarnings);
            nextAnswerMissing = journeyState.PartnerYearlyEarnings is null;
        }

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAnswerMissing = journeyState.PartnerChildcareSupport.Count == 0;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nameof(PartnerChildcareSupport), new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);

        // Complex logic for sick leave falls through
        var nextAction = nameof(PartnerWeeklyEarnings);
        var nextAnswerMissing = journeyState.PartnerWeeklyEarnings is null;

        if (journeyState.PartnerPaidWork == PartnerPaidWorkOption.SickLeave)
        {
            nextAction = nameof(PartnerYearlyEarnings);
            nextAnswerMissing = journeyState.PartnerYearlyEarnings is null;
        }

        if (journeyState.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            nextAction = nameof(PartnerBenefits);
            nextAnswerMissing = journeyState.PartnerBenefits.Count == 0;
        }

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var (nextAction, nextAnswerMissing) = journeyState.PartnerWeeklyEarnings switch
        {
            WeeklyEarningsOption.AboveThreshold => (nameof(PartnerYearlyEarnings), journeyState.PartnerYearlyEarnings is null),
            WeeklyEarningsOption.BelowThreshold => (nameof(PartnerBenefits), journeyState.PartnerBenefits.Count == 0),
            _ => throw new UnreachableException($"Unexpected PartnerWeeklyEarnings: {journeyState.PartnerWeeklyEarnings}"),
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAnswerMissing = journeyState.PartnerBenefits.Count == 0;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nameof(PartnerBenefits), new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        if (journeyState.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers))
        {
            if (model.ReturnTo is not null && journeyState.PartnerChildcareVoucherReceipt is not null)
            {
                return this.RedirectToReturnTo(model.ReturnTo);
            }

            return RedirectToAction(nameof(PartnerChildcareVoucherReceipt), new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        return RedirectToAction(nameof(SummaryController.CheckAnswers), SummaryController.Name);
    }

    private string GetPartnerPaidWorkBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (journeyState.Nationality == NationalityOption.BritishOrIrishCitizen)
        {
            return Url.ActionOrThrow(nameof(PartnerAge));
        }

        if (journeyState is { Nationality: NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatus: SettledStatusOption.Yes })
        {
            return Url.ActionOrThrow(nameof(PartnerAge));
        }

        if (journeyState.PartnerNationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland)
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
