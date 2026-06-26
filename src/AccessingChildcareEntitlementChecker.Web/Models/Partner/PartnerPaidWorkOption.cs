using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public enum PartnerPaidWorkOption
{
    [Display(Name = "Yes")]
    Yes,

    [Display(Name = "Yes, but they are on parental leave", Description = "Parental leave includes maternity, paternity, shared parental, adoption, neonatal care and bereaved partners leave")]
    ParentalLeave,

    [Display(Name = "Yes, but they are on sick leave")]
    SickLeave,

    [Display(Name = "No, they are not in work")]
    No,
}
