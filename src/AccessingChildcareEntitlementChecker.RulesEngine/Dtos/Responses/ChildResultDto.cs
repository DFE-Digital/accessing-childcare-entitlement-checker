namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;

public class ChildResultDto
{
    public string ChildName { get; set; } = string.Empty;

    public List<SchemeResultDto> Schemes { get; set; } = [];
}