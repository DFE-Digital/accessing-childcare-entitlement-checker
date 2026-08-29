using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models;

public enum WeeklyEarningsOption
{
    [Display(Name = "Yes")]
    AboveThreshold,

    [Display(Name = "No")]
    BelowThreshold,
}
