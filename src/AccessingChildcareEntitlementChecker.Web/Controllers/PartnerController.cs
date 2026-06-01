using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using System.Diagnostics.CodeAnalysis;
namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class PartnerController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public PartnerController(JourneyState journeyState, IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public ViewResult PartnerAge()
    {
        return View(new PartnerAgeViewModel(_journeyState));
    }

    [HttpPost]
    public IActionResult PartnerAge(PartnerAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerNationality));
    }

    [HttpGet]
    public IActionResult PartnerNationality(string? returnTo = null)
    {
        return View(new PartnerNationalityViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerNationality(PartnerNationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

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
        return View(new PartnerSettledStatusViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerSettledStatus(PartnerSettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerPaidWork));
    }

    [HttpGet]
    public IActionResult PartnerPaidWork(string? returnTo = null)
    {
        return View(new PartnerPaidWorkViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerPaidWork(PartnerPaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
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
        return View(new PartnerWorkStatusViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerWorkStatus(PartnerWorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        if (model.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerSelfEmployedDuration));
        }

        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerWeeklyEarnings));
    }

    [HttpGet]
    public IActionResult PartnerBenefits(string? returnTo = null)
    {
        return View(new PartnerBenefitsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerBenefits(PartnerBenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerChildcareSupport));
    }

    [HttpGet]
    public IActionResult PartnerSelfEmployedDuration(string? returnTo = null)
    {
        return View(new PartnerSelfEmployedDurationViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerSelfEmployedDuration(PartnerSelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

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
        return View(new PartnerWeeklyEarningsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerWeeklyEarnings(PartnerWeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

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
        return View(new PartnerYearlyEarningsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerYearlyEarnings(PartnerYearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerBenefits));
    }

    [HttpGet]
    public IActionResult PartnerChildcareSupport(string? returnTo = null)
    {
        return View(new PartnerChildcareSupportViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerChildcareSupport(PartnerChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        if (model.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers))
        {
            return this.RedirectTo<PartnerController>(nameof(PartnerController.PartnerChildcareVoucherReceipt));
        }

        return this.RedirectTo<CheckAnswersController>(nameof(CheckAnswersController.CheckAnswers));
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
        return View(new PartnerChildcareVoucherReceiptViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PartnerChildcareVoucherReceipt(PartnerChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);

        return this.RedirectTo<CheckAnswersController>(nameof(CheckAnswersController.CheckAnswers));
    }
}
