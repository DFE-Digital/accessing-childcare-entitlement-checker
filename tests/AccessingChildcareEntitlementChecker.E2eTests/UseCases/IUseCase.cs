using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases;

internal interface IUseCase
{
    string Name { get; }
    IEnumerable<JourneyStep> GetJourney();
}
