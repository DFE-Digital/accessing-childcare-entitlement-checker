using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class HasPartnerViewModel
{
    public HasPartnerViewModel()
    {

    }

    public HasPartnerViewModel(JourneyState journeyState)
    {
        HasPartner = journeyState.HasPartner;
    }


    [Required(ErrorMessage = "Error_SelectIfYouHavePartner")]
    public bool? HasPartner { get; set; }
}
