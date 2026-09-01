using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models.Partner;

public enum PartnerPaidWorkOption
{
    [Display(Name = "Yes, they are currently in work")]
    Yes,

    [Display(Name = "Yes, but they are on parental leave", Description = "Parental leave includes maternity, paternity, shared parental, adoption, neonatal care and bereaved partner's leave")]
    ParentalLeave,

    [Display(Name = "Yes, but they are on sick leave")]
    SickLeave,

    [Display(Name = "No, they are not in work")]
    No,
}
