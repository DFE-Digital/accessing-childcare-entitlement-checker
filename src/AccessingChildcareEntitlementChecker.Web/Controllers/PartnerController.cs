using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Filters;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

[ServiceFilter(typeof(RequireJourneySessionFilter))]
public class PartnerController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public const string Name = "Partner";

    public PartnerController(JourneyState journeyState, IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public ViewResult PartnerAge(string? returnTo = null)
    {
        var backLink = GetPartnerAgeBackLink(returnTo);
        return View(new PartnerAgeViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerAge(PartnerAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerAgeBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        // Logic here a little complex because of the dependencies between questions
        // We need to walk forwards through the journey to find the next dependent,
        // unanswered question.
        // See also - UserController.UserAge
        var requiresPartnerNationality = _journeyState.Nationality != NationalityOption.BritishOrIrishCitizen
            && _journeyState.SettledStatus != SettledStatusOption.Yes;
        var partnerNationalityMissing = requiresPartnerNationality && _journeyState.PartnerNationality == null;
        var partnerPaidWorkMissing = _journeyState.PartnerPaidWork == null;
        var partnerWeeklyEarningsMissing = _journeyState.PartnerPaidWork == PartnerPaidWorkOption.Yes && _journeyState.PartnerWeeklyEarnings == null;
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

        return this.RedirectToAction(
            nextAction,
            new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerNationality(string? returnTo = null)
    {
        var backLink = GetPartnerNationalityBackLink(returnTo);
        return View(new PartnerNationalityViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerNationality(PartnerNationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerNationalityBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var (nextAction, nextAnswerMissing) = _journeyState.PartnerNationality switch
        {
            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland => (nameof(PartnerSettledStatus), _journeyState.PartnerSettledStatus is null),
            NationalityOption.BritishOrIrishCitizen => (nameof(PartnerPaidWork), _journeyState.PartnerPaidWork is null),
            NationalityOption.CitizenOfADifferentCountry => (nameof(PartnerPaidWork), _journeyState.PartnerPaidWork is null),
            _ => throw new UnreachableException($"Unexpected PartnerNationality: {_journeyState.PartnerNationality}"),
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerSettledStatus(string? returnTo = null)
    {
        var backLink = GetPartnerSettledStatusBackLink(returnTo);
        return View(new PartnerSettledStatusViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerSettledStatus(PartnerSettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerSettledStatusBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAnswerMissing = _journeyState.PartnerPaidWork is null;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nameof(PartnerPaidWork), new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerPaidWork(string? returnTo = null)
    {
        var backLink = GetPartnerPaidWorkBackLink(returnTo);
        return View(new PartnerPaidWorkViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerPaidWork(PartnerPaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerPaidWorkBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var (nextAction, nextAnswerMissing) = _journeyState.PartnerPaidWork switch
        {
            PartnerPaidWorkOption.Yes => (nameof(PartnerWorkStatus), _journeyState.PartnerWorkStatus.Count == 0),
            PartnerPaidWorkOption.ParentalLeave => (nameof(PartnerParentalLeave), _journeyState.PartnerParentalLeaveChildrenIds.Count == 0),
            PartnerPaidWorkOption.SickLeave => (nameof(PartnerWorkStatus), _journeyState.PartnerWorkStatus.Count == 0),
            PartnerPaidWorkOption.No => (nameof(PartnerBenefits), _journeyState.PartnerBenefits.Count == 0),
            _ => throw new UnreachableException($"Unexpected PartnerPaidWork: {_journeyState.PartnerPaidWork}"),
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerParentalLeave(string? returnTo = null)
    {
        var backLink = GetPartnerParentalLeaveBackLink(returnTo);
        return View(new PartnerParentalLeaveViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerParentalLeave(PartnerParentalLeaveViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerParentalLeaveBackLink(model.ReturnTo);
            model.Children = _journeyState.Children.Values.ToList();
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAction = nameof(PartnerWorkStatus);
        var nextAnswerMissing = _journeyState.PartnerWorkStatus.Count == 0;

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(
            nextAction,
            new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerWorkStatus(string? returnTo = null)
    {
        var backLink = GetPartnerWorkStatusBackLink(returnTo);
        return View(new PartnerWorkStatusViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerWorkStatus(PartnerWorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerWorkStatusBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAction = nameof(PartnerWeeklyEarnings);
        var nextAnswerMissing = _journeyState.PartnerWeeklyEarnings is null;
        if (_journeyState.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            nextAction = nameof(PartnerSelfEmployedDuration);
            nextAnswerMissing = _journeyState.PartnerSelfEmployedDuration is null;
        }
        else if (_journeyState.PartnerPaidWork == PartnerPaidWorkOption.SickLeave)
        {
            nextAction = nameof(PartnerYearlyEarnings);
            nextAnswerMissing = _journeyState.PartnerYearlyEarnings is null;
        }

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerBenefits(string? returnTo = null)
    {
        var backLink = GetPartnerBenefitsBackLink(returnTo);
        return View(new PartnerBenefitsViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerBenefits(PartnerBenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerBenefitsBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAnswerMissing = _journeyState.PartnerChildcareSupport.Count == 0;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nameof(PartnerChildcareSupport), new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerSelfEmployedDuration(string? returnTo = null)
    {
        var backLink = GetPartnerSelfEmployedDurationBackLink(returnTo);
        return View(new PartnerSelfEmployedDurationViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerSelfEmployedDuration(PartnerSelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerSelfEmployedDurationBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        // Complex logic for sick leave falls through
        var nextAction = nameof(PartnerWeeklyEarnings);
        var nextAnswerMissing = _journeyState.PartnerWeeklyEarnings is null;

        if (_journeyState.PartnerPaidWork == PartnerPaidWorkOption.SickLeave)
        {
            nextAction = nameof(PartnerYearlyEarnings);
            nextAnswerMissing = _journeyState.PartnerYearlyEarnings is null;
        }

        if (_journeyState.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            nextAction = nameof(PartnerBenefits);
            nextAnswerMissing = _journeyState.PartnerBenefits.Count == 0;
        }

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerWeeklyEarnings(string? returnTo = null)
    {
        var backLink = GetPartnerWeeklyEarningsBackLink(returnTo);
        var weeklyEarningsThresholds = WeeklyEarningsThresholds.Create(_journeyState.PartnerAge, _journeyState.PartnerWorkStatus);
        var isOnParentalLeave = _journeyState.PartnerPaidWork == PartnerPaidWorkOption.ParentalLeave;
        return View(new PartnerWeeklyEarningsViewModel(_journeyState, weeklyEarningsThresholds, isOnParentalLeave, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerWeeklyEarnings(PartnerWeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.WeeklyEarningsThresholds = WeeklyEarningsThresholds.Create(_journeyState.PartnerAge, _journeyState.PartnerWorkStatus);
            model.IsOnParentalLeave = _journeyState.PartnerPaidWork == PartnerPaidWorkOption.ParentalLeave;
            model.BackLink = GetPartnerWeeklyEarningsBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var (nextAction, nextAnswerMissing) = _journeyState.PartnerWeeklyEarnings switch
        {
            WeeklyEarningsOption.AboveThreshold => (nameof(PartnerYearlyEarnings), _journeyState.PartnerYearlyEarnings is null),
            WeeklyEarningsOption.BelowThreshold => (nameof(PartnerBenefits), _journeyState.PartnerBenefits.Count == 0),
            _ => throw new UnreachableException($"Unexpected PartnerWeeklyEarnings: {_journeyState.PartnerWeeklyEarnings}"),
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerYearlyEarnings(string? returnTo = null)
    {
        var backLink = GetPartnerYearlyEarningsBackLink(returnTo);
        return View(new PartnerYearlyEarningsViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerYearlyEarnings(PartnerYearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerYearlyEarningsBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAnswerMissing = _journeyState.PartnerBenefits.Count == 0;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nameof(PartnerBenefits), new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PartnerChildcareSupport(string? returnTo = null)
    {
        var backLink = GetPartnerChildcareSupportBackLink(returnTo);
        return View(new PartnerChildcareSupportViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerChildcareSupport(PartnerChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerChildcareSupportBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (_journeyState.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers))
        {
            if (model.ReturnTo is not null && _journeyState.PartnerChildcareVoucherReceipt is not null)
            {
                return this.RedirectToReturnTo(model.ReturnTo);
            }

            return this.RedirectToAction(nameof(PartnerChildcareVoucherReceipt), new { returnTo = model.ReturnTo });
        }

        return this.RedirectToAction(nameof(SummaryController.CheckAnswers), SummaryController.Name);
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
        var backLink = GetPartnerChildcareVoucherReceiptBackLink(returnTo);
        return View(new PartnerChildcareVoucherReceiptViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerChildcareVoucherReceipt(PartnerChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerChildcareVoucherReceiptBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return this.RedirectToAction(nameof(SummaryController.CheckAnswers), SummaryController.Name);
    }

    private string GetPartnerAgeBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(UserController.HasPartner), UserController.Name);
    }

    private string GetPartnerNationalityBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(PartnerAge));
    }

    private string GetPartnerSettledStatusBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(PartnerNationality));
    }

    private string GetPartnerPaidWorkBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (_journeyState.Nationality == NationalityOption.BritishOrIrishCitizen)
        {
            return Url.ActionOrThrow(nameof(PartnerAge));
        }

        if (_journeyState.Nationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland
            && _journeyState.SettledStatus == SettledStatusOption.Yes)
        {
            return Url.ActionOrThrow(nameof(PartnerAge));
        }

        if (_journeyState.PartnerNationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland)
        {
            return Url.ActionOrThrow(nameof(PartnerSettledStatus));
        }

        return Url.ActionOrThrow(nameof(PartnerNationality));
    }

    private string GetPartnerParentalLeaveBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(PartnerPaidWork));
    }

    private string GetPartnerWorkStatusBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(PartnerPaidWork));
    }

    private string GetPartnerSelfEmployedDurationBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(PartnerWorkStatus));
    }

    private string GetPartnerWeeklyEarningsBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (_journeyState.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            return Url.ActionOrThrow(nameof(PartnerSelfEmployedDuration));
        }

        return Url.ActionOrThrow(nameof(PartnerWorkStatus));
    }

    private string GetPartnerYearlyEarningsBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(PartnerWeeklyEarnings));
    }

    private string GetPartnerBenefitsBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (_journeyState.PartnerYearlyEarnings == YearlyEarningsOption.AboveThreshold)
        {
            return Url.ActionOrThrow(nameof(PartnerYearlyEarnings));
        }
        else if (_journeyState.PartnerWeeklyEarnings == WeeklyEarningsOption.AboveThreshold)
        {
            return Url.ActionOrThrow(nameof(PartnerYearlyEarnings));
        }
        else if (_journeyState.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            return Url.ActionOrThrow(nameof(PartnerSelfEmployedDuration));
        }
        else if (_journeyState.PartnerPaidWork == PartnerPaidWorkOption.No)
        {
            return Url.ActionOrThrow(nameof(PartnerPaidWork));
        }

        return Url.ActionOrThrow(nameof(PartnerWeeklyEarnings));
    }

    private string GetPartnerChildcareSupportBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(PartnerBenefits));
    }

    private string GetPartnerChildcareVoucherReceiptBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(PartnerChildcareSupport));
    }
}
