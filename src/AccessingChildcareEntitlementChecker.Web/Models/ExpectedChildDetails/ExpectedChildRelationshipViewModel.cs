using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;

public class ExpectedChildRelationshipViewModel
{
    public string? ReturnTo { get; set; }

    public ExpectedChildRelationshipViewModel()
    {
        ChildId = default!;
    }

    public ExpectedChildRelationshipViewModel(string? childId, JourneyState journeyState)
    {
        var child = journeyState.GetChild(childId);
        ChildId = childId;
        ExpectedChildRelationship = child.ExpectedRelationship;
    }

    public string? ChildId { get; set; }

    [Display(Name = "What will your relationship be to this child?")]
    [Required(ErrorMessage = "Select your expected relationship to the child")]
    public RelationshipOption? ExpectedChildRelationship { get; set; }
}
