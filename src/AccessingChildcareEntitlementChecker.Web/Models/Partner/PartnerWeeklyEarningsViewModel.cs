using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerWeeklyEarningsViewModel : IValidatableObject
{
    public PartnerWeeklyEarningsViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerWeeklyEarningsViewModel(
        JourneyState journeyState,
        WeeklyEarningsThresholds weeklyEarningsThresholds,
        string backLink,
        string? returnTo = null)
    {
        PartnerWeeklyEarnings = journeyState.PartnerWeeklyEarnings;
        WeeklyEarningsThresholds = weeklyEarningsThresholds;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [BindNever]
    public WeeklyEarningsThresholds? WeeklyEarningsThresholds { get; set; }

    public WeeklyEarningsOption? PartnerWeeklyEarnings { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var journeyState = validationContext.GetService(typeof(JourneyState)) as JourneyState;
        if (journeyState is null)
        {
            throw new InvalidOperationException($"JourneyState service is not available in the validation context for {nameof(WeeklyEarningsViewModel)}");
        }

        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(WorkStatusViewModel));
        var weeklyEarningsThresholds = WeeklyEarningsThresholds.Factory(journeyState.PartnerAge, journeyState.PartnerWorkStatus);
        if (PartnerWeeklyEarnings == null)
        {
            yield return new ValidationResult(localizer["Select if your partner earns £{0} a week or more before tax", weeklyEarningsThresholds.PerWeek], [nameof(PartnerWeeklyEarnings)]);
        }
    }
}
