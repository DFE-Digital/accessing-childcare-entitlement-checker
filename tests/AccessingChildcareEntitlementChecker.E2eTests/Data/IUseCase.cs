using AccessingChildcareEntitlementChecker.E2eTests.Data.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.Data;

internal interface IUseCase
{
    string Name { get; }
    IEnumerable<JourneyStep> GetJourney();
}
