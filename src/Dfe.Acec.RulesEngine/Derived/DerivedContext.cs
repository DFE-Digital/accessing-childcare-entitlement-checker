namespace Dfe.Acec.RulesEngine.Derived;

public class DerivedContext
{
    public HouseholdFacts Household { get; init; } = new();
    public PersonFacts User { get; init; } = new();
    public PersonFacts? Partner { get; init; }
    public List<ChildFacts> Children { get; init; } = [];
}