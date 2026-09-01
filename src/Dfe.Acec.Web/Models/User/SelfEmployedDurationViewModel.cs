using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Models.User;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class SelfEmployedDurationViewModel
{
    public SelfEmployedDurationViewModel()
    {
        BackLink = string.Empty;
    }

    public SelfEmployedDurationViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        SelfEmployedDuration = journeyState.SelfEmployedDuration;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Have you been self-employed for less than 12 months?", Description = "Think about when you first began working for yourself")]
    [Required(ErrorMessage = "Select if you have been self-employed for less than 12 months")]
    public SelfEmployedDurationOption? SelfEmployedDuration { get; set; }
}
