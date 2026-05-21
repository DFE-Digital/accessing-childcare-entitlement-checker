using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Derived;

public class HouseholdFacts
{
    public bool HasPartner { get; set; }
    public bool ReceivesUniversalCredit { get; set; }
    public bool HasAccessToPublicFunds { get; set; }
    public bool LivesInGreatBritain { get; set; }
    public CountryOfResidence? CountryOfResidence { get; set; }
}