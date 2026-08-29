using Dfe.Acec.Web.Models.Summary;

namespace Dfe.Acec.Web.Services.Summary;

public interface IPartnerSummaryBuilder
{
    IReadOnlyList<SummaryRowViewModel> BuildPartnerSummary(JourneyState journeyState);
}
