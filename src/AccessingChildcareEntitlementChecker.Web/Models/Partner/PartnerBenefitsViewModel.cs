using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerBenefitsViewModel : IValidatableObject
{
    public PartnerBenefitsViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerBenefitsViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerBenefits = journeyState.PartnerBenefits;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Does your partner get any of these benefits?", Description = "Select all that apply.")]
    public List<PartnerBenefitsOption> PartnerBenefits { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(PartnerBenefitsViewModel));
        var isEmpty = PartnerBenefits.Count == 0;
        var selectedAndNone = PartnerBenefits.Contains(PartnerBenefitsOption.None) && PartnerBenefits.Count > 1;
        if (isEmpty || selectedAndNone)
        {
            yield return new ValidationResult(localizer["Select any benefits your partner gets, or select 'No, they do not get any of these benefits'"], [nameof(PartnerBenefits)]);
        }
    }
}
