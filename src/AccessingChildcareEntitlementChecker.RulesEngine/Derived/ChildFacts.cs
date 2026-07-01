using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Derived;

public class ChildFacts
{
    public string ChildId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsBorn { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? DueDate { get; set; }
    public int? AgeInYears { get; set; }
    public int? AgeInMonths { get; set; }
    public List<ChildRelatedBenefit> ChildRelatedBenefits { get; set; } = [];
    public RelationshipToChild? RelationshipToChild { get; set; }
    public bool UserIsOnParentalLeaveForChild { get; set; }
    public bool PartnerIsOnParentalLeaveForChild { get; set; }
}