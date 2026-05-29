using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class UniversalCreditViewModel
{
    public string? ReturnTo { get; set; }

    public UniversalCreditViewModel()
    {
    }

    public UniversalCreditViewModel(JourneyState journeyState)
    {
        UniversalCredit = journeyState.UniversalCredit;
    }

    [Display(Name = "Does your household receive universal credit?")]
    [Required(ErrorMessage = "Select if your household receives universal credit")]
    public UniversalCreditOption? UniversalCredit { get; set; }
}
