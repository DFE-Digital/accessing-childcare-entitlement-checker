using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Models.Summary;

public record CheckAnswersViewModel(
    IReadOnlyList<ChildSummaryViewModel> Children,
    bool HasChildren,
    Child? LastEditedChild,
    IReadOnlyList<SummaryRowViewModel> UserDetails,
    IReadOnlyList<SummaryRowViewModel> PartnerDetails,
    string BackLink);
