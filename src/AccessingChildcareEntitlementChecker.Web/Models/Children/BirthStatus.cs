using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Children;

public enum BirthStatus
{
    [Display(Name = "Yes")]
    Born,

    [Display(Name = "No")]
    Due,
}
