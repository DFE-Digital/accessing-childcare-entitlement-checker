using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

    [Required(ErrorMessage = "Error_SelectLocation")]
    public CountryOfResidence? Country { get; set; }
}
