using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class PartnerAgeViewModel
{
    public PartnerAgeViewModel()
    {

    }

    public PartnerAgeViewModel(JourneyState journeyState)
    {
        PartnerAge = journeyState.PartnerAge;
    }

    [Display(Name = "Title")]
    [Required(ErrorMessage = "Error_SelectYourPartnersAge")]
    public AgeRange? PartnerAge { get; set; }
}