using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class WhereDoYouLiveViewModel
{
    public WhereDoYouLiveViewModel()
    {

    }

    public WhereDoYouLiveViewModel(JourneyState journeyState)
    {
        Country = journeyState.CountryOfResidence;
    }

    [Required(ErrorMessage = "Error_SelectWhereYouLive")]
    public CountryOfResidence? Country { get; set; }
}