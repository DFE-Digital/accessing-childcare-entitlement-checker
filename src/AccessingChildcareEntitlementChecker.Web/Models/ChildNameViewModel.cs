using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class ChildNameViewModel
{
    public ChildNameViewModel()
    {

    }

    public ChildNameViewModel(Child? child, string? backLink, string? returnTo = null)
    {
        ChildId = child?.ChildId;
        ChildName = child?.Name;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string? BackLink { get; set; }

    public string? ReturnTo { get; set; }

    public string? ChildId { get; set; }

    [Display(Name = "What name should we use for this child?", Description = "You can use any name you want. For example, 'Baby Smith'.")]
    [Required(ErrorMessage = "Enter a name for your child")]
    public string? ChildName { get; set; }
}
