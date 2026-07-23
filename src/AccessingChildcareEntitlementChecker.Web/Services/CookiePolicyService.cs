namespace AccessingChildcareEntitlementChecker.Web.Services;

public class CookiePolicyService(
    IHttpContextAccessor httpContextAccessor,
    CookieSecurePolicy securePolicy) : ICookiePolicyService
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
        var cookieOptions = new CookieBuilder
        {
            Path = "/",
            HttpOnly = true,
            SecurePolicy = securePolicy,
            SameSite = SameSiteMode.Lax,
            IsEssential = true,
            Expiration = TimeSpan.FromDays(365),
        }.Build(context, DateTimeOffset.UtcNow);

        context.Response.Cookies.Append(CookieName, serialisedValue, cookieOptions);
    }
}