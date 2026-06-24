using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class WeeklyEarningsViewModel
{
    public WeeklyEarningsViewModel()
    {
        BackLink = string.Empty;
    }

    public WeeklyEarningsViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        WeeklyEarnings = journeyState.WeeklyEarnings;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "On average, do you earn £203 a week or more before tax?", Description = "This is the same as £879 a month or £10,556 a year. If your income varies, use what you earn in a typical week.")]
    [Required(ErrorMessage = "Select if you earn £203 a week or more before tax")]
    public WeeklyEarningsOption? WeeklyEarnings { get; set; }
}
