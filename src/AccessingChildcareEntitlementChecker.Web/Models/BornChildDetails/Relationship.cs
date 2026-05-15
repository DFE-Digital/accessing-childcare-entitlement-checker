using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails
{
    public enum Relationship
    {
        [Display(Name = "Parent")]
        Parent,

        [Display(Name = "Guardian or short-term respite carer")]
        GuardianOrCarer,

        [Display(Name = "Foster parent")]
        FosterParent,
    }
}
