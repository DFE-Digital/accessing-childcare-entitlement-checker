using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class PaidWorkViewModel
{
    public string? ReturnTo { get; set; }

    public PaidWorkViewModel()
    {
    }

    public PaidWorkViewModel(JourneyState journeyState)
    {
        PaidWork = journeyState.PaidWork;
    }

    [Display(Name = "Are you in paid work?", Description = "Paid work includes being self-employed or freelance")]
    [Required(ErrorMessage = "Select if you are in paid work")]
    public PaidWorkOption? PaidWork { get; set; }
}
