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

    [Required(ErrorMessage = "Error_Select-your-age")]
    public AgeRange? UserAge { get; set; }
}