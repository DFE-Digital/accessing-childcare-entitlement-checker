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

    public static string Name => "User";

    public UserController(JourneyState journeyState, IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public ViewResult UserAge()
    {
        return View(new UserAgeViewModel(_journeyState));
    }

    [HttpPost]
    public IActionResult UserAge(UserAgeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return this.RedirectTo<UserController>(nameof(Nationality));
    }

    [HttpGet]
    public IActionResult Nationality(string? returnTo = null)
    {
        return View(new NationalityViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult Nationality(NationalityViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
        return View(new SettledStatusViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult SettledStatus(SettledStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
        return View(new PaidWorkViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult PaidWork(PaidWorkViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
    [ExcludeFromCodeCoverage(Justification = "This page is a stub for a future page")]
    public IActionResult WorkStatus(string? returnTo = null)
    {
        return View(new WorkStatusViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult WorkStatus(WorkStatusViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
        return View(new SelfEmployedDurationViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult SelfEmployedDuration(SelfEmployedDurationViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
        return View(new YearlyEarningsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult YearlyEarnings(YearlyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
        return View(new WeeklyEarningsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult WeeklyEarnings(WeeklyEarningsViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
        return View(new UniversalCreditViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult UniversalCredit(UniversalCreditViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
        return View(new BenefitsViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult Benefits(BenefitsViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
        return View(new ChildcareSupportViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildcareSupport(ChildcareSupportViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
        return View(new ChildcareVoucherReceiptViewModel(_journeyState) { ReturnTo = returnTo });
    }

    [HttpPost]
    public IActionResult ChildcareVoucherReceipt(ChildcareVoucherReceiptViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
    public ViewResult HasPartner()
    {
        return View(new HasPartnerViewModel(_journeyState));
    }

    [HttpPost]
    public IActionResult HasPartner(HasPartnerViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
}
