using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class ChildcareSupportViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public ChildcareSupportViewModel()
    {
    }

    public ChildcareSupportViewModel(JourneyState journeyState)
    {
        ChildcareSupport = journeyState.ChildcareSupport;
    }

    [Display(Name = "Do you already get any of this childcare support?", Description = "Select all that apply.")]
    public List<ChildcareSupportOption> ChildcareSupport { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(ChildcareSupportViewModel));

        if (ChildcareSupport.Count == 0)
        {
            yield return new ValidationResult(localizer["Select any of this childcare support you already get, or select 'No, I do not get any of this childcare support'"], [nameof(ChildcareSupport)]);
        }

        if (ChildcareSupport.Contains(ChildcareSupportOption.None) && ChildcareSupport.Count > 1)
        {
            yield return new ValidationResult(localizer["Select any of this childcare support you already get, or select 'No, I do not get any of this childcare support'"], [nameof(ChildcareSupport)]);
        }
    }
}
