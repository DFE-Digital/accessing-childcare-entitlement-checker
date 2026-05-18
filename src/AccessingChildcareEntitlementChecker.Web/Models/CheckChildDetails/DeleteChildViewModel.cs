using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;

public class DeleteChildViewModel
{
    public string ChildId { get; set; } = default!;

    public string Name { get; set; } = string.Empty;

    [Display(Name = "Are you sure you want to delete {0}?")]
    [Required(ErrorMessage = "Select yes if you want to delete this child")]
    public bool? DeleteConfirmed { get; set; }
}
