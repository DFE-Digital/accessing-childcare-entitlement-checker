using AccessingChildcareEntitlementChecker.Web.Models.Children;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class ChildState
{
    public string ChildId { get; set; }

    public string Name { get; set; }

    public BirthStatus? BirthStatus { get; set; }

    public DateOnly? BirthDate { get; set; }

    public Relationship? BornRelationship { get; set; }

    public List<ChildSupport> ChildSupportOptions { get; set; } = [];

    public DateOnly? DueDate { get; set; }

    public Relationship? ExpectedRelationship { get; set; }

    public ChildState(string name)
    {
        ChildId = Guid.NewGuid().ToString();
        Name = name;
    }

    public void Apply(ChildNameViewModel model)
    {
        if (model.ChildName == null)
        {
            throw new InvalidOperationException("Cannot add a child with no name");
        }

        this.Name = model.ChildName;
    }

    public void Apply(ChildIsBornViewModel model)
    {
        this.BirthStatus = model.ChildIsBorn;
    }

    public void Apply(ChildBirthDateViewModel model)
    {
        this.BirthDate = model.ChildBirthDate;
    }

    public void Apply(ChildDueDateViewModel model)
    {
        this.DueDate = model.ChildDueDate;
    }

    public void Apply(ChildRelationshipViewModel model)
    {
        this.BornRelationship = model.Relationship;
    }

    public void Apply(ChildSupportViewModel model)
    {
        this.ChildSupportOptions = model.ChildSupportOptions;
    }

    public void Apply(ExpectedChildRelationshipViewModel model)
    {
        this.ExpectedRelationship = model.ExpectedChildRelationship;
    }
}
