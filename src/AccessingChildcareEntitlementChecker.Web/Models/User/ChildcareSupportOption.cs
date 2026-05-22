using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public enum ChildcareSupportOption
{
    [Display(Name = "Childcare vouchers")]
    ChildcareVouchers,

    [Display(Name = "A childcare bursary or grant (as part of education funding)")]
    ChildcareBursaryOrGrant,

    [Display(Name = "No, I do not get any of this childcare support")]
    None,
}
