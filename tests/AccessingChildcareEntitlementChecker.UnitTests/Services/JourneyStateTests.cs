using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class JourneyStateTests
{
    private readonly JourneyState _journeyState;
    public JourneyStateTests()
    {
        _journeyState = new JourneyState();
    }

    [Fact]
    public void GetChildReturnsNullIfChildDoesNotExist()
    {
        Assert.False(_journeyState.Children.TryGetValue("non-existent-child-id", out var child));
    }

    [Fact]
    public void ApplyChildNameThrowsIfNoChildName()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            _journeyState.Apply(new ChildNameViewModel());
        });
    }

    [Fact]
    public void ApplyChildNameSetsChildIdIfNull()
    {
        var model = new ChildNameViewModel { ChildName = "Child A" };
        _journeyState.Apply(model);

        Assert.NotNull(model.ChildId);
    }

    [Fact]
    public void ApplyChildNameAddsChildIdIfNotExisting()
    {
        var model = new ChildNameViewModel { ChildName = "Child A" };
        _journeyState.Apply(model);

        Assert.Single(_journeyState.Children.Keys);
    }
}
