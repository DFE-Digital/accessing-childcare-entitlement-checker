namespace AccessingChildcareEntitlementChecker.Web.Models.Summary;

public record ChildSummaryViewModel(
    string ChildId,
    string Name,
    string ReturnTo,
    IReadOnlyList<SummaryRowViewModel> Rows);
