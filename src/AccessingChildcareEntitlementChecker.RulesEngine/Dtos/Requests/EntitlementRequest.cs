namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;

public class EntitlementRequest
{
    public HouseholdDto Household { get; set; } = new();

    public PersonDto User { get; set; } = new();

    public PersonDto? Partner { get; set; }

    public List<ChildDto> Children { get; set; } = [];
}