using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models;

public enum NationalityOption
{
    [Display(Name = "British or Irish citizen")]
    BritishOrIrishCitizen,

    [Display(Name = "Citizen of an EU country, EEA country or Switzerland")]
    CitizenOfAnEuCountryEeaCountryOrSwitzerland,

    [Display(Name = "Citizen of a different country")]
    CitizenOfADifferentCountry,
}
