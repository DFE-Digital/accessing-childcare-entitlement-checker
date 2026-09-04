using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Models.BornChildDetails;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class ChildSupportViewModel : IValidatableObject
{
    public ChildSupportViewModel()
    {
        ChildId = string.Empty;
        BackLink = string.Empty;
    }

    public ChildSupportViewModel(Child child, string backLink, string? returnTo = null)
    {
        ChildId = child.ChildId;
        ChildSupportOptions = child.ChildSupportOptions;
        ChildName = child.Name;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    public string ChildId { get; set; }

    [BindNever]
    public string ChildName { get; set; } = string.Empty;

    [Display(Name = "Does {0} get any of the following support?", Description = "Select all that apply")]
    public List<ChildSupport> ChildSupportOptions { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory =
            validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;

        var localizer = localizerFactory!.Create(typeof(ChildSupportViewModel));

        var isEmpty = ChildSupportOptions.Count == 0;
        var selectedAndNone =
            ChildSupportOptions.Contains(ChildSupport.NoneOfTheseApply)
            && ChildSupportOptions.Count > 1;

        if (isEmpty || selectedAndNone)
        {
            yield return new ValidationResult(localizer["Select any support this child gets, or select 'No, none of these apply'"], [nameof(ChildSupportOptions)]);
        }
    }
}
