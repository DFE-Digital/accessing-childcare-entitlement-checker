using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Derived;

public class PersonFacts
{
    public bool IsInPaidWork { get; set; }
    public List<PersonBenefit> Benefits { get; set; } = [];
}