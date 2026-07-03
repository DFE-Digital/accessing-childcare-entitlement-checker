using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class BenefitsViewModel : IValidatableObject
{
    public BenefitsViewModel()
    {
        BackLink = string.Empty;
    }

    public BenefitsViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        Benefits = journeyState.Benefits;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Do you get any of these benefits?", Description = "Select all that apply.")]
    public List<BenefitsOption> Benefits { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(BenefitsViewModel));
        var isEmpty = Benefits.Count == 0;
        var selectedAndNone = Benefits.Count > 1 && Benefits.Contains(BenefitsOption.None);
        if (isEmpty || selectedAndNone)
        {
            yield return new ValidationResult(localizer["Select any benefits you get, or select 'No, I do not get any of these benefits'"], [nameof(Benefits)]);
        }
    }
}
