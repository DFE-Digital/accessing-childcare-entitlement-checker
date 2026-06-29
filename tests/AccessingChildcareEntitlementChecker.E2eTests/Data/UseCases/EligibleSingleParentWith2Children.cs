

using AccessingChildcareEntitlementChecker.E2eTests.Pages;

namespace AccessingChildcareEntitlementChecker.E2eTests.Data.UseCases;

internal class EligibleSingleParentWith2Children : IUseCase
{
    public string Name => "Single parent with 2 children (Eligible)";

    public IEnumerable<(string PageName, string Answer)> GetJourney()
    {
        return
        [
            (PageNames.Location, "England"),
            
            // Child 1
            (PageNames.ChildName, "Aydin"),
            (PageNames.ChildIsBorn, "Yes"),
            (PageNames.ChildBirthDate, "Yesterday"),
            (PageNames.ChildRelationship, "Parent"),
            (PageNames.ChildSupport, "No, none of these apply"),
            
            // Action: Add another child
            (PageNames.Action, "Add another child"),

            // Child 2
            (PageNames.ChildName, "Sara"),
            (PageNames.ChildIsBorn, "No"),
            (PageNames.ChildDueDate, "Tomorrow"),
            (PageNames.ChildRelationship, "Parent"),

            // Complete child details loop
            (PageNames.Action, "Continue"),

            (PageNames.UserAge, "21 or over"),
            (PageNames.Nationality, "British or Irish citizen"),
            (PageNames.PaidWork, "Yes"),
            (PageNames.WorkStatus, "Paid employment"),
            (PageNames.WeeklyEarnings, "Yes"),
            (PageNames.YearlyEarnings, "No"),
            (PageNames.UniversalCredit, "No"),
            (PageNames.Benefits, "No, I do not get any of these benefits"),
            (PageNames.ChildcareSupport, "No, I do not get any of this childcare support"),
            (PageNames.HasPartner, "No")
        ];
    }
}
