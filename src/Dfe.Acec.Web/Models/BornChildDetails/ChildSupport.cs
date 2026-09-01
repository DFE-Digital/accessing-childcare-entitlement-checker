using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models.BornChildDetails;

public enum ChildSupport
{
    [Display(Name = "Armed Forces Independence Payment")]
    ArmedForcesIndependencePayment,

    [Display(Name = "Certificate of visual impairment")]
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
