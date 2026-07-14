namespace AccessingChildcareEntitlementChecker.Web.Services;

using System.Text.Json;

public sealed class CookiePolicyService(
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment environment) : ICookiePolicyService
{
    public const string CookieName = "cookie_policy";

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public CookiePolicy Get()
    {
        var context = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No active HTTP context.");

        if (!context.Request.Cookies.TryGetValue(CookieName, out var value))
        {
            return new CookiePolicy();

        }

        try
        {
            var policy = JsonSerializer.Deserialize<CookiePolicy>(value, JsonOptions);

            return policy?.Version == CookiePolicy.CurrentVersion
                ? policy
                : new CookiePolicy();
        }
        catch (JsonException)
        {
            return new CookiePolicy();
        }
    }

    public void Set(CookiePolicy policy)
    {
        var context = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No active HTTP context.");

        var serialisedCookie = JsonSerializer.Serialize(policy, JsonOptions);
        context.Response.Cookies.Append(
            CookieName,
            serialisedCookie,
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