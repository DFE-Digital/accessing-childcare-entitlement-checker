using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public enum PaidWorkOption
{
    [Display(Name = "Yes")]
    Yes,

    [Display(Name = "Yes, but I am on parental leave", Description = "Parental leave includes maternity, paternity, shared parental, adoption, neonatal care and bereaved partners leave")]
    ParentalLeave,

    [Display(Name = "Yes, but I am on sick leave")]
    SickLeave,

    [Display(Name = "No, I am not in work")]
    No,
}
