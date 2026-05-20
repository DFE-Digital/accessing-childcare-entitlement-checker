namespace AccessingChildcareEntitlementChecker.Web.Services;

public interface IJourneySession
{
    JourneyState Get();
    void Set(JourneyState journeyState);
}
