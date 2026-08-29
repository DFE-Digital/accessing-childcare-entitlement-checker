using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models;

public enum SelfEmployedDurationOption
{
    [Display(Name = "Yes")]
    LessThan12Months,

    [Display(Name = "No")]
    NotLessThan12Months,
}
