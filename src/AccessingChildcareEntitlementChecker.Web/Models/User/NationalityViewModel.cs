using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

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
