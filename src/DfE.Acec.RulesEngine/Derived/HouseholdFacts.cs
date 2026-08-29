using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Derived;

public class HouseholdFacts
{
    public bool HasPartner { get; init; }
    public bool ReceivesUniversalCredit { get; init; }
    public bool HasAccessToPublicFunds { get; init; }
    public bool LivesInGreatBritain { get; init; }
    public CountryOfResidence? CountryOfResidence { get; init; }
}