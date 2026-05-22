using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public enum WeeklyEarningsOption
{
    [Display(Name = "Yes")]
    AboveThreshold,

    [Display(Name = "No")]
    BelowThreshold,
}