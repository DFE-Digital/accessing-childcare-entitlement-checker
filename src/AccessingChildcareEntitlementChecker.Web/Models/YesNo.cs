using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public enum YesNo
{
    [Display(Name = "Option_Yes")]
    Yes,

    [Display(Name = "Option_No")]
    No,
}
