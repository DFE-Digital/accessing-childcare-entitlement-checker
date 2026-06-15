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
    private readonly NavigationService _navigationService;

    public UserController(
        JourneyState journeyState,
        IJourneySession journeySession,
        NavigationService navigationService)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
        _navigationService = navigationService;
    }

    [HttpGet]
    public IActionResult UserAge(string? returnTo = null)
    {
        if (!_navigationService.UserAgeValid())
        {
            return this.RedirectTo<HomeController>(nameof(HomeController.Start));
        }

        var back = _navigationService.UserAgePrevious(returnTo);
        return ViewWithBackLink(new UserAgeViewModel(_journeyState) { ReturnTo = returnTo }, back);
    }

    [HttpPost]
    public IActionResult UserAge(UserAgeViewModel model)
    {
        if (!_navigationService.UserAgeValid())
        {
            return this.RedirectTo<HomeController>(nameof(HomeController.Start));
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _navigationService.UserAgeNext(model.ReturnTo);
    }

    [HttpGet]
    public IActionResult Nationality(string? returnTo = null)
    {
        if (!_navigationService.NationalityValid())
        {
            return this.RedirectTo<HomeController>(nameof(HomeController.Start));
        }

        var back = _navigationService.NationalityPrevious(returnTo);
        return ViewWithBackLink(new NationalityViewModel(_journeyState) { ReturnTo = returnTo }, back);
    }

    [HttpPost]
    public IActionResult Nationality(NationalityViewModel model)
    {
        if (!_navigationService.NationalityValid())
        {
            return this.RedirectTo<HomeController>(nameof(HomeController.Start));
        }

        if (!ModelState.IsValid)
        {
            var back = _navigationService.NationalityPrevious(model.ReturnTo);
            return ViewWithBackLink(model, back);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _navigationService.NationalityNext(model.ReturnTo);
    }

    [HttpGet]
    public IActionResult SettledStatus(string? returnTo = null)
    {
        if (!_navigationService.SettledStatusValid())
        {
            return this.RedirectTo<HomeController>(nameof(HomeController.Start));
        }

        var back = _navigationService.SettledStatusPrevious(returnTo);
        return ViewWithBackLink(new SettledStatusViewModel(_journeyState) { ReturnTo = returnTo }, back);
    }

    [HttpPost]
    public IActionResult SettledStatus(SettledStatusViewModel model)
    {
        if (!_navigationService.SettledStatusValid())
        {
            return this.RedirectTo<HomeController>(nameof(HomeController.Start));
        }

        if (!ModelState.IsValid)
        {
            var back = _navigationService.SettledStatusPrevious(model.ReturnTo);
            return ViewWithBackLink(model, back);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        if (model.ReturnTo != null)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        return this.RedirectTo<UserController>(nameof(UserController.PaidWork), new { model.ReturnTo });
    }

    [HttpGet]
    public IActionResult PaidWork(string? returnTo = null)
    {
        if (!_navigationService.PaidWorkValid())
        {
            return this.RedirectTo<HomeController>(nameof(HomeController.Start));
        }

        var back = _navigationService.PaidWorkPrevious(returnTo);
        return ViewWithBackLink(new PaidWorkViewModel(_journeyState) { ReturnTo = returnTo }, back);
    }

    [HttpPost]
    public IActionResult PaidWork(PaidWorkViewModel model)
    {
        if (!_navigationService.PaidWorkValid())
        {
            return this.RedirectTo<HomeController>(nameof(HomeController.Start));
        }

        if (!ModelState.IsValid)
        {
            var back = _navigationService.PaidWorkPrevious(model.ReturnTo);
            return ViewWithBackLink(model, back);
        }

        _journeyState.Apply(model);
        _journeySession.Set(_journeyState);
        return _navigationService.PaidWorkNext(model.ReturnTo);
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

    private ViewResult ViewWithBackLink(
        object model,
        ActionLink back)
    {
        ViewData["BackLinkController"] = back.Controller;
        ViewData["BackLinkAction"] = back.Action;
        ViewData["BackLinkRouteValues"] = new { back.returnTo };
        return View(model);
    }
}
