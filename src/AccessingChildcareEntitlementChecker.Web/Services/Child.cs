using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class Child
{
    public string ChildId { get; set; } = default!;

    public string Name { get; set; } = string.Empty;

    public BirthStatus? BirthStatus { get; set; }

    public DateOnly? BirthDate { get; set; }

    public Relationship? BornRelationship { get; set; }

    public List<ChildSupport> ChildSupportOptions { get; set; } = [];

    public DateOnly? DueDate { get; set; }

    public Relationship? ExpectedRelationship { get; set; }

    public Child()
    {
    }

    public Child(string childId, string name)
    {
        ChildId = childId;
        Name = name;
    }

}
