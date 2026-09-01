namespace Dfe.Acec.RulesEngine.Dtos.Responses;

public class ChildResultDto
{
    public string ChildId { get; init; } = string.Empty;
    public string ChildName { get; init; } = string.Empty;
    public bool IsBorn { get; init; }
    public List<SchemeResultDto> Schemes { get; init; } = [];
}
