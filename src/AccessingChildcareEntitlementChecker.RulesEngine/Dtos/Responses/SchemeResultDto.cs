using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;

public class SchemeResultDto
{
    public SchemeCode SchemeCode { get; set; }

    public bool EligibleNow { get; set; }

    public bool EligibleInFuture { get; set; }
    
    public DateOnly? EligibleFrom { get; set; }
    
    public int? EligibleWhenChildTurns { get; set; }
}