using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class WorkStatusViewModel : IValidatableObject
{
    public WorkStatusViewModel()
    {
        BackLink = string.Empty;
    }

    public WorkStatusViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        WorkStatus = journeyState.WorkStatus;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "How would you describe your work status?", Description = "Select all that apply.")]
    public List<WorkStatusOption> WorkStatus { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(WorkStatusViewModel));
        var isEmpty = WorkStatus.Count == 0;
        if (isEmpty)
        {
            yield return new ValidationResult(localizer["Select your work status"], [nameof(WorkStatus)]);
        }
    }
}
