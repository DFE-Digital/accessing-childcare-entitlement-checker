using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class UserController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public const string Name = "User";

    public UserController(JourneyState journeyState, IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public ViewResult UserAge(string? returnTo = null)
    {
        var backLink = GetUserAgeBackLink(returnTo);
        return View(new UserAgeViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult UserAge(UserAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetUserAgeBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        // Logic here a little complex because this changes not just the next question,
        // but also a question a couple of steps ahead. So we need to walk forward through
        // the journey.
        //
        // See also - PartnerController.PartnerAge
        var nationalityMissing = _journeyState.Nationality == null;
        var paidWorkMissing = _journeyState.PaidWork == null;
        var weeklyEarningsMissing = _journeyState.PaidWork == PaidWorkOption.Yes && _journeyState.WeeklyEarnings == null;
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

        return this.RedirectToAction(
            nextAction,
            new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult Nationality(string? returnTo = null)
    {
        var backLink = GetNationalityBackLink(returnTo);
        return View(new NationalityViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult Nationality(NationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetNationalityBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var (nextAction, nextAnswerMissing) = _journeyState.Nationality switch
        {
            NationalityOption.BritishOrIrishCitizen => (nameof(PaidWork), _journeyState.PaidWork == null),
            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland => (nameof(SettledStatus), _journeyState.SettledStatus == null),
            NationalityOption.CitizenOfADifferentCountry => (nameof(PaidWork), _journeyState.PaidWork == null),
            _ => throw new UnreachableException($"Unexpected nationality option: {_journeyState.Nationality}")
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(
            nextAction,
            new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult SettledStatus(string? returnTo = null)
    {
        var backLink = GetSettledStatusBackLink(returnTo);
        return View(new SettledStatusViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult SettledStatus(SettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetSettledStatusBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAnswerMissing = _journeyState.PaidWork == null;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(
            nameof(PaidWork),
            new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PaidWork(string? returnTo = null)
    {
        var backLink = GetPaidWorkBackLink(returnTo);
        return View(new PaidWorkViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PaidWork(PaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPaidWorkBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var (nextAction, nextAnswerMissing) = _journeyState.PaidWork switch
        {
            PaidWorkOption.Yes => (nameof(WorkStatus), _journeyState.WorkStatus.Count == 0),
            PaidWorkOption.ParentalLeave => (nameof(ParentalLeave), _journeyState.ParentalLeaveChildrenIds.Count == 0),
            PaidWorkOption.SickLeave => (nameof(WorkStatus), _journeyState.WorkStatus.Count == 0),
            PaidWorkOption.No => (nameof(UniversalCredit), _journeyState.UniversalCredit is null),
            _ => throw new UnreachableException($"Unexpected PaidWork: {_journeyState.PaidWork}"),
        };

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(
            nextAction,
            new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult ParentalLeave(string? returnTo = null)
    {
        var backLink = GetParentalLeaveBackLink(returnTo);
        return View(new ParentalLeaveViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ParentalLeave(ParentalLeaveViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetParentalLeaveBackLink(model.ReturnTo);
            model.Children = _journeyState.Children.Values.ToList();
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAction = nameof(WorkStatus);
        var nextAnswerMissing = _journeyState.WorkStatus.Count == 0;

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(
            nextAction,
            new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult WorkStatus(string? returnTo = null)
    {
        var backLink = GetWorkStatusBackLink(returnTo);
        return View(new WorkStatusViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult WorkStatus(WorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetWorkStatusBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        var nextAction = nameof(WeeklyEarnings);
        var nextAnswerMissing = _journeyState.WeeklyEarnings is null;
        if (_journeyState.WorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            nextAction = nameof(SelfEmployedDuration);
            nextAnswerMissing = _journeyState.SelfEmployedDuration is null;
        }
        else if (_journeyState.PaidWork == PaidWorkOption.SickLeave)
        {
            nextAction = nameof(YearlyEarnings);
            nextAnswerMissing = _journeyState.YearlyEarnings is null;
        }

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult SelfEmployedDuration(string? returnTo = null)
    {
        var backLink = GetSelfEmployedDurationBackLink(returnTo);
        return View(new SelfEmployedDurationViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult SelfEmployedDuration(SelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetSelfEmployedDurationBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        // Complex logic for sick leave falls through
        var nextAction = nameof(WeeklyEarnings);
        var nextAnswerMissing = _journeyState.WeeklyEarnings is null;

        if (_journeyState.PaidWork == PaidWorkOption.SickLeave)
        {
            nextAction = nameof(YearlyEarnings);
            nextAnswerMissing = _journeyState.YearlyEarnings is null;
        }

        if (_journeyState.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            nextAction = nameof(UniversalCredit);
            nextAnswerMissing = _journeyState.UniversalCredit is null;
        }

        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult YearlyEarnings(string? returnTo = null)
    {
        var backLink = GetYearlyEarningsBackLink(returnTo);
        return View(new YearlyEarningsViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult YearlyEarnings(YearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetYearlyEarningsBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var (nextAction, nextAnswerMissing) = _journeyState.YearlyEarnings switch
        {
            YearlyEarningsOption.AboveThreshold => (nameof(Benefits), _journeyState.Benefits.Count == 0),
            YearlyEarningsOption.BelowThreshold => (nameof(UniversalCredit), _journeyState.UniversalCredit is null),
            _ => throw new UnreachableException($"Unexpected YearlyEarnings: {_journeyState.YearlyEarnings}"),
        };
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult WeeklyEarnings(string? returnTo = null)
    {
        var backLink = GetWeeklyEarningsBackLink(returnTo);
        var weeklyEarningsThresholds = WeeklyEarningsThresholds.Factory(_journeyState.UserAge, _journeyState.WorkStatus);
        var isOnParentalLeave = _journeyState.PaidWork == PaidWorkOption.ParentalLeave;
        return View(new WeeklyEarningsViewModel(_journeyState, weeklyEarningsThresholds, isOnParentalLeave, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult WeeklyEarnings(WeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var weeklyEarningsThresholds = WeeklyEarningsThresholds.Factory(_journeyState.UserAge, _journeyState.WorkStatus);
            model.WeeklyEarningsThresholds = weeklyEarningsThresholds;
            model.IsOnParentalLeave = _journeyState.PaidWork == PaidWorkOption.ParentalLeave;
            model.BackLink = GetWeeklyEarningsBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var (nextAction, nextAnswerMissing) = _journeyState.WeeklyEarnings switch
        {
            WeeklyEarningsOption.AboveThreshold => (nameof(YearlyEarnings), _journeyState.YearlyEarnings is null),
            WeeklyEarningsOption.BelowThreshold => (nameof(UniversalCredit), _journeyState.UniversalCredit is null),
            _ => throw new UnreachableException($"Unexpected WeeklyEarnings: {_journeyState.WeeklyEarnings}"),
        };
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nextAction, new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult UniversalCredit(string? returnTo = null)
    {
        var backLink = GetUniversalCreditBackLink(returnTo);
        return View(new UniversalCreditViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult UniversalCredit(UniversalCreditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetUniversalCreditBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAnswerMissing = _journeyState.Benefits.Count == 0;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nameof(Benefits), new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult Benefits(string? returnTo = null)
    {
        var backLink = GetBenefitsBackLink(returnTo);
        return View(new BenefitsViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult Benefits(BenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetBenefitsBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAnswerMissing = _journeyState.ChildcareSupport.Count == 0;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nameof(ChildcareSupport), new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult ChildcareSupport(string? returnTo = null)
    {
        var backLink = GetChildcareSupportBackLink(returnTo);
        return View(new ChildcareSupportViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ChildcareSupport(ChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetChildcareSupportBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (_journeyState.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers))
        {
            if (model.ReturnTo is not null && _journeyState.ChildcareVoucherReceipt is not null)
                return this.RedirectToReturnTo(model.ReturnTo);
            return this.RedirectToAction(nameof(ChildcareVoucherReceipt), new { returnTo = model.ReturnTo });
        }

        if (model.ReturnTo is not null && _journeyState.HasPartner is not null)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nameof(HasPartner), new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public IActionResult ChildcareVoucherReceipt(string? returnTo = null)
    {
        var backLink = GetChildcareVoucherReceiptBackLink(returnTo);
        return View(new ChildcareVoucherReceiptViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ChildcareVoucherReceipt(ChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetChildcareVoucherReceiptBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAnswerMissing = _journeyState.HasPartner is null;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectToAction(nameof(HasPartner), new { returnTo = model.ReturnTo });
    }

    [HttpGet]
    public ViewResult HasPartner(string? returnTo = null)
    {
        var backLink = GetHasPartnerBackLink(returnTo);
        return View(new HasPartnerViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult HasPartner(HasPartnerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetHasPartnerBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        var nextAnswerMissing = _journeyState.HasPartner == true && _journeyState.PartnerAge is null;
        if (model.ReturnTo is not null && !nextAnswerMissing)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        if (_journeyState.HasPartner == true)
        {
            return this.RedirectToAction(
                nameof(PartnerController.PartnerAge),
                PartnerController.Name,
                new { returnTo = model.ReturnTo });
        }

        return this.RedirectToAction(
            nameof(SummaryController.CheckAnswers),
            SummaryController.Name,
            new { returnTo = model.ReturnTo });
    }

    private string GetUserAgeBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(SummaryController.CheckChildDetails), SummaryController.Name);
    }

    private string GetNationalityBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(UserAge));
    }

    private string GetSettledStatusBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(Nationality));
    }

    private string GetPaidWorkBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (_journeyState.Nationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland)
        {
            return Url.ActionOrThrow(nameof(SettledStatus));
        }

        return Url.ActionOrThrow(nameof(Nationality));
    }

    private string GetParentalLeaveBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(PaidWork));
    }

    private string GetWorkStatusBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(PaidWork));
    }

    private string GetSelfEmployedDurationBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(WorkStatus));
    }

    private string GetWeeklyEarningsBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (_journeyState.WorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            return Url.ActionOrThrow(nameof(SelfEmployedDuration));
        }

        return Url.ActionOrThrow(nameof(WorkStatus));
    }

    private string GetYearlyEarningsBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(WeeklyEarnings));
    }

    private string GetUniversalCreditBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (_journeyState.PaidWork == PaidWorkOption.No)
        {
            return Url.ActionOrThrow(nameof(PaidWork));
        }
        else if (_journeyState.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            return Url.ActionOrThrow(nameof(SelfEmployedDuration));
        }
        else if (_journeyState.WeeklyEarnings == WeeklyEarningsOption.AboveThreshold)
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

        if (_journeyState.YearlyEarnings == YearlyEarningsOption.AboveThreshold)
        {
            return Url.Action(nameof(YearlyEarnings), Name)
                ?? throw new InvalidOperationException("Unable to generate back link");
        }

        return Url.ActionOrThrow(nameof(UniversalCredit));
    }

    private string GetChildcareSupportBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(Benefits));
    }

    private string GetChildcareVoucherReceiptBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        return Url.ActionOrThrow(nameof(ChildcareSupport));
    }

    private string GetHasPartnerBackLink(string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, out var url))
        {
            return url;
        }

        if (_journeyState.ChildcareSupport != null && _journeyState.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers))
        {
            return Url.ActionOrThrow(nameof(ChildcareVoucherReceipt));
        }

        return Url.ActionOrThrow(nameof(ChildcareSupport));
    }
}
