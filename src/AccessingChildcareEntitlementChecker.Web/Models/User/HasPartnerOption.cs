using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public enum HasPartnerOption
{
    [Display(Name = "Yes")]
    HasPartner,

    [Display(Name = "No")]
    DoesNotHavePartner,
}
