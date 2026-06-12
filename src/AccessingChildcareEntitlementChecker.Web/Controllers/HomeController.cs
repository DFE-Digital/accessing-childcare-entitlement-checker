using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class HomeController(
    JourneyState journeyState,
    IJourneySession journeySession,
    INavigationService navigationService)
    : Controller
{
    [HttpGet]
    public IActionResult SessionExpired()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Start()
    {
        return View();
    }

    [HttpGet]
    public ViewResult Location()
    {
        return View(new LocationViewModel(journeyState));
    }

    [HttpPost]
    public IActionResult Location(LocationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.Set(journeyState);
        return Redirect(navigationService.GetNextUrl(Page.Location));
    }
}
