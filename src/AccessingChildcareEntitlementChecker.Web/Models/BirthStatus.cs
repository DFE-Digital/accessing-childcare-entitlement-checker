using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models
{
    public enum BirthStatus
    {
        [Display(Name = "Option_Born")]
        Born,

        [Display(Name = "Option_Due")]
        Due,
    }
}
