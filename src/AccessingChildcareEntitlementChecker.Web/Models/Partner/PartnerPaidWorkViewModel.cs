using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerPaidWorkViewModel
{
    public string? ReturnTo { get; set; }

    public PartnerPaidWorkViewModel()
    {
    }

    public PartnerPaidWorkViewModel(JourneyState journeyState)
    {
        PartnerPaidWork = journeyState.PartnerPaidWork;
    }

    [Display(Name = "Is your partner in paid work?", Description = "Paid work includes being self-employed or freelance")]
    [Required(ErrorMessage = "Select if your partner is in paid work")]
    public PartnerPaidWorkOption? PartnerPaidWork { get; set; }
}
