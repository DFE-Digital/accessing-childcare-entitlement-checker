using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class LocationViewModel
{
    public LocationViewModel()
    {
        BackLink = string.Empty;
    }

    public LocationViewModel(JourneyState journeyState, string backlink, string? returnTo = null)
    {
        Country = journeyState.CountryOfResidence;
        BackLink = backlink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Required(ErrorMessage = "Error_SelectLocation")]
    public CountryOfResidence? Country { get; set; }
}
