using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerYearlyEarningsViewModel
{
    public PartnerYearlyEarningsViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerYearlyEarningsViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerYearlyEarnings = journeyState.PartnerYearlyEarnings;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Does your partner expect their adjusted net income to be more than £100,000 for the current tax year?", Description = "Adjusted net income is your total income before tax, minus certain tax reliefs.")]
    [Required(ErrorMessage = "Select if your partner expects their adjusted net income to be more than £100,000 for the current tax year")]
    public YearlyEarningsOption? PartnerYearlyEarnings { get; set; }
}
