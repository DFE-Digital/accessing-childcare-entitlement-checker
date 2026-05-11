namespace AccessingChildcareEntitlementChecker.RulesEngine.Derived;

public class DerivedContext
{
    public HouseholdFacts Household { get; set; } = new();
    public PersonFacts User { get; set; } = new();
    public PersonFacts? Partner { get; set; }
    public List<ChildFacts> Children { get; set; } = [];
}