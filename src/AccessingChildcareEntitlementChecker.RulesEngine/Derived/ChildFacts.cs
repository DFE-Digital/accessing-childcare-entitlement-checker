namespace AccessingChildcareEntitlementChecker.RulesEngine.Derived;

public class ChildFacts
{
    public string Name { get; set; } = string.Empty;
    public bool IsBorn { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? DueDate { get; set; }
    public int? AgeInYears { get; set; }
}