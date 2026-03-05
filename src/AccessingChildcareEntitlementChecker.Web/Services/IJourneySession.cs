namespace AccessingChildcareEntitlementChecker.Web.Services;

public interface IJourneySession
{
    JourneyState Get();
    void Save(JourneyState state);
}