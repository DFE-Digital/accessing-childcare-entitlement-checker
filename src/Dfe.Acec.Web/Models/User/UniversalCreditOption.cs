using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models.User;

public enum UniversalCreditOption
{
    [Display(Name = "Yes")]
    Receives,

    [Display(Name = "No")]
    DoesNotReceive,
}
