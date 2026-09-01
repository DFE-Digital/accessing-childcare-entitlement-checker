using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Dfe.Acec.Web.Models.Summary;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class RemoveChildViewModel
{
    public string ChildId { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string ReturnTo { get; set; } = Models.ReturnTo.CheckChildDetails;

    [Display(Name = "Are you sure you want to remove {0}?")]
    [Required(ErrorMessage = "Select yes if you want to remove this child")]
    public bool? RemoveConfirmed { get; set; }
}
