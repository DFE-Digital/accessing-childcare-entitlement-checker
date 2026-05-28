using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class SettledStatusViewModel
{
    public string? ReturnTo { get; set; }

    public SettledStatusViewModel()
    {
    }

    public SettledStatusViewModel(JourneyState journeyState)
    {
        SettledStatus = journeyState.User.SettledStatus;
    }

    [Display(Name = "Do you have settled or pre-settled status under the EU Settlement Scheme?")]
    [Required(ErrorMessage = "Select your status")]
    public SettledStatusOption? SettledStatus { get; set; }
}
