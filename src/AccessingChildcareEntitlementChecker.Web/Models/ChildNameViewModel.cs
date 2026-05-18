using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class ChildNameViewModel
{
    public string? ReturnTo { get; set; }

    public ChildNameViewModel()
    {
    }

    public ChildNameViewModel(string? childId, JourneyState journeyState)
    {
        ChildId = childId;
        if (childId is not null)
        {
            var child = journeyState.GetChild(childId);
            ChildName = child.Name;
        }
    }

    public string? ChildId { get; set; }

    [Display(Name = "What name should we use for this child?", Description = "You can use any name you want. For example, 'Baby Smith'.")]
    [Required(ErrorMessage = "Enter a name for your child")]
    public string? ChildName { get; set; }
}
