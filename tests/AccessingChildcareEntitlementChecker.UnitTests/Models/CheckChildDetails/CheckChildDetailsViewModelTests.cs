using AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.CheckChildDetails;

public class CheckChildDetailsViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly Guid _child1Id;
    private readonly Guid _child2Id;

    public CheckChildDetailsViewModelTests()
    {
        _journeyState = new JourneyState();
        _child1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        _child2Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var child1 = new ChildState(_child1Id, "Child 1");
        var child2 = new ChildState(_child2Id, "Child 2");
        _journeyState.AddChild(child1);
        _journeyState.AddChild(child2);
    }


    [Fact]
    public void ResolveLastEditedChild_ReturnsNull_WhenNoChildren()
    {
        var journeyState = new JourneyState();
        var viewModel = new CheckChildDetailsViewModel(journeyState, null);
        Assert.Null(viewModel.LastEditedChild);
    }

    [Fact]
    public void ResolveLastEditedChild_ReturnsChild_WhenFromChildIdMatches()
    {
        var viewModel = new CheckChildDetailsViewModel(_journeyState, _child1Id);
        Assert.Equal(_journeyState.Children[_child1Id], viewModel.LastEditedChild);
    }

    [Fact]
    public void YourChildrenMatchesJourneyStateChildren_AndHasCount()
    {
        var viewModel = new CheckChildDetailsViewModel(_journeyState, null);
        Assert.Equal(2, viewModel.YourChildren.Count);
        Assert.Contains(viewModel.YourChildren, c => c.ChildId == _child1Id && c.Title == "Child 1");
        Assert.Contains(viewModel.YourChildren, c => c.ChildId == _child2Id && c.Title == "Child 2");
    }

    [Fact]
    public void HasChildren_ReturnsTrue_WhenChildrenExist()
    {

        var viewModel = new CheckChildDetailsViewModel(_journeyState, null);
        Assert.True(viewModel.HasChildren);
    }
}
