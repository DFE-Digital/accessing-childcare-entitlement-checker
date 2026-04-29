using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public enum AgeRange
{
    [Display(Name = "Option_Under18")]
    UnderEighteen,

    [Display(Name = "Option_18To20")]
    EighteenToTwenty,

    [Display(Name = "Option_21OrOver")]
    TwentyOneOrOver,
}