namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;

public class EntitlementResponse
{
    public List<ChildResultDto> ChildResults { get; set; } = [];

    public bool HasAccessToPublicFunds { get; set; }
}
