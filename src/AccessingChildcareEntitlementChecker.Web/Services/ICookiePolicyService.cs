namespace AccessingChildcareEntitlementChecker.Web.Services;

public interface ICookiePolicyService
{
    CookiePolicy Get();
    void Set(CookiePolicy policy);
}
