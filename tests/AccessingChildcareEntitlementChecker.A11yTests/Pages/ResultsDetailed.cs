namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ResultsDetailedPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ResultsDetailedPageHasNoAccessibilityViolations()
    {
        await CompleteJourneyToResultsDetailed();
        await EvaluatePage();
    }
}