using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public enum YearlyEarningsOption
{
    [Display(Name = "Yes")]
    AboveThreshold,

    [Display(Name = "No")]
    BelowThreshold,
}