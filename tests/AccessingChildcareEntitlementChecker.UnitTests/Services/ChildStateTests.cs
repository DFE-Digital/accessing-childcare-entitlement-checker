using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class ChildStateTests
{
    private readonly ChildState _childState;

    private readonly Guid _childAId;

    public ChildStateTests()
    {
        _childAId = Guid.Parse("00000000-0000-0000-0000-00000000000a");
        _childState = new ChildState(_childAId, "Child A");
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
