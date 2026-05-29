using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerWeeklyEarningsViewModel
{
    public string? ReturnTo { get; set; }

    public PartnerWeeklyEarningsViewModel()
    {
    }

    public PartnerWeeklyEarningsViewModel(JourneyState journeyState)
    {
        PartnerWeeklyEarnings = journeyState.PartnerWeeklyEarnings;
    }

    [Display(Name = "On average, does your partner earn £203 a week or more before tax?", Description = "This is the same as £879 a month or £10,556 a year. If your partner's income varies, use what they earn in a typical week.")]
    [Required(ErrorMessage = "Select if your partner earns £203 a week or more before tax")]
    public WeeklyEarningsOption? PartnerWeeklyEarnings { get; set; }
}
