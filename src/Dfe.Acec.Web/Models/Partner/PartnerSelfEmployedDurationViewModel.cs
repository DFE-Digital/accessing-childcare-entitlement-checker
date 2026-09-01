using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.Acec.Web.Models.Partner;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class PartnerSelfEmployedDurationViewModel
{
    public PartnerSelfEmployedDurationViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerSelfEmployedDurationViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerSelfEmployedDuration = journeyState.PartnerSelfEmployedDuration;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Has your partner been self-employed for less than 12 months?", Description = "Think about when they first began working for themself")]
    [Required(ErrorMessage = "Select if your partner has been self-employed for less than 12 months")]
    public SelfEmployedDurationOption? PartnerSelfEmployedDuration { get; set; }
}
