using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Models.Partner;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
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

    [Display(Name = "Does your partner get any of these benefits?", Description = "Select all that apply")]
    public List<PartnerBenefitsOption> PartnerBenefits { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(PartnerBenefitsViewModel));
        var isEmpty = PartnerBenefits.Count == 0;
        if (isEmpty)
        {
            yield return new ValidationResult(localizer["Select any benefits your partner gets, or select 'No, they do not get any of these benefits'"], [nameof(PartnerBenefits)]);
        }

        var selectedAndNone = PartnerBenefits.Count > 1 && PartnerBenefits.Contains(PartnerBenefitsOption.None);
        if (selectedAndNone)
        {
            yield return new ValidationResult(localizer["You may not select 'No, they do not get any of these benefits' with other options"], [nameof(PartnerBenefits)]);
        }
    }
}
