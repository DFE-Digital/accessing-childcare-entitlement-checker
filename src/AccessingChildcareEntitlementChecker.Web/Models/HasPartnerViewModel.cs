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


    [Display(Name = "Title", Description = "Caption")]
    [Required(ErrorMessage = "Error_SelectIfYouHavePartner")]
    public YesNo? HasPartner { get; set; }
}