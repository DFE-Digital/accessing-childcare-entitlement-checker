using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerYearlyEarningsViewModel
{
    public string? ReturnTo { get; set; }

    public PartnerYearlyEarningsViewModel()
    {
    }

    public PartnerYearlyEarningsViewModel(JourneyState journeyState)
    {
        PartnerYearlyEarnings = journeyState.PartnerYearlyEarnings;
    }

    [Display(Name = "Is your partner's adjusted net income more than £100,000 a year?", Description = "Adjusted net income is your total income before tax, minus certain tax reliefs.")]
    [Required(ErrorMessage = "Select if your partner's adjusted net income is more than £100,000 a year")]
    public YearlyEarningsOption? PartnerYearlyEarnings { get; set; }
}
