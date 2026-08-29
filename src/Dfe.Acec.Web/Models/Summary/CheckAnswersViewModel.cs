using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Models.Summary;

public record CheckAnswersViewModel(
    IReadOnlyList<ChildSummaryViewModel> Children,
    bool HasChildren,
    Child? LastEditedChild,
    IReadOnlyList<SummaryRowViewModel> UserDetails,
    IReadOnlyList<SummaryRowViewModel> PartnerDetails,
    string BackLink,
    Guid CorrelationId,
    IReadOnlyList<string> RemovedChildNames);
