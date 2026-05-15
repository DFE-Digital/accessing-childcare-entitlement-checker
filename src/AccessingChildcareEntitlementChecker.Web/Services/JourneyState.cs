using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class JourneyState
{
    public CountryOfResidence? CountryOfResidence { get; set; }

    public string? ChildName { get; set; }

    public BirthStatus? ChildIsBorn { get; set; }

    public DateOnly? ChildBirthDate { get; set; }

    public Relationship? Relationship { get; set; }

    public List<ChildSupport> ChildSupportOptions { get; set; } = [];

    public DateOnly? ChildDueDate { get; set; }

    public Relationship? ExpectedChildRelationship { get; set; }

    public bool? HasPartner { get; set; }

    public AgeRange? UserAge { get; set; }

    public AgeRange? PartnerAge { get; set; }

    public void Apply(LocationViewModel model)
    {
        CountryOfResidence = model.Country;
    }

    public void Apply(ChildNameViewModel model)
    {
        ChildName = model.ChildName;
    }

    public void Apply(ChildIsBornViewModel model)
    {
        ChildIsBorn = model.ChildIsBorn;
    }

    public void Apply(ChildBirthDateViewModel model)
    {
        ChildBirthDate = model.ChildBirthDate;
    }

    public void Apply(ChildRelationshipViewModel model)
    {
        Relationship = model.Relationship;
    }

    public void Apply(ChildSupportViewModel model)
    {
        ChildSupportOptions = model.ChildSupportOptions;
    }

    public void Apply(ChildDueDateViewModel model)
    {
        ChildDueDate = model.ChildDueDate;
    }

    public void Apply(ExpectedChildRelationshipViewModel model)
    {
        ExpectedChildRelationship = model.ExpectedChildRelationship;
    }

    public void Apply(HasPartnerViewModel model)
    {
        HasPartner = model.HasPartner;
    }

    public void Apply(UserAgeViewModel model)
    {
        UserAge = model.UserAge;
    }

    public void Apply(PartnerAgeViewModel model)
    {
        PartnerAge = model.PartnerAge;
    }
}
