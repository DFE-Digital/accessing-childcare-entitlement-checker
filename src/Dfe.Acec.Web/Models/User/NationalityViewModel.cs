using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Models.User;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class NationalityViewModel
{
    public NationalityViewModel()
    {
        BackLink = string.Empty;
    }

    public NationalityViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        Nationality = journeyState.Nationality;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "What is your nationality?")]
    [Required(ErrorMessage = "Select your nationality")]
    public NationalityOption? Nationality { get; set; }
}
