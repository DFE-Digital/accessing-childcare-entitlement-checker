using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerSelfEmployedDurationViewModel
{
    public PartnerSelfEmployedDurationViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerSelfEmployedDurationViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerSelfEmployedDuration = journeyState.PartnerSelfEmployedDuration;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Has your partner been self-employed for less than 12 months?", Description = "Think about when they first began working for themself")]
    [Required(ErrorMessage = "Select if your partner has been self-employed for less than 12 months")]
    public SelfEmployedDurationOption? PartnerSelfEmployedDuration { get; set; }
}
