using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.User;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class JourneyState
{

    public PaidWorkOption? PaidWork { get; set; }

    public SettledStatusOption? SettledStatus { get; set; }

    public CountryOfResidence? CountryOfResidence { get; set; }

    public Dictionary<string, Child> Children { get; set; } = [];

    public AgeRange? UserAge { get; set; }

    public NationalityOption? Nationality { get; set; }

    public bool? HasPartner { get; set; }

    public AgeRange? PartnerAge { get; set; }

    public Child? GetChild(string childId)
    {
        return Children.TryGetValue(childId, out var child) ? child : null;
    }

    public void Apply(LocationViewModel model)
    {
        CountryOfResidence = model.Country;
    }

    public void Apply(ChildNameViewModel model)
    {
        if (model.ChildName == null)
        {
            throw new InvalidOperationException("Child name cannot be null");
        }

        if (model.ChildId == null)
        {
            model.ChildId = Guid.NewGuid().ToString();
        }

        var child = GetChild(model.ChildId);
        if (child == null)
        {
            child = new Child(model.ChildId, model.ChildName);
            Children.Add(model.ChildId, child);
            return;
        }

        child.Name = model.ChildName;
    }

    public void Apply(ChildIsBornViewModel model)
    {
        var child = Children[model.ChildId];
        child.BirthStatus = model.ChildIsBorn;
    }

    public void Apply(ChildBirthDateViewModel model)
    {
        var child = Children[model.ChildId];
        child.BirthDate = model.ChildBirthDate;
    }

    public void Apply(ChildDueDateViewModel model)
    {
        var child = Children[model.ChildId];
        child.DueDate = model.ChildDueDate;
    }

    public void Apply(ChildRelationshipViewModel model)
    {
        var child = Children[model.ChildId];
        child.BornRelationship = model.Relationship;
    }

    public void Apply(ChildSupportViewModel model)
    {
        var child = Children[model.ChildId];
        child.ChildSupportOptions = model.ChildSupportOptions;
    }

    public void Apply(ExpectedChildRelationshipViewModel model)
    {
        var child = Children[model.ChildId];
        child.ExpectedRelationship = model.ExpectedChildRelationship;
    }

    public void Apply(UserAgeViewModel model)
    {
        UserAge = model.UserAge;
    }

    public void Apply(NationalityViewModel model)
    {
        Nationality = model.Nationality;
    }

    public void Apply(HasPartnerViewModel model)
    {
        HasPartner = model.HasPartner;
    }

    public void Apply(PartnerAgeViewModel model)
    {
        PartnerAge = model.PartnerAge;
    }

    public void Apply(SettledStatusViewModel model)
    {
        SettledStatus = model.SettledStatus;
    }

    public void Apply(PaidWorkViewModel model)
    {
        PaidWork = model.PaidWork;
    }
}
