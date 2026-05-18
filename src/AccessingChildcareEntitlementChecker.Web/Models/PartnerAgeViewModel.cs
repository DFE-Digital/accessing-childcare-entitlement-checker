using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class PartnerAgeViewModel
{
    public string? ReturnTo { get; set; }

    public PartnerAgeViewModel()
    {
    }

    public PartnerAgeViewModel(JourneyState journeyState)
    {
        PartnerAge = journeyState.PartnerAge;
    }

    [Required(ErrorMessage = "Error_SelectYourPartnersAge")]
    public AgeRange? PartnerAge { get; set; }
}
