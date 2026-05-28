using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class HasPartnerViewModel
{
    public HasPartnerViewModel()
    {

    }

    public HasPartnerViewModel(JourneyState journeyState)
    {
        HasPartner = journeyState.User.HasPartner;
    }

    [Display(Name = "Do you live with a partner?")]
    [Required(ErrorMessage = "Select if you live with a partner")]
    public HasPartnerOption? HasPartner { get; set; }
}
