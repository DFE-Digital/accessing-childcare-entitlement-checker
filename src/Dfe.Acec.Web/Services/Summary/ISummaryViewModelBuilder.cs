using Dfe.Acec.Web.Models.Summary;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web.Services.Summary;

public interface ISummaryViewModelBuilder
{
    CheckChildDetailsViewModel BuildCheckChildDetailsViewModel(
        JourneyState journeyState,
        IUrlHelper urlHelper,
        string? childId = null,
        IReadOnlyList<string>? removedChildNames = null);

    Task<CheckAnswersViewModel> BuildCheckAnswersViewModelAsync(
        JourneyState journeyState,
        IUrlHelper urlHelper,
        string? fromChildId = null,
        IReadOnlyList<string>? removedChildNames = null);
}
