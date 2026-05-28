using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class UserAgeViewModel
{
    public UserAgeViewModel()
    {

    }

    public UserAgeViewModel(JourneyState journeyState)
    {
        UserAge = journeyState.User.UserAge;
    }

    [Display(Name = "What is your age?")]
    [Required(ErrorMessage = "Select your age")]
    public AgeRange? UserAge { get; set; }
}
