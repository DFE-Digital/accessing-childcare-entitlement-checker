using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;

public class ExpectedChildRelationshipViewModel
{
    public string? ReturnTo { get; set; }

    public ExpectedChildRelationshipViewModel()
    {
        ChildId = string.Empty;
    }

    public ExpectedChildRelationshipViewModel(string childId, JourneyState journeyState)
    {
        var child = journeyState.GetChild(childId);
        ChildId = childId;
        ExpectedChildRelationship = child.ExpectedRelationship;
    }

    public string ChildId { get; set; }

    [Display(Name = "What will your relationship be to this child?")]
    [Required(ErrorMessage = "Select what your relationship will be to this child")]
    public Relationship? ExpectedChildRelationship { get; set; }
}
