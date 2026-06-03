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


    [Required(ErrorMessage = "Select if you live with a partner")]
    public bool? HasPartner { get; set; }
}
