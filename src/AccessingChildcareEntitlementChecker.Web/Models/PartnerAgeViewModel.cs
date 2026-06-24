using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class PartnerAgeViewModel
{
    public PartnerAgeViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerAgeViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerAge = journeyState.PartnerAge;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Required(ErrorMessage = "Error_SelectYourPartnersAge")]
    public AgeRange? PartnerAge { get; set; }
}
