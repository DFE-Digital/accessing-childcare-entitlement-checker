using System.Collections.Generic;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class Child
{
    public string ChildId { get; set; } = default!;

    public string Name { get; set; } = string.Empty;

    public AccessingChildcareEntitlementChecker.Web.Models.BirthStatusOption? BirthStatus { get; set; }

    public DateOnly? BirthDate { get; set; }

    public AccessingChildcareEntitlementChecker.Web.Models.RelationshipOption? BornRelationship { get; set; }

    public List<AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails.ChildSupportOption> ChildSupportOptions { get; set; } = [];

    public DateOnly? DueDate { get; set; }

    public AccessingChildcareEntitlementChecker.Web.Models.RelationshipOption? ExpectedRelationship { get; set; }

    public Child()
    {
    }

    public Child(string childId, string name)
    {
        ChildId = childId;
        Name = name;
    }

}
