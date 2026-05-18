using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public enum AgeRange
{
    [Display(Name = "Under 18")]
    UnderEighteen,

    [Display(Name = "18 to 20")]
    EighteenToTwenty,

    [Display(Name = "21 or over")]
    TwentyOneOrOver,
}
