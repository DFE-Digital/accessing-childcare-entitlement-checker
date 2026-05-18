using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class UserController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public UserController(
        JourneyState journeyState,
        IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

    [HttpGet]
    public IActionResult UserAge(string? returnTo = null)
    {
        return View(new UserAgeViewModel(_journeyState) { ReturnTo = returnTo });
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
        if (model.ReturnTo == "check-your-childrens-details")
        {
            return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails");
        }

        return RedirectToAction(nameof(UserController.Nationality), "User");
    }

    [HttpGet]
    public IActionResult PartnerNationality()
    {
        return View();
    }

    [HttpGet]
    public IActionResult TypeOfLeave()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Nationality()
    {
        return View();
    }
}
