namespace AccessingChildcareEntitlementChecker.Web.Services;

public class CookiePolicyService(
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment environment) : ICookiePolicyService
{
    public const string CookieName = "cookie_policy";

    private const string Enabled = "enabled";

    private const string Disabled = "disabled";

    public bool HasConsented
    {
        get
        {
            var context = httpContextAccessor.HttpContext;
            if (context == null)
            {
                return false;
            }

            if (!context.Request.Cookies.TryGetValue(CookieName, out var value))
            {
                return false;
            }

            return value == Enabled;
        }
    }

    public bool HasUserPreference
    {
        get
        {
            var context = httpContextAccessor.HttpContext;
            if (context == null)
            {
                return false;
            }

            if (!context.Request.Cookies.TryGetValue(CookieName, out var value))
            {
                return false;
            }

            return value == Enabled || value == Disabled;
        }
    }

    public void SetConsentStatus(bool consented)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            return;
        }

        var serialisedValue = consented ? Enabled : Disabled;
        context.Response.Cookies.Append(
            CookieName,
            serialisedValue,
            new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                // Match the app's cookie convention (see securePolicy in Program.cs):
                // Always Secure in production, but SameAsRequest in Development so the
                // cookie isn't marked Secure over http. WebKit refuses to store a Secure
                // cookie received over an insecure connection (unlike Chromium/Firefox,
                // which treat localhost as trustworthy), which breaks consent on http.
                Secure = !environment.IsDevelopment() || context.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });
    }
}