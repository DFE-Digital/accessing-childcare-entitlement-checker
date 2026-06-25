namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;

public class ChildResultDto
{
    public string ChildName { get; set; } = string.Empty;

    public int? AgeInYears { get; set; }

    public bool IsBorn { get; set; }

    public List<SchemeResultDto> Schemes { get; set; } = [];
}