using Dfe.Acec.Web.Models.Summary;

namespace Dfe.Acec.Web.Services.Summary;

public interface IUserSummaryBuilder
{
    Task<IReadOnlyList<SummaryRowViewModel>> BuildUserSummaryAsync(JourneyState journeyState);
}
