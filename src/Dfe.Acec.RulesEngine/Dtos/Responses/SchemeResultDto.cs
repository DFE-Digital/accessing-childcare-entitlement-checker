using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Dtos.Responses;

public class SchemeResultDto
{
    public SchemeCode SchemeCode { get; init; }
    public bool EligibleNow { get; init; }
    public bool EligibleInFuture { get; init; }
    public DateOnly? ApplyFromDate { get; init; }
    public DateOnly? UseFromDate { get; init; }
    public ParentalLeaveParty? ApplyAndStartAffectedByParentalLeave { get; init; }
    public ParentalLeaveParty? EligibilityEndsWithParentalLeaveFor { get; init; }
}