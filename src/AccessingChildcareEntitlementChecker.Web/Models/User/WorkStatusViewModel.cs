using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class WorkStatusViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public WorkStatusViewModel()
    {
    }

    public WorkStatusViewModel(JourneyState journeyState)
    {
        WorkStatus = journeyState.User.WorkStatus;
    }

    [Display(Name = "How would you describe your work status?", Description = "Select all that apply.")]
    public List<WorkStatusOption> WorkStatus { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(WorkStatusViewModel));

        if (WorkStatus.Count == 0)
        {
            yield return new ValidationResult(localizer["Select your work status"], [nameof(WorkStatus)]);
        }
    }
}
