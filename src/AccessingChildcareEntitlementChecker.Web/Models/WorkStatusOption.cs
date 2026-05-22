using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public enum WorkStatusOption
{
    [Display(Name = "Paid employment")]
    PaidEmployment,

    [Display(Name = "Self-employed")]
    SelfEmployed,

    [Display(Name = "Apprentice")]
    Apprentice,
}