using AccessingChildcareEntitlementChecker.Web.Models;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class JourneyState
{
    public CountryOfResidence? CountryOfResidence { get; set; }

    public string? ChildName { get; set; }

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