using AccessingChildcareEntitlementChecker.Web.Models.Cookies;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class CookiesController(ICookiePolicyService cookiePolicyService) : Controller
{
    private readonly ICookiePolicyService _cookiePolicyService = cookiePolicyService;

    public const string Name = "Cookies";

    [HttpGet]
    public IActionResult Cookies(bool? hasSetCookies)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var analyticsEnabled = _cookiePolicyService.HasConsented;
        var cookiesViewModel = new CookiesViewModel(
            hasSetCookies ?? false,
            analyticsEnabled);
        return View(cookiesViewModel);
    }

    [HttpPost]
    public IActionResult Cookies(CookiesViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _cookiePolicyService.SetConsentStatus(model.AnalyticsCookiesEnabled ?? false);
        return RedirectToAction(nameof(Cookies), Name, new { hasSetCookies = true });
    }

    [HttpPost]
    public IActionResult BannerConsent(CookiesViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        _cookiePolicyService.SetConsentStatus(model.AnalyticsCookiesEnabled ?? false);
        return NoContent();
    }
}
