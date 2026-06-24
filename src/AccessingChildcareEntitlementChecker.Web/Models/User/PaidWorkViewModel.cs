using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class PaidWorkViewModel
{
    public PaidWorkViewModel()
    {
        BackLink = string.Empty;
    }

    public PaidWorkViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PaidWork = journeyState.PaidWork;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Are you in paid work?", Description = "Paid work includes being self-employed or freelance")]
    [Required(ErrorMessage = "Select if you are in paid work")]
    public PaidWorkOption? PaidWork { get; set; }
}
