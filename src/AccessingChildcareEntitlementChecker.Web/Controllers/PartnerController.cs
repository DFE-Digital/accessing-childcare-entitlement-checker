using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerNationality));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        var redirect = model.PartnerNationality switch
        {
            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland => this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerSettledStatus)),
            _ => this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerPaidWork)),
        };

        return redirect;
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerPaidWork));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        var redirect = model.PartnerPaidWork switch
        {
            PartnerPaidWorkOption.Yes => this.RedirectTo<PartnerController>(nameof(PartnerWorkStatus)),
            PartnerPaidWorkOption.OnLeave => this.RedirectTo<PartnerController>(nameof(PartnerTypeOfLeave)),
            PartnerPaidWorkOption.No => this.RedirectTo<PartnerController>(nameof(PartnerBenefits)),
            _ => throw new InvalidOperationException($"Unexpected PartnerPaidWorkOption value: {model.PartnerPaidWork}"),
        };

        return redirect;
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        if (model.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerSelfEmployedDuration));
        }

        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerWeeklyEarnings));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerChildcareSupport));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        var redirect = model.PartnerSelfEmployedDuration switch
        {
            SelfEmployedDurationOption.LessThan12Months => this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerBenefits)),
            _ => this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerWeeklyEarnings)),
        };

        return redirect;
    }

    [HttpGet]
    public IActionResult PartnerWeeklyEarnings(string? returnTo = null)
    {
        var backLink = GetPartnerWeeklyEarningsBackLink(returnTo);
        return View(new PartnerWeeklyEarningsViewModel(_journeyState, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult PartnerWeeklyEarnings(PartnerWeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLink = GetPartnerWeeklyEarningsBackLink(model.ReturnTo);
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        var redirect = model.PartnerWeeklyEarnings switch
        {
            WeeklyEarningsOption.AboveThreshold => this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerYearlyEarnings)),
            _ => this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerBenefits)),
        };

        return redirect;
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerBenefits));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        if (model.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers))
        {
            return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerChildcareVoucherReceipt));
        }

        return this.RedirectTo<SummaryController>(nameof(SummaryController.CheckAnswers));
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
        if (model.ReturnTo == ReturnTo.CheckAnswers)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<SummaryController>(nameof(SummaryController.CheckAnswers));
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

        if (_journeyState.PartnerNationality == NationalityOption.BritishOrIrishCitizen)
        {
            return Url.ActionOrThrow(nameof(PartnerAge));
        }

        if (_journeyState.PartnerNationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland)
        {
            if (_journeyState.SettledStatus == SettledStatusOption.Yes)
            {
                return Url.ActionOrThrow(nameof(PartnerAge));
            }

            return Url.ActionOrThrow(nameof(PartnerSettledStatus));
        }

        return Url.ActionOrThrow(nameof(PartnerNationality));
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
