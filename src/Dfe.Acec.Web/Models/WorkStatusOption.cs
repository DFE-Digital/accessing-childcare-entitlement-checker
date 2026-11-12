using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models;

public enum WorkStatusOption
{
    [Display(Name = "Employee")]
    PaidEmployment,

    [Display(Name = "Self-employed")]
    SelfEmployed,

    [Display(Name = "Apprentice")]
    Apprentice,
}
