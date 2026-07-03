using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerChildcareSupportViewModel : IValidatableObject
{
    public PartnerChildcareSupportViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerChildcareSupportViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerChildcareSupport = journeyState.PartnerChildcareSupport;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Does your partner already get any of these to help pay for childcare?", Description = "Select all that apply.")]
    public List<PartnerChildcareSupportOption> PartnerChildcareSupport { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(PartnerChildcareSupportViewModel));
        var isEmpty = PartnerChildcareSupport.Count == 0;
        var selectedAndNone = PartnerChildcareSupport.Count > 1 && PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.None);
        if (isEmpty || selectedAndNone)
        {
            yield return new ValidationResult(localizer["Select any of this childcare support your partner already gets, or select 'No, they do not get any of these'"], [nameof(PartnerChildcareSupport)]);
        }
    }
}
