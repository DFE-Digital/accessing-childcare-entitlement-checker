using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Derived;

public class PersonFacts
{
    public PaidWorkStatus? PaidWorkStatus { get; set; }
    public bool SelfEmployedLessThan12Months { get; set; }
    public bool EarnsAboveThreshold { get; set; }
    public bool ExceedsAdjustedNetIncomeLimit { get; set; }
    public List<WorkStatus> WorkStatuses { get; set; } = [];
    public List<PersonBenefit> Benefits { get; set; } = [];
    public List<ChildcareSupport> ChildcareSupport { get; set; } = [];
}