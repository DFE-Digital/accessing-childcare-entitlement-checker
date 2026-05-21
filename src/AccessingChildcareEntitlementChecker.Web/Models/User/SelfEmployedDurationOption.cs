using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public enum SelfEmployedDurationOption
{
    [Display(Name = "Yes")]
    LessThan12Months,

    [Display(Name = "No")]
    NotLessThan12Months,
}