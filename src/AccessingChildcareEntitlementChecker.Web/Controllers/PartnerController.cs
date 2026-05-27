using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
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
    public IActionResult PartnerPaidWork()
    {
        return Content("<h1>Is your partner in paid work?</h1>", "text/html");
    }

    [HttpGet]
    public IActionResult PartnerSettledStatus()
    {
        return Content("<h1>Does your partner have settled or pre-settled status under the EU Settlement Scheme?</h1>", "text/html");
    }
}
