using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Models.Summary;

public record CheckChildDetailsViewModel(
    IReadOnlyList<ChildSummaryViewModel> Children,
    bool HasChildren,
    Child? LastEditedChild,
    string BackLink,
    Guid CorrelationId,
    IReadOnlyList<string> RemovedChildNames);
