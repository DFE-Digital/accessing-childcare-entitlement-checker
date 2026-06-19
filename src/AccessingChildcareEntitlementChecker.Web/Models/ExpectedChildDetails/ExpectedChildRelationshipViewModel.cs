using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;

public class ExpectedChildRelationshipViewModel
{
    public ExpectedChildRelationshipViewModel()
    {
        ChildId = string.Empty;
        BackLink = string.Empty;
    }

    public ExpectedChildRelationshipViewModel(Child child, string backLink, string? returnTo = null)
    {
        ChildId = child.ChildId;
        ExpectedChildRelationship = child.ExpectedRelationship;
        ChildName = child.Name;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    public string ChildId { get; set; }

    [BindNever]
    public string ChildName { get; set; } = string.Empty;

    [Display(Name = "What will your relationship be to this child?")]
    [Required(ErrorMessage = "Select what your relationship will be to this child")]
    public Relationship? ExpectedChildRelationship { get; set; }
}
