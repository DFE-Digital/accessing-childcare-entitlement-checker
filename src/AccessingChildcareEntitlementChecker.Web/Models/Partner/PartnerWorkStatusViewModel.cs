using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerWorkStatusViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public PartnerWorkStatusViewModel()
    {
    }

    public PartnerWorkStatusViewModel(JourneyState journeyState)
    {
        PartnerWorkStatus = journeyState.PartnerWorkStatus;
    }

    [Display(Name = "How would you describe your partner's work status?", Description = "Select all that apply.")]
    public List<WorkStatusOption> PartnerWorkStatus { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(PartnerWorkStatusViewModel));

        if (PartnerWorkStatus.Count == 0)
        {
            yield return new ValidationResult(localizer["Select how you would describe your partner's work status"], [nameof(PartnerWorkStatus)]);
        }
    }
}
