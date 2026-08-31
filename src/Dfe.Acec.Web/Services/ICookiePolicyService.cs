namespace Dfe.Acec.Web.Services;

public interface ICookiePolicyService
{
    public bool HasConsented { get; }
    public bool HasUserPreference { get; }
    public void SetConsentStatus(bool consented);
}
