using AccessingChildcareEntitlementChecker.Web.Extensions;
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
        journeySession.Set(journeyState);
        // Logic here a little complex because this changes not just the next question,
        // but also a question a couple of steps ahead. So we need to walk forward through
        // the journey.
        //
        // See also - PartnerController.PartnerAge
        var nationalityMissing = journeyState.Nationality == null;
        var paidWorkMissing = journeyState.PaidWork == null;
        var weeklyEarningsMissing = journeyState.PaidWork == PaidWorkOption.Yes && journeyState.WeeklyEarnings == null;
        var nextAnswerMissing = nationalityMissing || paidWorkMissing || weeklyEarningsMissing;

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        // Now walk backwards from weekly earnings.
        var nextAction = nameof(WeeklyEarnings);
        if (paidWorkMissing)
        {
            nextAction = nameof(PaidWork);
        }
        if (nationalityMissing)
        {
            nextAction = nameof(Nationality);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var (nextAction, nextAnswerMissing) = journeyState.Nationality switch
        {
            NationalityOption.BritishOrIrishCitizen => (nameof(PaidWork), journeyState.PaidWork == null),
            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland => (nameof(SettledStatus), journeyState.SettledStatus == null),
            NationalityOption.CitizenOfADifferentCountry => (nameof(PaidWork), journeyState.PaidWork == null),
            _ => throw new UnreachableException($"Unexpected nationality option: {journeyState.Nationality}")
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAnswerMissing = journeyState.PaidWork == null;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nameof(PaidWork), new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var (nextAction, nextAnswerMissing) = journeyState.PaidWork switch
        {
            PaidWorkOption.Yes => (nameof(WorkStatus), journeyState.WorkStatus.Count == 0),
            PaidWorkOption.ParentalLeave => (nameof(ParentalLeave), journeyState.ParentalLeaveChildrenIds.Count == 0),
            PaidWorkOption.SickLeave => (nameof(WorkStatus), journeyState.WorkStatus.Count == 0),
            PaidWorkOption.No => (nameof(UniversalCredit), journeyState.UniversalCredit is null),
            _ => throw new UnreachableException($"Unexpected PaidWork: {journeyState.PaidWork}"),
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAction = nameof(WorkStatus);
        var nextAnswerMissing = journeyState.WorkStatus.Count == 0;

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);

        var nextAction = nameof(WeeklyEarnings);
        var nextAnswerMissing = journeyState.WeeklyEarnings is null;
        if (journeyState.WorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            nextAction = nameof(SelfEmployedDuration);
            nextAnswerMissing = journeyState.SelfEmployedDuration is null;
        }
        else if (journeyState.PaidWork == PaidWorkOption.SickLeave)
        {
            nextAction = nameof(YearlyEarnings);
            nextAnswerMissing = journeyState.YearlyEarnings is null;
        }

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);

        // Complex logic for sick leave falls through
        var nextAction = nameof(WeeklyEarnings);
        var nextAnswerMissing = journeyState.WeeklyEarnings is null;

        if (journeyState.PaidWork == PaidWorkOption.SickLeave)
        {
            nextAction = nameof(YearlyEarnings);
            nextAnswerMissing = journeyState.YearlyEarnings is null;
        }

        if (journeyState.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            nextAction = nameof(UniversalCredit);
            nextAnswerMissing = journeyState.UniversalCredit is null;
        }

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var (nextAction, nextAnswerMissing) = journeyState.YearlyEarnings switch
        {
            YearlyEarningsOption.AboveThreshold => (nameof(Benefits), journeyState.Benefits.Count == 0),
            YearlyEarningsOption.BelowThreshold => (nameof(UniversalCredit), journeyState.UniversalCredit is null),
            _ => throw new UnreachableException($"Unexpected YearlyEarnings: {journeyState.YearlyEarnings}"),
        };
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var (nextAction, nextAnswerMissing) = journeyState.WeeklyEarnings switch
        {
            WeeklyEarningsOption.AboveThreshold => (nameof(YearlyEarnings), journeyState.YearlyEarnings is null),
            WeeklyEarningsOption.BelowThreshold => (nameof(UniversalCredit), journeyState.UniversalCredit is null),
            _ => throw new UnreachableException($"Unexpected WeeklyEarnings: {journeyState.WeeklyEarnings}"),
        };
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAnswerMissing = journeyState.Benefits.Count == 0;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nameof(Benefits), new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAnswerMissing = journeyState.ChildcareSupport.Count == 0;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nameof(ChildcareSupport), new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        if (journeyState.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers))
        {
            if (model.ReturnTo is not null && journeyState.ChildcareVoucherReceipt is not null)
                return this.RedirectToReturnTo(model.ReturnTo);
            return RedirectToAction(nameof(ChildcareVoucherReceipt), new { returnTo = model.ReturnTo });
        }

        if (model.ReturnTo is not null && journeyState.HasPartner is not null)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nameof(HasPartner), new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAnswerMissing = journeyState.HasPartner is null;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return RedirectToAction(nameof(HasPartner), new { returnTo = model.ReturnTo });
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
        journeySession.Set(journeyState);
        var nextAnswerMissing = journeyState.HasPartner == true && journeyState.PartnerAge is null;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        if (journeyState.HasPartner == true)
        {
            return RedirectToAction(
                nameof(PartnerController.PartnerAge),
                PartnerController.Name,
                new { returnTo = model.ReturnTo });
        }

        return RedirectToAction(
            nameof(SummaryController.CheckAnswers),
            SummaryController.Name,
            new { returnTo = model.ReturnTo });
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
