using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.Acec.Web.Models;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class ChildIsBornViewModel
{
    public ChildIsBornViewModel()
    {
        ChildId = string.Empty;
        BackLink = string.Empty;
    }

    public ChildIsBornViewModel(Child child, string backLink, string? returnTo = null)
    {
        ChildId = child.ChildId;
        ChildIsBorn = child.BirthStatus;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    public string ChildId { get; set; }

    [Display(Name = "Has this child been born yet?")]
    [Required(ErrorMessage = "Select if this child has been born")]
    public BirthStatus? ChildIsBorn { get; set; }
}
