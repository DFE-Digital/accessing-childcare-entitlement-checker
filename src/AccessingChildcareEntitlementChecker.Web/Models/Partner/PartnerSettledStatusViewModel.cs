using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerSettledStatusViewModel
{
    public string? ReturnTo { get; set; }

    public PartnerSettledStatusViewModel()
    {
    }

    public PartnerSettledStatusViewModel(JourneyState journeyState)
    {
        PartnerSettledStatus = journeyState.Partner.PartnerSettledStatus;
    }

    [Display(Name = "Does your partner have settled or pre-settled status under the EU Settlement Scheme?")]
    [Required(ErrorMessage = "Select if your partner has settled or pre-settled status")]
    public SettledStatusOption? PartnerSettledStatus { get; set; }
}
