using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerAgeViewModel
{
    public PartnerAgeViewModel()
    {

    }

    public PartnerAgeViewModel(JourneyState journeyState)
    {
        PartnerAge = journeyState.Partner.PartnerAge;
    }

    [Display(Name = "What is your partner's age?")]
    [Required(ErrorMessage = "Select your partner's age")]
    public AgeRange? PartnerAge { get; set; }
}
