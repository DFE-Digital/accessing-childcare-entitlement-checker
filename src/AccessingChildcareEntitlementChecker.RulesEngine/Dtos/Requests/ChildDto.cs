using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;

public class ChildDto
{
    public string ChildId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public BirthStatus? BirthStatus { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public DateOnly? DueDate { get; init; }
    public List<ChildRelatedBenefit> ChildRelatedBenefits { get; init; } = [];
    public bool UserIsOnParentalLeaveForChild { get; init; }
    public bool PartnerIsOnParentalLeaveForChild { get; init; }
}