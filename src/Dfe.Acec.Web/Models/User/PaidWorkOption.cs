using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models.User;

public enum PaidWorkOption
{
    [Display(Name = "Yes, I am currently in work")]
    Yes,

    [Display(Name = "Yes, but I am on parental leave", Description = "Parental leave includes maternity, paternity, shared parental, adoption, neonatal care and bereaved partner's leave")]
    ParentalLeave,

    [Display(Name = "Yes, but I am on sick leave")]
    SickLeave,

    [Display(Name = "No, I am not in work")]
    No,
}
