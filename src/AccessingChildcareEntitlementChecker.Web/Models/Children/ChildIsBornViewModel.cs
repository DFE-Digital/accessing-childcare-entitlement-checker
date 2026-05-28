using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Children;

public class ChildIsBornViewModel
{
    public string? ReturnTo { get; set; }

    public ChildIsBornViewModel()
    {
        ChildId = Guid.Empty;
    }

    public ChildIsBornViewModel(ChildState child)
    {
        ChildId = child.ChildId;
        ChildIsBorn = child.BirthStatus;
    }

    public Guid ChildId { get; set; }

    [Display(Name = "Has this child been born yet?")]
    [Required(ErrorMessage = "Select if this child has been born")]
    public BirthStatus? ChildIsBorn { get; set; }
}
