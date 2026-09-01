using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models;

public enum WorkStatusOption
{
    [Display(Name = "Paid employment")]
    PaidEmployment,

    [Display(Name = "Self-employed")]
    SelfEmployed,

    [Display(Name = "Apprentice")]
    Apprentice,
}
