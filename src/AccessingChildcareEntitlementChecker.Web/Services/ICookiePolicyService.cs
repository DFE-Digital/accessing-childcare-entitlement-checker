namespace AccessingChildcareEntitlementChecker.Web.Services;

public interface ICookiePolicyService
{
    bool HasConsented { get; }
    bool IsAnalyticsEnabled { get; set; }
}
