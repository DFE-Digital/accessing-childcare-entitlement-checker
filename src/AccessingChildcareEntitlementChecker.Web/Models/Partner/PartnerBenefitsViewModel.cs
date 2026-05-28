using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerBenefitsViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public PartnerBenefitsViewModel()
    {
    }

    public PartnerBenefitsViewModel(JourneyState journeyState)
    {
        PartnerBenefits = journeyState.Partner.PartnerBenefits;
    }

    [Display(Name = "Does your partner get any of these benefits?", Description = "Select all that apply.")]
    public List<PartnerBenefitsOption> PartnerBenefits { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(PartnerBenefitsViewModel));

        if (PartnerBenefits.Count == 0)
        {
            yield return new ValidationResult(localizer["Select any benefits your partner gets, or select 'No, they do not get any of these benefits'"], [nameof(PartnerBenefits)]);
        }

        if (PartnerBenefits.Contains(PartnerBenefitsOption.None) && PartnerBenefits.Count > 1)
        {
            yield return new ValidationResult(localizer["Select any benefits your partner gets, or select 'No, I do not get any of these benefits'"], [nameof(PartnerBenefits)]);
        }
    }
}
