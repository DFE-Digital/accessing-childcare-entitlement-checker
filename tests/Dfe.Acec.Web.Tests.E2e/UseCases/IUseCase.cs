using Dfe.Acec.Web.Tests.E2e.UseCases.Builders;

namespace Dfe.Acec.Web.Tests.E2e.UseCases;

internal interface IUseCase
{
    public string Name { get; }
    public IEnumerable<JourneyStep> GetJourney();
}
