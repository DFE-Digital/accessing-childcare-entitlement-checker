using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class ChildcareSupportViewModel : IValidatableObject
{
    public ChildcareSupportViewModel()
    {
        BackLink = string.Empty;
    }

    public ChildcareSupportViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        ChildcareSupport = journeyState.ChildcareSupport;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Do you already get any of this childcare support?", Description = "Select all that apply.")]
    public List<ChildcareSupportOption> ChildcareSupport { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(ChildcareSupportViewModel));
        var isEmpty = ChildcareSupport.Count == 0;
        var selectedAndNone = ChildcareSupport.Count > 1 && ChildcareSupport.Contains(ChildcareSupportOption.None);
        if (isEmpty || selectedAndNone)
        {
            yield return new ValidationResult(localizer["Select any of this childcare support you already get, or select 'No, I do not get any of this childcare support'"], [nameof(ChildcareSupport)]);
        }
    }
}
