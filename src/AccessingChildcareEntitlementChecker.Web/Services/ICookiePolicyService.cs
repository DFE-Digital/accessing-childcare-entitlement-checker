namespace AccessingChildcareEntitlementChecker.Web.Services;

public interface ICookiePolicyService
{
    bool HasConsented { get; }
    bool HasUserPreference { get; }
    void SetConsentStatus(bool consented);
}
