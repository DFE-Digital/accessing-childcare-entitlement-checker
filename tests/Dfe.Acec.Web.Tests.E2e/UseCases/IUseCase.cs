using Dfe.Acec.Web.Tests.E2e.UseCases.Builders;

namespace Dfe.Acec.Web.Tests.E2e.UseCases;

internal interface IUseCase
{
    string Name { get; }
    IEnumerable<JourneyStep> GetJourney();
}
