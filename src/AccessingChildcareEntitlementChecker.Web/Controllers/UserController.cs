using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
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
        return this.RedirectTo<UserController>(nameof(Nationality));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return model.Nationality switch
        {
            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland => this.RedirectTo<UserController>(nameof(SettledStatus)),
            _ => this.RedirectTo<UserController>(nameof(PaidWork)),
        };
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<UserController>(nameof(PaidWork));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        var redirect = model.PaidWork switch
        {
            PaidWorkOption.Yes => this.RedirectTo<UserController>(nameof(WorkStatus)),
            PaidWorkOption.OnLeave => this.RedirectTo<UserController>(nameof(TypeOfLeave)),
            _ => this.RedirectTo<UserController>(nameof(UniversalCredit)),
        };

        return redirect;
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        if (model.WorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            return this.RedirectTo<UserController>(nameof(UserController.SelfEmployedDuration));
        }

        return this.RedirectTo<UserController>(nameof(UserController.WeeklyEarnings));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        var redirect = model.SelfEmployedDuration switch
        {
            SelfEmployedDurationOption.LessThan12Months => this.RedirectTo<UserController>(nameof(UserController.UniversalCredit)),
            _ => this.RedirectTo<UserController>(nameof(UserController.WeeklyEarnings)),
        };

        return redirect;
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        var redirect = model.YearlyEarnings switch
        {
            YearlyEarningsOption.AboveThreshold => this.RedirectTo<UserController>(nameof(UserController.Benefits)),
            _ => this.RedirectTo<UserController>(nameof(UserController.UniversalCredit)),
        };

        return redirect;
    }

    [HttpGet]
    public IActionResult WeeklyEarnings(string? returnTo = null)
    {
        var backLink = GetWeeklyEarningsBackLink(returnTo);
        return View(new WeeklyEarningsViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult WeeklyEarnings(WeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetWeeklyEarningsBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        var redirect = model.WeeklyEarnings switch
        {
            WeeklyEarningsOption.AboveThreshold => this.RedirectTo<UserController>(nameof(UserController.YearlyEarnings)),
            _ => this.RedirectTo<UserController>(nameof(UserController.UniversalCredit)),
        };

        return redirect;
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<UserController>(nameof(UserController.Benefits));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<UserController>(nameof(UserController.ChildcareSupport));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        if (model.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers))
        {
            return this.RedirectTo<UserController>(nameof(UserController.ChildcareVoucherReceipt));
        }

        return this.RedirectTo<UserController>(nameof(UserController.HasPartner));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<UserController>(nameof(UserController.HasPartner));
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

        if (_journeyState.HasPartner == true)
        {
            return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerAge));
        }

        return this.RedirectTo<SummaryController>(nameof(SummaryController.CheckAnswers));
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
