using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class WeeklyEarningsViewModel : IValidatableObject
{
    public WeeklyEarningsViewModel()
    {
        BackLink = string.Empty;
        IsOnParentalLeave = false;
    }

    public WeeklyEarningsViewModel(
        JourneyState journeyState,
        WeeklyEarningsThresholds weeklyEarningsThresholds,
        bool isOnParentalLeave,
        string backLink,
        string? returnTo = null)
    {
        WeeklyEarnings = journeyState.WeeklyEarnings;
        WeeklyEarningsThresholds = weeklyEarningsThresholds;
        IsOnParentalLeave = isOnParentalLeave;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [BindNever]
    public WeeklyEarningsThresholds? WeeklyEarningsThresholds { get; set; }

    [BindNever]
    public bool IsOnParentalLeave { get; set; }

    public WeeklyEarningsOption? WeeklyEarnings { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var journeyState = validationContext.GetService(typeof(JourneyState)) as JourneyState;
        if (journeyState is null)
        {
            throw new InvalidOperationException($"JourneyState service is not available in the validation context for {nameof(WeeklyEarningsViewModel)}");
        }

        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(WorkStatusViewModel));
        var weeklyEarningsThresholds = WeeklyEarningsThresholds.Create(journeyState.UserAge, journeyState.WorkStatus);
        if (WeeklyEarnings == null)
        {
            yield return new ValidationResult(localizer["Select if you earn £{0} a week or more before tax", weeklyEarningsThresholds.PerWeek], [nameof(WeeklyEarnings)]);
        }
    }
}
