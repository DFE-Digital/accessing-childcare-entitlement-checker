using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public enum PaidWorkOption
{
    [Display(Name = "Yes")]
    Yes,

    [Display(Name = "No")]
    No,

    [Display(Name = "I am on leave from work", Description = "This includes maternity, paternity, adoption or neonatal care leave and sick leave")]
    OnLeave,
}
