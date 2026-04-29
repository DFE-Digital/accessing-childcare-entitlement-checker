using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class LocationViewModel
{
    public LocationViewModel()
    {

    }

    public LocationViewModel(JourneyState journeyState)
    {
        Country = journeyState.CountryOfResidence;
    }

    [Display(Name = "Title", Description = "Caption")]
    [Required(ErrorMessage = "Error_SelectLocation")]
    public CountryOfResidence? Country { get; set; }
}