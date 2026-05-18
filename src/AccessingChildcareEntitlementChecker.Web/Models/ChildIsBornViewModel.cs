using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models
{
    public class ChildIsBornViewModel
    {
        public string? ReturnTo { get; set; }

        public ChildIsBornViewModel()
        {
            ChildId = string.Empty;
        }

        public ChildIsBornViewModel(string childId, JourneyState journeyState)
        {
            var child = journeyState.GetChild(childId);
            ChildId = childId;
            ChildIsBorn = child.BirthStatus;
        }

        public string ChildId { get; set; }

        [Display(Name = "Has this child been born yet?")]
        [Required(ErrorMessage = "Select if this child has been born")]
        public BirthStatus? ChildIsBorn { get; set; }
    }
}
