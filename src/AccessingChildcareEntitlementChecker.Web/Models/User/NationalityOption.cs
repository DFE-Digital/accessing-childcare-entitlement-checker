using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public enum NationalityOption
{
    [Display(Name = "British or Irish citizen")]
    BritishOrIrishCitizen,

    [Display(Name = "Citizen of an EU country, EEA country or Switzerland")]
    CitizenOfAnEUCountryEEACountryOrSwitzerland,

    [Display(Name = "Citizen of a different country")]
    CitizenOfADifferentCountry,
}
