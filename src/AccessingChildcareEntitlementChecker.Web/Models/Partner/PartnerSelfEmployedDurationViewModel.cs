using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerSelfEmployedDurationViewModel
{
    public string? ReturnTo { get; set; }

    public PartnerSelfEmployedDurationViewModel()
    {
    }

    public PartnerSelfEmployedDurationViewModel(JourneyState journeyState)
    {
        PartnerSelfEmployedDuration = journeyState.Partner.PartnerSelfEmployedDuration;
    }

    [Display(Name = "Has your partner been self-employed for less than 12 months?", Description = "If their self-employed work has stopped and started, think about when they first began doing this type of work.")]
    [Required(ErrorMessage = "Select if your partner has been self-employed for less than 12 months")]
    public SelfEmployedDurationOption? PartnerSelfEmployedDuration { get; set; }
}
