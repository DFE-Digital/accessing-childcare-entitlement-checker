using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Models;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
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
