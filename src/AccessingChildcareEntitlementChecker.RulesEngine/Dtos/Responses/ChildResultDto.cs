namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;

public class ChildResultDto
{
    public string ChildId { get; set; } = string.Empty;
    public string ChildName { get; set; } = string.Empty;
    public bool IsBorn { get; set; }
    public List<SchemeResultDto> Schemes { get; set; } = [];
}