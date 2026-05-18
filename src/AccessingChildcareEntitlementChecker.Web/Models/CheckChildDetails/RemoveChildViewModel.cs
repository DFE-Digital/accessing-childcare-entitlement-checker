using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;

public class RemoveChildViewModel
{
    public string ChildId { get; set; } = default!;

    public string Name { get; set; } = string.Empty;

    [Display(Name = "Are you sure you want to remove {0}?")]
    [Required(ErrorMessage = "Select yes if you want to remove this child")]
    public bool? RemoveConfirmed { get; set; }
}
