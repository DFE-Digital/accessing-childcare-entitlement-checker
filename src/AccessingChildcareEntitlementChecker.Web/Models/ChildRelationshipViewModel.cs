using System.Diagnostics.CodeAnalysis;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Models;

[ExcludeFromCodeCoverage(Justification = "Will be covered in subsequent pages")]
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
