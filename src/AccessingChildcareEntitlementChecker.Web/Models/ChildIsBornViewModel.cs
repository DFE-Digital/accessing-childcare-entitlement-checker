using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class ChildIsBornViewModel
{
    public string? ReturnTo { get; set; }

    public ChildIsBornViewModel()
    {
        ChildId = default!;
    }

    public ChildIsBornViewModel(string? childId, JourneyState journeyState)
    {
        var child = journeyState.GetChild(childId);
        ChildId = childId;
        IsChildBorn = child.BirthStatus;
    }

    public string? ChildId { get; set; }

    [Display(Name = "Has this child been born yet?")]
    [Required(ErrorMessage = "Select if this child has been born")]
    public BirthStatusOption? IsChildBorn { get; set; }
}
