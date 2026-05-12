using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models
{
    public class ChildIsBornViewModel
    {
        public ChildIsBornViewModel()
        {

        }

        public ChildIsBornViewModel(JourneyState journeyState)
        {
            ChildIsBorn = journeyState.ChildIsBorn;
        }

        [Display(Name = "Has this child been born yet?")]
        [Required(ErrorMessage = "Select if this child has been born")]
        public BirthStatus? ChildIsBorn { get; set; }
    }
}
