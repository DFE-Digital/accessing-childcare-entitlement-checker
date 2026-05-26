using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;

public class ChildDto
{
    public string Name { get; set; } = string.Empty;
    public BirthStatus BirthStatus { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? DueDate { get; set; }
    public RelationshipToChild? RelationshipToChild { get; set; }
    public List<ChildRelatedBenefit> ChildRelatedBenefits { get; set; } = [];
}