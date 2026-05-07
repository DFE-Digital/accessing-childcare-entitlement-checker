using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class ChildRelationshipViewModel
{
    public ChildRelationshipViewModel(JourneyState journeyState)
    {
        if (journeyState.ChildName == null)
        {
            throw new ArgumentNullException(nameof(journeyState.ChildName));
        }

        ChildName = journeyState.ChildName;
    }

    public string ChildName { get; set; }
}
