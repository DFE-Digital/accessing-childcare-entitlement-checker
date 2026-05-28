using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class ChildStateTests
{
    private readonly ChildState _childState;

    public ChildStateTests()
    {
        _childState = new ChildState("Child A");
    }

    [Fact]
    public void Apply_ChildName_ThrowsIfNoChildName()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            _childState.Apply(new ChildNameViewModel());
        });
    }

    [Fact]
    public void Apply_ChildName_SetsChildName()
    {
        var model = new ChildNameViewModel { ChildName = "Child A" };
        _childState.Apply(model);

        Assert.Equal(_childState.Name, model.ChildName);
    }
}
