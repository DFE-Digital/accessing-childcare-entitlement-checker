using AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.CheckChildDetails;

public class CheckChildDetailsViewModelTests
{
    private readonly JourneyState _journeyState;

    public CheckChildDetailsViewModelTests()
    {
        _journeyState = new JourneyState();
        var child1 = new ChildState("Child 1");
        var child2 = new ChildState("Child 2");
        _journeyState.Children["child-1"] = child1;
        _journeyState.Children["child-2"] = child2;
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
        var viewModel = new CheckChildDetailsViewModel(_journeyState, "child-1");
        Assert.Equal(_journeyState.Children["child-1"], viewModel.LastEditedChild);
    }

    [Fact]
    public void YourChildrenMatchesJourneyStateChildren_AndHasCount()
    {
        var viewModel = new CheckChildDetailsViewModel(_journeyState, null);
        Assert.Equal(2, viewModel.YourChildren.Count);
        Assert.Contains(viewModel.YourChildren, c => c.ChildId == "child-1" && c.Title == "Child 1");
        Assert.Contains(viewModel.YourChildren, c => c.ChildId == "child-2" && c.Title == "Child 2");
    }

    [Fact]
    public void HasChildren_ReturnsTrue_WhenChildrenExist()
    {

        var viewModel = new CheckChildDetailsViewModel(_journeyState, null);
        Assert.True(viewModel.HasChildren);
    }
}
