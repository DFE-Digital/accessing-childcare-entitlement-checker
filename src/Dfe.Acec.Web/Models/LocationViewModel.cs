using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.Acec.Web.Models;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class LocationViewModel
{
    public LocationViewModel()
    {
        BackLink = string.Empty;
    }

    public LocationViewModel(JourneyState journeyState, string backlink, string? returnTo = null)
    {
        Country = journeyState.CountryOfResidence;
        BackLink = backlink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Required(ErrorMessage = "Error_SelectLocation")]
    public CountryOfResidence? Country { get; set; }
}
