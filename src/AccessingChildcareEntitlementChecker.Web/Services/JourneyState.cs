using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class JourneyState
{
    public CountryOfResidence? CountryOfResidence { get; set; }

    public string? ChildName { get; set; }

    public BirthStatus? ChildIsBorn { get; set; }

    public DateTime? ChildBirthDate { get; set; }

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