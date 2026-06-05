namespace AccessingChildcareEntitlementChecker.Web.Models.Summary;

public record SummaryRowViewModel(
    bool isLocalised,
    string Key,
    string Value,
    string ChangeController,
    string ChangeAction);
