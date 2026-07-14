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
    public ViewResult Cookies(bool? hasSetCookies, string? returnUrl)
    {
        var cookiesCookie = _cookiePolicyService.Get();
        returnUrl = returnUrl ?? Url.ActionOrThrow(nameof(HomeController.Start), HomeController.Name);
        var cookiesViewModel = new CookiesViewModel(
            hasSetCookies ?? false,
            returnUrl,
            cookiesCookie.Analytics);
        return View(cookiesViewModel);
    }

    [HttpPost]
    public IActionResult Cookies(CookiesViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _cookiePolicyService.Set(new CookiePolicy
        {
            Analytics = model.IsAnalyticsEnabled,
        });

        return RedirectToAction(nameof(Cookies), Name, new { hasSetCookies = true, model.ReturnUrl });
    }

    [HttpPost]
    public IActionResult BannerConsent(bool analytics, string returnUrl)
    {
        _cookiePolicyService.Set(new CookiePolicy(analytics));
        return LocalRedirect(returnUrl);
    }
}
