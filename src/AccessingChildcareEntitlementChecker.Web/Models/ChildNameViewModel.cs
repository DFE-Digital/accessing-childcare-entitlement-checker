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

    [Display(Name = "Label_ChildName", Description = "Description_ChildName")]
    [Required(ErrorMessage = "Error_ChildName")]
    public string? ChildName { get; set; }
}