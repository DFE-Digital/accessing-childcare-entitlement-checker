using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;

public class CheckChildDetailsViewModel
{
    public CheckChildDetailsViewModel(JourneyState journeyState, Guid? fromChildId)
    {
        YourChildren = [.. journeyState.Children.Values.Select(child => new ChildSummaryViewModel(child))];
        LastEditedChild = ResolveLastEditedChild(journeyState, fromChildId);
    }

    public IReadOnlyList<ChildSummaryViewModel> YourChildren { get; }

    public bool HasChildren => YourChildren.Count > 0;

    public ChildState? LastEditedChild { get; }

    private static ChildState? ResolveLastEditedChild(JourneyState journeyState, Guid? fromChildId)
    {
        if (fromChildId is not null && journeyState.Children.TryGetValue(fromChildId.Value, out var fromChild))
        {
            return fromChild;
        }

        return journeyState.Children.Values.LastOrDefault();
    }
}
