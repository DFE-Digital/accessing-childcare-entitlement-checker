using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Models;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class UserAgeViewModel
{
    public UserAgeViewModel()
    {
        BackLink = string.Empty;
    }

    public UserAgeViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        UserAge = journeyState.UserAge;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Required(ErrorMessage = "Error_SelectYourAge")]
    public AgeRange? UserAge { get; set; }
}
