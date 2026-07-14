namespace AccessingChildcareEntitlementChecker.Web.Services;

public record CookiePolicy(
    bool Analytics = false,
    int Version = CookiePolicy.CurrentVersion)
{
    public const int CurrentVersion = 1;
}