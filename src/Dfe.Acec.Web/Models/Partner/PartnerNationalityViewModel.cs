using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Models.Partner;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class PartnerNationalityViewModel : IValidatableObject
{
    public PartnerNationalityViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerNationalityViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerNationalityOptions = journeyState.PartnerNationalityOptions;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Which of these best describes your partners nationality?")]
    public List<NationalityOption> PartnerNationalityOptions { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(PartnerNationalityViewModel));
        var isEmpty = PartnerNationalityOptions.Count == 0;
        if (isEmpty)
        {
            yield return new ValidationResult(localizer["Select your partner's nationality"], [nameof(PartnerNationalityOptions)]);
        }
    }
}
