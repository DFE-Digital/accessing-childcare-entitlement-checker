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
    [Required(ErrorMessage = "Select \"Yes\" if your partner normally lives with you, but they are away because of work.")]
    public HasPartnerOption? HasPartner { get; set; }
}
