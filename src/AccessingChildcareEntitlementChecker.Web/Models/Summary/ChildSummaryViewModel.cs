namespace AccessingChildcareEntitlementChecker.Web.Models.Summary;

public record ChildSummaryViewModel(
    string ChildId,
    string Name,
    IReadOnlyList<SummaryRowViewModel> Rows);
