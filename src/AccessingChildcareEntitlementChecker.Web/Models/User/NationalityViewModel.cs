using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class NationalityViewModel
{
    public string? ReturnTo { get; set; }

    public NationalityViewModel()
    {
    }

    public NationalityViewModel(JourneyState journeyState)
    {
        Nationality = journeyState.Nationality;
    }

    [Display(Name = "What is your nationality?")]
    [Required(ErrorMessage = "Select your nationality")]
    public NationalityOption? Nationality { get; set; }
}
