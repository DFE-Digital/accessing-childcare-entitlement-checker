using System.ComponentModel.DataAnnotations;
using Dfe.Acec.Web.Attributes;

namespace Dfe.Acec.Web.Models;

public enum NationalityOption
{
    [Display(Name = "British or Irish citizen")]
    BritishOrIrishCitizen,

    [Display(Name = "Citizen of an EU country, EEA country or Switzerland", Description = "View the list of EU and EEA countries (opens in new tab)")]
    [ExternalLink(To = "https://www.gov.uk/eu-eea", OpenInNewTab = true)]
    CitizenOfAnEuCountryEeaCountryOrSwitzerland,

    [Display(Name = "Citizen of a different country")]
    CitizenOfADifferentCountry,
}
