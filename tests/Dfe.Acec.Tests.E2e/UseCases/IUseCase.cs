using Dfe.Acec.Tests.E2e.UseCases.Builders;

namespace Dfe.Acec.Tests.E2e.UseCases;

internal interface IUseCase
{
    string Name { get; }
    IEnumerable<JourneyStep> GetJourney();
}
