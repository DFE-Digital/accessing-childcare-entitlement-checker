namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation;

public interface INavigationService
{
    string GetNextUrl(Page currentPage, string? returnTo = null, string? childId = null);
    string GetBackUrl(Page currentPage, string? returnTo = null, string? childId = null);
}
