using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class WeeklyEarningsViewModel
{
    public string? ReturnTo { get; set; }

    public WeeklyEarningsViewModel()
    {
    }

    public WeeklyEarningsViewModel(JourneyState journeyState)
    {
        WeeklyEarnings = journeyState.WeeklyEarnings;
    }

    [Display(Name = "On average, do you earn £203 a week or more before tax?", Description = "This is the same as £879 a month or £10,556 a year. If your income varies, use what you earn in a typical week.")]
    [Required(ErrorMessage = "Select if you earn £203 a week or more before tax")]
    public WeeklyEarningsOption? WeeklyEarnings { get; set; }
}
