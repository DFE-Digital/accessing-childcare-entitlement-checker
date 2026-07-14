namespace AccessingChildcareEntitlementChecker.Web.Models.Cookies;

public record CookiesViewModel(
    bool HasSetCookies,
    string ReturnUrl,
    bool IsAnalyticsEnabled);