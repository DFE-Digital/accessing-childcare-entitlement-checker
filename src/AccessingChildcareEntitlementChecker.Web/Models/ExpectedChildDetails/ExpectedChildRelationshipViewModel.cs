using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;

public class ExpectedChildRelationshipViewModel
{
    public ExpectedChildRelationshipViewModel()
    {

    }

    public ExpectedChildRelationshipViewModel(JourneyState journeyState)
    {
        ExpectedChildRelationship = journeyState.ExpectedChildRelationship;
    }

    [Display(Name = "What will your relationship be to this child?")]
    [Required(ErrorMessage = "Select what your relationship will be to this child")]
    public Relationship? ExpectedChildRelationship { get; set; }
}
