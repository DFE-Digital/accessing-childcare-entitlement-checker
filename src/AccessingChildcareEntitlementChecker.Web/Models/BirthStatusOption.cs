using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public enum BirthStatusOption
{
    [Display(Name = "Yes")]
    Born,

    [Display(Name = "No")]
    Due,
}
