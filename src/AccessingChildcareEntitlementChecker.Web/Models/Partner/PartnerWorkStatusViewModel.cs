using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerWorkStatusViewModel : IValidatableObject
{
    public PartnerWorkStatusViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerWorkStatusViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerWorkStatus = journeyState.PartnerWorkStatus;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "How would you describe your partner's work status?", Description = "Select all that apply.")]
    public List<WorkStatusOption> PartnerWorkStatus { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(PartnerWorkStatusViewModel));
        var isEmpty = PartnerWorkStatus.Count == 0;
        if (isEmpty)
        {
            yield return new ValidationResult(localizer["Select how you would describe your partner's work status"], [nameof(PartnerWorkStatus)]);
        }
    }
}
