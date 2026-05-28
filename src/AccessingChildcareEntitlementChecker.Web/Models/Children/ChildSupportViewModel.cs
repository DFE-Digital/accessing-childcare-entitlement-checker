using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Children;

public class ChildSupportViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public ChildSupportViewModel()
    {
        ChildId = string.Empty;
    }

    public ChildSupportViewModel(ChildState child)
    {
        ChildId = child.ChildId;
        ChildName = child.Name;
        ChildSupportOptions = child.ChildSupportOptions;
    }

    public string ChildId { get; set; }

    [BindNever]
    public string ChildName { get; set; } = string.Empty;

    [Display(Name = "Does the child get any of the following support?", Description = "Select all that apply")]
    public List<ChildSupport> ChildSupportOptions { get; set; } = new List<ChildSupport>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var journeyState = validationContext.GetService(typeof(JourneyState)) as JourneyState;
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(ChildSupportViewModel));
        if (ChildSupportOptions.Count == 0)
        {
            if (!journeyState!.TryGetChild(ChildId, out var child))
            {
                throw new InvalidOperationException($"No child found with ID {ChildId}");
            }

            yield return new ValidationResult(localizer["Select any support {0} gets, or select 'No, none of these apply'", child.Name], [nameof(ChildSupportOptions)]);
        }

        if (ChildSupportOptions.Contains(ChildSupport.NoneOfTheseApply) && ChildSupportOptions.Count > 1)
        {
            yield return new ValidationResult(localizer["You may not select 'no, none of these apply' with other options"], [nameof(ChildSupportOptions)]);
        }
    }
}
