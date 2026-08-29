namespace Dfe.Acec.RulesEngine.Dtos.Responses;

public class EntitlementResponse
{
    public List<ChildResultDto> ChildResults { get; init; } = [];

    public bool HasAccessToPublicFunds { get; init; }
}
