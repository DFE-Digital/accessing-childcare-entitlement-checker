namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;

public class EntitlementRequest
{
    public HouseholdDto Household { get; init; } = new();
    public PersonDto User { get; init; } = new();
    public PersonDto? Partner { get; init; }
    public List<ChildDto> Children { get; init; } = [];
}