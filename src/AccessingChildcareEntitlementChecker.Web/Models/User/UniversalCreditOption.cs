using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public enum UniversalCreditOption
{
    [Display(Name = "Yes")]
    Receives,

    [Display(Name = "No")]
    DoesNotReceive,
}
