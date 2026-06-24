using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class PartnerSettledStatusViewModel
{
    public PartnerSettledStatusViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerSettledStatusViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerSettledStatus = journeyState.PartnerSettledStatus;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Does your partner have settled or pre-settled status under the EU Settlement Scheme?")]
    [Required(ErrorMessage = "Select if your partner has settled or pre-settled status")]
    public SettledStatusOption? PartnerSettledStatus { get; set; }
}
