using Dfe.Acec.Web.Models.Cookies;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web.Controllers;

public class CookiesController(ICookiePolicyService cookiePolicyService) : Controller
{
    public const string Name = "Cookies";

    [HttpGet]
    public IActionResult Cookies(bool? hasSetCookies)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var analyticsEnabled = cookiePolicyService.HasConsented;
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

        cookiePolicyService.SetConsentStatus(model.AnalyticsCookiesEnabled ?? false);
        return RedirectToAction(nameof(Cookies), Name, new { hasSetCookies = true });
    }

    [HttpPost]
    public IActionResult BannerConsent(CookiesViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        cookiePolicyService.SetConsentStatus(model.AnalyticsCookiesEnabled ?? false);
        return NoContent();
    }
}
