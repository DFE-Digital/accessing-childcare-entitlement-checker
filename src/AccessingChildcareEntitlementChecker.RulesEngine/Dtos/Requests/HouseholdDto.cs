using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;

public class HouseholdDto
{
    public CountryOfResidence? CountryOfResidence { get; init; }
    public bool HasPartner { get; init; }
    public bool ReceivesUniversalCredit { get; init; }

}