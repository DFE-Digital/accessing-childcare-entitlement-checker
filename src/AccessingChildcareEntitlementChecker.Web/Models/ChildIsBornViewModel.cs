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

        [Display(Name = "Label_ChildIsBorn")]
        [Required(ErrorMessage = "Error_ChildIsBorn")]
        public BirthStatus? ChildIsBorn { get; set; }
    }
}
