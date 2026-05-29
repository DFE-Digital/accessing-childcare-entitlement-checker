using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public enum SettledStatusOption
{
    [Display(Name = "Yes")]
    Yes,

    [Display(Name = "No")]
    No,

    [Display(Name = "I applied before 1 July 2021 and am still waiting for a decision")]
    StillWaiting,
}
