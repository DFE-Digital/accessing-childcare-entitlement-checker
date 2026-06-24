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

    [Display(Name = "Is your adjusted net income more than £100,000 a year?", Description = "Adjusted net income is your total income before tax, minus certain tax reliefs.")]
    [Required(ErrorMessage = "Select if your adjusted net income is more than £100,000 a year")]
    public YearlyEarningsOption? YearlyEarnings { get; set; }
}
