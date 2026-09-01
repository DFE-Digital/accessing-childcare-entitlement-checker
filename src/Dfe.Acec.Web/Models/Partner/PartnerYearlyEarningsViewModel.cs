using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.Acec.Web.Models.Partner;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class PartnerYearlyEarningsViewModel
{
    public PartnerYearlyEarningsViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerYearlyEarningsViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerYearlyEarnings = journeyState.PartnerYearlyEarnings;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "Does your partner expect their adjusted net income to be more than £100,000 for the current tax year?", Description = "Adjusted net income is your total income before tax, minus certain tax reliefs.")]
    [Required(ErrorMessage = "Select if your partner expects their adjusted net income to be more than £100,000 for the current tax year")]
    public YearlyEarningsOption? PartnerYearlyEarnings { get; set; }
}
