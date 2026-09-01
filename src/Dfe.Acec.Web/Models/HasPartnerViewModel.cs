using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Models;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class HasPartnerViewModel
{
    public HasPartnerViewModel()
    {
        BackLink = string.Empty;
    }

    public HasPartnerViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        HasPartner = journeyState.HasPartner;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Required(ErrorMessage = "Select if you live with a partner")]
    public bool? HasPartner { get; set; }
}
