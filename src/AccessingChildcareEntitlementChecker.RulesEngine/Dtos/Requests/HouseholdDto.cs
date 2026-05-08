using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;

public class HouseholdDto
{
    public CountryOfResidence? CountryOfResidence { get; set; }

    public bool HasPartner { get; set; }
}