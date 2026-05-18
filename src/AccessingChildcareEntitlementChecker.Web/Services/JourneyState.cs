using System;
using System.Collections.Generic;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class JourneyState
{
    public Dictionary<string, Child> Children { get; set; } = [];

    public CountryOfResidence? CountryOfResidence { get; set; }

    public AgeRange? UserAge { get; set; }

    public bool? HasPartner { get; set; }

    public AgeRange? PartnerAge { get; set; }

    public Child GetChild(string? childId)
    {
        if (childId == null)
        {
            throw new ArgumentException("Child ID cannot be null", nameof(childId));
        }

        return Children[childId];
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

        var id = model.ChildId ?? Guid.NewGuid().ToString();
        model.ChildId = id;
        if (!Children.TryGetValue(id, out var child))
        {
            child = new Child(id, model.ChildName);
            Children.Add(id, child);
        }

        child.Name = model.ChildName;
    }

    public void Apply(ChildIsBornViewModel model)
    {
        var child = Children[model.ChildId!];
        child.BirthStatus = model.IsChildBorn;
    }

    public void Apply(ChildBirthDateViewModel model)
    {
        var child = Children[model.ChildId!];
        child.BirthDate = model.ChildBirthDate;
    }

    public void Apply(ChildRelationshipViewModel model)
    {
        var child = Children[model.ChildId!];
        child.BornRelationship = model.ChildRelationship;
    }

    public void Apply(ChildSupportViewModel model)
    {
        var child = Children[model.ChildId!];
        child.ChildSupportOptions = model.ChildSupportOptions;
    }

    public void Apply(UserAgeViewModel model)
    {
        UserAge = model.UserAge;
    }

    public void Apply(HasPartnerViewModel model)
    {
        HasPartner = model.HasPartner;
    }

    public void Apply(PartnerAgeViewModel model)
    {
        PartnerAge = model.PartnerAge;
    }

    public void Apply(ChildDueDateViewModel model)
    {
        var child = Children[model.ChildId!];
        child.DueDate = model.ChildDueDate;
    }

    public void Apply(ExpectedChildRelationshipViewModel model)
    {
        var child = Children[model.ChildId!];
        child.ExpectedRelationship = model.ExpectedChildRelationship;
    }
}
