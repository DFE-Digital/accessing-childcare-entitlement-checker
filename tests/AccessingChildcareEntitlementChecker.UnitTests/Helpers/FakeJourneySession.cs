using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.UnitTests.Helpers;

public class FakeJourneySession : IJourneySession
{
    public JourneyState State { get; private set; } = new();

    public JourneyState Get()
    {
        return State;
    }

    public void Save(JourneyState state)
    {
        State = state;
    }
}