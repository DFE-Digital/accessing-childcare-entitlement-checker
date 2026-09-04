using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.RulesEngine.Types;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Models.User;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class NationalityViewModel : IValidatableObject
{
    public NationalityViewModel()
    {
        BackLink = string.Empty;
    }

    public NationalityViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        NationalityOptions = journeyState.NationalityOptions;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "What is your nationality?")]
    public List<NationalityOption> NationalityOptions { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(NationalityViewModel));
        var isEmpty = NationalityOptions.Count == 0;
        if (isEmpty)
        {
            yield return new ValidationResult(localizer["Select your nationality"], [nameof(NationalityOptions)]);
        }
    }
}
