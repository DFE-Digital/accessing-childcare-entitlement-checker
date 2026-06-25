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

    [Display(Name = "Is your partner's adjusted net income more than £100,000 a year?", Description = "Adjusted net income is your total income before tax, minus certain tax reliefs.")]
    [Required(ErrorMessage = "Select if your partner's adjusted net income is more than £100,000 a year")]
    public YearlyEarningsOption? PartnerYearlyEarnings { get; set; }
}
