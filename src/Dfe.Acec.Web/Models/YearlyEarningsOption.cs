using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models;

public enum YearlyEarningsOption
{
    [Display(Name = "Yes")]
    AboveThreshold,

    [Display(Name = "No")]
    BelowThreshold,
}
