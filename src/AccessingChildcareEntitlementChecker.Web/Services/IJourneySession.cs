namespace AccessingChildcareEntitlementChecker.Web.Services;

public interface IJourneySession
{
    bool HasSession { get; }
    JourneyState Get();
    void Set(JourneyState journeyState);
    void Clear();
}
