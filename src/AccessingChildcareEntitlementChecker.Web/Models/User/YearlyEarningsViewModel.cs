using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class YearlyEarningsViewModel
{
    public YearlyEarningsViewModel()
    {
        BackLink = string.Empty;
    }

    public YearlyEarningsViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        YearlyEarnings = journeyState.YearlyEarnings;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Do you expect your adjusted net income to be more than £100,000 for the current tax year?", Description = "Adjusted net income is your total income before tax, minus certain tax reliefs.")]
    [Required(ErrorMessage = "Select if you expect your adjusted net income to be more than £100,000 for the current tax year")]
    public YearlyEarningsOption? YearlyEarnings { get; set; }
}
