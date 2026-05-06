using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class ChildRelationshipViewModel
{
    public ChildRelationshipViewModel(JourneyState journeyState)
    {
        ChildName = journeyState.ChildName;
    }

    public string? ChildName { get; set; }
}
