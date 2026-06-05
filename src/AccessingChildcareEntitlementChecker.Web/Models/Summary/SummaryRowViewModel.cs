namespace AccessingChildcareEntitlementChecker.Web.Models.Summary;

public record SummaryRowViewModel(
    bool IsLocalised,
    string Key,
    string Value,
    string ChangeController,
    string ChangeAction);
