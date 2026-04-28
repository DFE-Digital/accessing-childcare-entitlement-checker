using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class UserAgeViewModel
{
    public UserAgeViewModel()
    {

    }

    public UserAgeViewModel(JourneyState journeyState)
    {
        UserAge = journeyState.UserAge;
    }

    [Required(ErrorMessage = "Error_SelectYourAge")]
    public AgeRange? UserAge { get; set; }
}