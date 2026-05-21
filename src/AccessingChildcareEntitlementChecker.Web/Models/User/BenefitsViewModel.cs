using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class BenefitsViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public BenefitsViewModel()
    {
    }

    public BenefitsViewModel(JourneyState journeyState)
    {
        Benefits = journeyState.Benefits;
    }

    [Display(Name = "Do you get any of these benefits?", Description = "Select all that apply.")]
    public List<BenefitsOption> Benefits { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(BenefitsViewModel));

        if (Benefits.Count == 0)
        {
            yield return new ValidationResult(localizer["Select any benefits you get, or select 'No, I do not get any of these benefits'"], [nameof(Benefits)]);
        }

        if (Benefits.Contains(BenefitsOption.None) && Benefits.Count > 1)
        {
            yield return new ValidationResult(localizer["Select any benefits you get, or select 'No, I do not get any of these benefits'"], [nameof(Benefits)]);
        }
    }
}
