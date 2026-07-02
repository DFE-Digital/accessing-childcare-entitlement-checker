using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public enum PartnerChildcareSupportOption
{
    [Display(Name = "Childcare vouchers", Description = "A scheme that lets you pay for childcare from your salary before tax, which closed to new applicants in October 2018")]
    ChildcareVouchers,

    [Display(Name = "A childcare bursary or grant", Description = "Money to help pay for childcare while you study, for example through a college or university")]
    ChildcareBursaryOrGrant,

    [Display(Name = "No, they do not get any of these")]
    None,
}
