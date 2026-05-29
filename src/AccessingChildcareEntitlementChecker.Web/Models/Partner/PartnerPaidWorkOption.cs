using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public enum PartnerPaidWorkOption
{
    [Display(Name = "Yes")]
    Yes,

    [Display(Name = "No")]
    No,

    [Display(Name = "They are on leave from work", Description = "This includes maternity, paternity, adoption or neonatal care leave and sick leave")]
    OnLeave,
}
