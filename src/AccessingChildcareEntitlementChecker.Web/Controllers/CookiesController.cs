using AccessingChildcareEntitlementChecker.Web.Models.Cookies;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class CookiesController : Controller
{
    private readonly ICookiePolicyService _cookiePolicyService;

    public const string Name = "Cookies";

    public CookiesController(ICookiePolicyService cookiePolicyService)
    {
        _cookiePolicyService = cookiePolicyService;
    }

    [HttpGet]
    public ViewResult Cookies(bool? hasSetCookies)
    {
        var analyticsEnabled = _cookiePolicyService.IsAnalyticsEnabled;
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

        _cookiePolicyService.IsAnalyticsEnabled = model.AnalyticsCookiesEnabled;

        return RedirectToAction(nameof(Cookies), Name, new { hasSetCookies = true });
    }

    [HttpPost]
    public IActionResult BannerConsent(bool analyticsEnabled, string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            BadRequest();
        }

        _cookiePolicyService.IsAnalyticsEnabled = analyticsEnabled;
        return LocalRedirect(returnUrl);
    }
}
