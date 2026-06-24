using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerNationalityViewModel
{
    public PartnerNationalityViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerNationalityViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerNationality = journeyState.PartnerNationality;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Which of these best describes your partners nationality?")]
    [Required(ErrorMessage = "Select your partner's nationality")]
    public NationalityOption? PartnerNationality { get; set; }
}
