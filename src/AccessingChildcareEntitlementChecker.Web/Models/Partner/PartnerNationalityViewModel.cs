using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerNationalityViewModel
{
    public string? ReturnTo { get; set; }

    public PartnerNationalityViewModel()
    {
    }

    public PartnerNationalityViewModel(JourneyState journeyState)
    {
        PartnerNationality = journeyState.PartnerNationality;
    }

    [Display(Name = "Which of these best describes your partners nationality?")]
    [Required(ErrorMessage = "Select your partner's nationality")]
    public NationalityOption? PartnerNationality { get; set; }
}
