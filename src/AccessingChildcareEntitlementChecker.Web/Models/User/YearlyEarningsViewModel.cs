using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class YearlyEarningsViewModel
{
    public string? ReturnTo { get; set; }

    public YearlyEarningsViewModel()
    {
    }

    public YearlyEarningsViewModel(JourneyState journeyState)
    {
        YearlyEarnings = journeyState.User.YearlyEarnings;
    }

    [Display(Name = "Is your adjusted net income more than £100,000 a year?", Description = "Adjusted net income is your total income before tax, minus certain tax reliefs.")]
    [Required(ErrorMessage = "Select if your adjusted net income is more than £100,000 a year")]
    public YearlyEarningsOption? YearlyEarnings { get; set; }
}
