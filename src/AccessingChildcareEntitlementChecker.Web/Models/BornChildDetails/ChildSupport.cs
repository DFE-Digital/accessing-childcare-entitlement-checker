using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails
{
    public enum ChildSupport
    {
        [Display(Name = "Armed Forces Independence Payment")]
        ArmedForcesIndependencePayment,

        [Display(Name = "Certificate of Visual Impairment")]
        CertificateOfVisualImpairment,

        [Display(Name = "Disability Living Allowance (DLA)")]
        DisabilityLivingAllowance,

        [Display(Name = "Education, health and care (EHC) plan")]
        EducationHealthAndCarePlan,

        [Display(Name = "Personal Independence Payment (PIP)")]
        PersonalIndependencePayment,

        [Display(Name = "No, none of these apply")]
        NoneOfTheseApply,
    }
}
