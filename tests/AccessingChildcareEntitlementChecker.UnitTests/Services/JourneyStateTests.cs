using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class JourneyStateTests
{
    private readonly JourneyState _journeyState;
    private readonly Guid _childAId;
    private readonly Guid _nonExistentChildId;

    public JourneyStateTests()
    {
        _childAId = Guid.Parse("00000000-0000-0000-0000-00000000000a");
        _nonExistentChildId = Guid.Parse("00000000-0000-0000-0000-00000000000b");
        _journeyState = new JourneyState();
        _journeyState.AddChild(new ChildState(_childAId, "Child A"));
    }

    [Fact]
    public void GetChild_ReturnsFalseIfChildDoesNotExist()
    {
        Assert.False(_journeyState.TryGetChild(_nonExistentChildId, out var child));
    }
}
