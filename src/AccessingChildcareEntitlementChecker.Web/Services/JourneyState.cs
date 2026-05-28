using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class JourneyState
{
    public UserState User { get; set; } = new UserState();

    public Dictionary<Guid, ChildState> Children { get; set; } = [];

    public PartnerState Partner { get; set; } = new PartnerState();

    public void AddChild(ChildState childState)
    {
        if (Children.ContainsKey(childState.ChildId))
        {
            throw new InvalidOperationException($"A child with id {childState.ChildId} already exists");
        }

        Children[childState.ChildId] = childState;
    }

    public bool TryGetChild(Guid childId, [NotNullWhen(true)] out ChildState? child)
    {
        return Children.TryGetValue(childId, out child);
    }
}
