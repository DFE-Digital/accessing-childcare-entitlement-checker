using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Derived;

public class ChildFacts
{
    public string ChildId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public bool IsBorn { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public DateOnly? DueDate { get; init; }
    public int? AgeInYears { get; init; }
    public int? AgeInMonths { get; init; }
    public List<ChildRelatedBenefit> ChildRelatedBenefits { get; init; } = [];
    public bool UserIsOnParentalLeaveForChild { get; set; }
    public bool PartnerIsOnParentalLeaveForChild { get; set; }
}