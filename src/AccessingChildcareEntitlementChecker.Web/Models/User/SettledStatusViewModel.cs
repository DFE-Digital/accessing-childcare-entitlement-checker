using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class SettledStatusViewModel
{
    public SettledStatusViewModel()
    {
        BackLink = string.Empty;
    }

    public SettledStatusViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        SettledStatus = journeyState.SettledStatus;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Do you have settled or pre-settled status under the EU Settlement Scheme?")]
    [Required(ErrorMessage = "Select if you have settled or pre-settled status")]
    public SettledStatusOption? SettledStatus { get; set; }
}
