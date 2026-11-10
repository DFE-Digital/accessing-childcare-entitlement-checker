using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models;

public enum NationalityOption
{
    [Display(Name = "British or Irish citizen")]
    BritishOrIrishCitizen,

    [Display(Name = "Citizen of an EU country, EEA country or Switzerland", Description = "<a href=\"https://www.gov.uk/eu-eea\" target=\"_blank\">View the list of EU and EEA countries (opens in new tab)</a>")]
    CitizenOfAnEuCountryEeaCountryOrSwitzerland,

    [Display(Name = "Citizen of a different country")]
    CitizenOfADifferentCountry,
}
