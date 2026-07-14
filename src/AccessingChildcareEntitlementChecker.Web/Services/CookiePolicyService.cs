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
            var context = GetContextOrThrow();
            if (!context.Request.Cookies.TryGetValue(CookieName, out var value))
            {
                return false;
            }

            if (value != Enabled && value != Disabled)
            {
                return false;
            }

            return true;
        }
    }

    public bool IsAnalyticsEnabled
    {
        get
        {
            var context = GetContextOrThrow();
            if (!context.Request.Cookies.TryGetValue(CookieName, out var value))
            {
                return false;
            }

            return value == Enabled;
        }
        set
        {
            var context = GetContextOrThrow();
            var serialisedValue = value ? Enabled : Disabled;
            context.Response.Cookies.Append(
                CookieName,
                serialisedValue,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = !environment.IsDevelopment(),
                    SameSite = SameSiteMode.Lax,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });
        }

    }

    private HttpContext GetContextOrThrow() => httpContextAccessor.HttpContext ?? throw new InvalidOperationException("No active HTTP context.");
}