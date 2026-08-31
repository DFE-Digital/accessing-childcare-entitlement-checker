namespace Dfe.Acec.Web.Services;

public class CookiePolicyService(
    IHttpContextAccessor httpContextAccessor,
    CookieSecurePolicy securePolicy) : ICookiePolicyService
{
    private const string _cookieName = "cookie_policy";

    private const string _enabled = "enabled";

    private const string _disabled = "disabled";

    public bool HasConsented
    {
        get
        {
            var context = httpContextAccessor.HttpContext;
            if (context == null)
            {
                return false;
            }

            if (!context.Request.Cookies.TryGetValue(_cookieName, out var value))
            {
                return false;
            }

            return value == _enabled;
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

            if (!context.Request.Cookies.TryGetValue(_cookieName, out var value))
            {
                return false;
            }

            return value is _enabled or _disabled;
        }
    }

    public void SetConsentStatus(bool consented)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            return;
        }

        var serialisedValue = consented ? _enabled : _disabled;
        var cookieOptions = new CookieBuilder
        {
            Path = "/",
            HttpOnly = true,
            SecurePolicy = securePolicy,
            SameSite = SameSiteMode.Lax,
            IsEssential = true,
            Expiration = TimeSpan.FromDays(365),
        }.Build(context, DateTimeOffset.UtcNow);

        context.Response.Cookies.Append(_cookieName, serialisedValue, cookieOptions);
    }
}
