using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class UniversalCreditViewModel
{
    public UniversalCreditViewModel()
    {
        BackLink = string.Empty;
    }

    public UniversalCreditViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        UniversalCredit = journeyState.UniversalCredit;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Does your household receive universal credit?")]
    [Required(ErrorMessage = "Select if your household receives universal credit")]
    public UniversalCreditOption? UniversalCredit { get; set; }
}
