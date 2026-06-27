namespace AccessingChildcareEntitlementChecker.E2eTests.Data;

internal interface IUseCase
{
    string Name { get; }
    IEnumerable<(string PageName, string Answer)> GetJourney();
}
