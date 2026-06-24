using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class UserAgeViewModel
{
    public UserAgeViewModel()
    {
        BackLink = string.Empty;
    }

    public UserAgeViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        UserAge = journeyState.UserAge;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Required(ErrorMessage = "Error_SelectYourAge")]
    public AgeRange? UserAge { get; set; }
}
