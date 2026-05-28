using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class LocationViewModel
{
    public LocationViewModel()
    {

    }

    public LocationViewModel(JourneyState journeyState)
    {
        Country = journeyState.User.CountryOfResidence;
    }

    [Display(Name = "Where do you live?", Description = "Childcare support is different between different countries in the UK")]
    [Required(ErrorMessage = "Select where you live")]
    public CountryOfResidence? Country { get; set; }
}
