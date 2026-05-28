using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class JourneyStateTests
{
    private readonly JourneyState _journeyState;
    public JourneyStateTests()
    {
        _journeyState = new JourneyState();
        _journeyState.AddChild(new ChildState("Child A"));
    }

    [Fact]
    public void GetChild_ReturnsFalseIfChildDoesNotExist()
    {
        Assert.False(_journeyState.TryGetChild("non-existent-child-id", out var child));
    }
}
