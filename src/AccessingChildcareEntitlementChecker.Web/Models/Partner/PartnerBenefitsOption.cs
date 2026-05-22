using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public enum PartnerBenefitsOption
{
    [Display(Name = "Carer's Allowance")]
    CarersAllowance,

    [Display(Name = "Contribution-based Employment and Support Allowance")]
    ContributionBasedEmploymentAndSupportAllowance,

    [Display(Name = "Employment and support allowance (ESA)")]
    EmploymentAndSupportAllowance,

    [Display(Name = "Guaranteed element of Pension Credit")]
    GuaranteedElementOfPensionCredit,

    [Display(Name = "Incapacity Benefit")]
    IncapacityBenefit,

    [Display(Name = "Limited capability for work (LCW)")]
    LimitedCapabilityForWork,

    [Display(Name = "Limited capability for work related activity (LCWRA)")]
    LimitedCapabilityForWorkRelatedActivity,

    [Display(Name = "Severe Disablement Allowance")]
    SevereDisablementAllowance,

    [Display(Name = "No, they do not get any of these benefits")]
    None,
}