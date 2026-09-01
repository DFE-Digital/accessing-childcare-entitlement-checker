using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Models.Partner;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class PartnerPaidWorkViewModel
{
    public PartnerPaidWorkViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerPaidWorkViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerPaidWork = journeyState.PartnerPaidWork;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Is your partner in paid work?", Description = "Paid work includes being self-employed or freelance")]
    [Required(ErrorMessage = "Select if your partner is in paid work")]
    public PartnerPaidWorkOption? PartnerPaidWork { get; set; }
}
