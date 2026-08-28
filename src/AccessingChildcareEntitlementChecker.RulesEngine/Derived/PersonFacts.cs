using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Derived;

public class PersonFacts
{
    public PaidWorkStatus? PaidWorkStatus { get; init; }
    public bool SelfEmployedLessThan12Months { get; init; }
    public bool EarnsAboveThreshold { get; init; }
    public bool ExceedsAdjustedNetIncomeLimit { get; init; }
    public List<PersonBenefit> Benefits { get; init; } = [];
    public List<ChildcareSupport> ChildcareSupport { get; init; } = [];
}