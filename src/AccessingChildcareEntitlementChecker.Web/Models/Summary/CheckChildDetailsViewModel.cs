using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Models.Summary;

public record CheckChildDetailsViewModel(
    IReadOnlyList<ChildSummaryViewModel> Children,
    bool HasChildren,
    Child? LastEditedChild,
    string BackLink);
