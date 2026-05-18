using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;

public class CheckChildDetailsViewModel
{
    public CheckChildDetailsViewModel(JourneyState journeyState, string? fromChildId)
    {
        YourChildren = [.. journeyState.Children.Values.Select(child => new ChildSummaryViewModel(child))];
        LastEditedChild = ResolveLastEditedChild(journeyState, fromChildId);
    }

    public IReadOnlyList<ChildSummaryViewModel> YourChildren { get; }

    public bool HasChildren => YourChildren.Count > 0;

    public Child? LastEditedChild { get; }

    private static Child? ResolveLastEditedChild(JourneyState journeyState, string? fromChildId)
    {
        if (fromChildId is not null && journeyState.Children.TryGetValue(fromChildId, out var fromChild))
        {
            return fromChild;
        }

        return journeyState.Children.Values.LastOrDefault();
    }
}
