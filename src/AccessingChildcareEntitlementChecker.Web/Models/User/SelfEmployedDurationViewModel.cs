using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class SelfEmployedDurationViewModel
{
    public string? ReturnTo { get; set; }

    public SelfEmployedDurationViewModel()
    {
    }

    public SelfEmployedDurationViewModel(JourneyState journeyState)
    {
        SelfEmployedDuration = journeyState.User.SelfEmployedDuration;
    }

    [Display(Name = "Have you been self-employed for less than 12 months?", Description = "Think about when you first started doing self-employed work, even if there have been some breaks")]
    [Required(ErrorMessage = "Select if you have been self-employed for less than 12 months")]
    public SelfEmployedDurationOption? SelfEmployedDuration { get; set; }
}
