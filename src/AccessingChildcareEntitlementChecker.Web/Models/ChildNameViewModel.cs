using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class ChildNameViewModel
{
    public ChildNameViewModel()
    {

    }

    public ChildNameViewModel(JourneyState journeyState)
    {
        ChildName = journeyState.ChildName;
    }

    [Display(Name = "What name should we use for this child?", Description = "You can use any name you want. For example, 'Baby Smith'.")]
    [Required(ErrorMessage = "Enter a name for your child")]
    public string? ChildName { get; set; }
}
