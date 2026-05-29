using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public enum PartnerChildcareSupportOption
{
    [Display(Name = "Childcare vouchers")]
    ChildcareVouchers,

    [Display(Name = "A childcare bursary or grant (as part of education funding)")]
    ChildcareBursaryOrGrant,

    [Display(Name = "No, they do not get any of this childcare support")]
    None,
}
